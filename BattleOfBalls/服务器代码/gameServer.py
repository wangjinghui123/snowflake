from twisted.internet.protocol import Factory,Protocol
from twisted.internet import reactor
from support import *
import json
from messageType import *
import messageHandle
from room import *

class ClientHandle(Protocol):
    def __init__(self):
        self.handshake = False
        self.GUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
        self.path = "/"
        self.index = 0
        self.clientName = "" 
         
    def dataReceived(self, data):        
        if self.handshake == False:            
            headers = {}
            buffer = bytes.decode(data)  
            
            if buffer.find('\r\n\r\n') != -1:  
                header, data = buffer.split('\r\n\r\n', 1)  
                for line in header.split("\r\n")[1:]:  
                    key, value = line.split(": ", 1)  
                    headers[key] = value  
    
                headers["Location"] = ("ws://%s%s" %(headers["Host"], self.path))  
                key = headers['Sec-WebSocket-Key']  
                token = b64encode(hashlib.sha1(str.encode(str(key + self.GUID))).digest())  
    
                handshake='HTTP/1.1 101 WebSocket Protocol Hybi-10\r\n\
Upgrade: WebSocket\r\n\
Connection: Upgrade\r\n\
Sec-WebSocket-Accept: %s\r\n\r\n' %(bytes.decode(token))
  
                self.transport.write(str.encode(str(handshake)))  
                self.handshake = True    
                print ('Socket %s Handshaken with %s success!' %(self.index, self.getId()))    
                #self.sendMessage('Welcome, ' + self.name + ' !')    
        else:                    
            if len(data) <= 0:  
                return  
            
            #print 'Data before handle: ',data
            try:
                headerLength,codeLength = get_datalength(data)
            except Exception:
                print 'data error'
                return
            
            
            if len(data) - headerLength < codeLength :
                print 'Msg bad'
                return  
            else :  
                buffer_utf8 = parse_data(data)  #解析出UTF-8编码的数据                 
                msg_unicode = str(buffer_utf8[:codeLength]).decode('utf-8', 'ignore') #解码成unicode
                
                #print 'MsgData: %s' % (msg_unicode)
                self.processMessage(msg_unicode)
                
                remainData = data[headerLength+codeLength:]
                self.dataReceived(remainData)
             
                
                                                          
    def getId(self):
        return str(self.transport.getPeer())
    
    def connectionMade(self):
        print "New connection from",self.getId()
        self.factory.addClient(self)
        
    def connectionLost(self, reason):
        print "connection lose"
        self.factory.delClient(self)
        
    def sendSingleMessage(self,msg):
        newMsg = processMessage(msg)
        self.transport.write(newMsg)
        
    def sendMessage(self,name,msg):        
        newMsg = processMessage(msg)
        #print "Send msg: %s" %(newMsg)
        self.factory.sendMsg(name,newMsg)  #以UTF-8编码发送数据
       
    def sendToAll(self,msg):
        newMsg = processMessage(msg)
        #print "Send msg: %s" %(newMsg)
        self.factory.sendAll(newMsg)
        
    def sendToOther(self,name,msg):   
        newMsg = processMessage(msg)
        #print "Send msg: %s" %(newMsg)
        self.factory.sendToOther(name,newMsg)
    
    def sendToAllOther(self,name,msg):
        newMsg = processMessage(msg)
        #print "Send msg: %s" %(newMsg)
        self.factory.sendToAllOther(name,newMsg)
        
    def processMessage(self,msg):
        try:
            msgData = json.loads(msg)
        except Exception:
            print 'catch exception in json.loads(msg)'
            return
            
        if msgData['MsgType'] == msgType['Login']:
            result = messageHandle.processLogin(msgData['Data'])
            code = result[0]
            if code==1:
                self.clientName = msgData['Data']['Name']
                self.factory.addClientMap(msgData['Data']['Name'],self)
                
            returnMsg = json.dumps(result[1])
            self.sendSingleMessage(returnMsg)
            #self.sendMessage(msgData['Data']['Name'],returnMsg)
        elif msgData['MsgType'] == msgType['Account_Info']:
            code = messageHandle.processAccountInfo(msgData['Name'])
            returnMsg = json.dumps(code)
            self.sendMessage(msgData['Name'],returnMsg)
        elif msgData['MsgType'] == msgType['ChatSend']:
            result = messageHandle.processChatSend(msgData)
            code = result[0]
            if code:pass
            
            returnMsg = json.dumps(result[1])
            self.sendToOther(msgData['Receiver'],returnMsg)
        elif msgData['MsgType'] == msgType['StartSingleGame']:
            result = messageHandle.processStartSingleGame(self.factory,msgData)
            code = result[0]
            if code:pass
            
            returnMsg = json.dumps(result[1])
            enterPlayerMsg = json.dumps(result[2])
            self.sendMessage(msgData['AccountName'],returnMsg)
            self.sendToAllOther(msgData['AccountName'], enterPlayerMsg)
        elif msgData['MsgType'] == msgType['Move']:
            result = messageHandle.processPlayerMove(msgData)
            code = result[0]
            if code:pass
            
            returnMsg = json.dumps(result[1])
            self.sendToAllOther(msgData['AccountName'], returnMsg)
        elif msgData['MsgType'] == msgType['Divide']:
            result = messageHandle.processPlayerDivide(msgData)
            code = result[0]
            if code:pass
            
            returnMsg = json.dumps(result[1])
            self.sendToAllOther(msgData['AccountName'], returnMsg)
        elif msgData['MsgType'] == msgType['UpdatePosition']:
            result = messageHandle.processUpdatePlayer(self.factory, msgData)
            code = result[0]
            if code:
                returnMsg = json.dumps(result[1])
                self.sendToAllOther(msgData['AccountName'], returnMsg)
        elif msgData['MsgType'] == msgType['SpitSpore']:
            result = messageHandle.processSpitSpore(self.factory, msgData)
            code = result[0]
            if code:
                returnMsg = json.dumps(result[1])
                self.sendToAll(returnMsg)
        elif msgData['MsgType'] == msgType['PlayerConcentrate']:
            result = messageHandle.processPlayerConcentrate(msgData)
            code = result[0]
            if code:pass
            
            returnMsg = json.dumps(result[1])
            self.sendToAllOther(msgData['AccountName'], returnMsg)
        elif msgData['MsgType'] == msgType['UpdateSpore']:
            messageHandle.processUpdateSpore(self.factory, msgData)
        elif msgData['MsgType'] == msgType['EatPrick']:
            messageHandle.processEatPrick(self.factory, msgData)
    
    def serverSynMessage(self,msg):
        returnMsg = json.dumps(msg)
        self.sendMessage(self.clientName, returnMsg)

        
class GameServer(Factory):
    protocol = ClientHandle
    
    def __init__(self):
        self.clients = []
        self.clientMap = {}
        self.roomMap = {}
        self.roomSerialID = 0
        
    def addClient(self,newClient):
        self.clients.append(newClient)
        
    def delClient(self,client):
        self.clients.remove(client)
        if self.clientMap.has_key(client.clientName):
            self.clientMap.pop(client.clientName)
            
    def addClientMap(self,name,client):
        self.clientMap[name] = client
        
    def sendAll(self,message):
        for proto in self.clients:
            proto.transport.write(message)
    
    def sendToOther(self,name,message):
        if self.clientMap.has_key(name):
            self.clientMap[name].transport.write(message)
            
    def sendToAllOther(self,name,message):
        for key,client in self.clientMap.items():
            if key != name:
                client.transport.write(message)
    
    def sendMsg(self,name,message):
        if self.clientMap.has_key(name):
            self.clientMap[name].transport.write(message)
    
    def addRoom(self,type):
        self.roomMap[self.roomSerialID] = Room(self.roomSerialID,type,self)
        self.roomMap[self.roomSerialID].start()
        self.roomSerialID +=1
        return self.roomSerialID-1
    
    def removeRoom(self,roomID):
        self.roomMap.pop(roomID)
            
factory = GameServer()   
reactor.listenTCP(8766,factory)  
print "GameServer run..." 
reactor.run() 


    
    
    
    
    
    
    
    
    
