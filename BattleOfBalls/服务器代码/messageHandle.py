import MySQLdb
from messageType import *
import random
from room import *

def processLogin(data):
    name = data['Name']
    password = data['Password']
    
    print 'Player %s login' % (name)
    conn = MySQLdb.connect(user='Andy',passwd='genius',db='battle_of_balls',charset='utf8')
    cur = conn.cursor()
    cur.execute("select * from account where name = %s",(name))
    row = cur.fetchone()
    result = {'MsgType':msgType['Login_Result']}
    if row == None:
        result['NameResult'] = 0
        return (0,result)
    else:
        result['NameResult'] = 1
    
    if row[2] == password:
        result['PasswordResult'] = 1
        code = 1
    else:
        result['PasswordResult'] = 0
        code = 0
    return (code,result)
    
def processAccountInfo(data):
    name = data
    conn = MySQLdb.connect(user='Andy',passwd='genius',db='battle_of_balls',charset='utf8')
    cur = conn.cursor()
    cur.execute("select * from account where name = %s",(name))
    row = cur.fetchone()
    
    result = {'MsgType':msgType['Account_Info_Result']}
    result['Data'] = {}
    result['Data']['Password'] = row[2]
    
    cur.execute("select * from player where name = %s",(name))
    row = cur.fetchone()
    
    result['Data']['Name'] = row[0]
    result['Data']['Age'] = row[1]
    result['Data']['Sex'] = row[2]
    
    return result
    
    
def processChatSend(data):
    result = {}
    result['MsgType'] = msgType['ChatReceive']
    result['Sender'] = data['Sender']
    result['ChatMsg'] = data['ChatMsg']   
    
    return (1,result)
    
def processStartSingleGame(server,data):
    accountName = data['AccountName']
    gameName = data['GameName']
    vestmentID = data['VestmentID']
    keywordID = data['KeywordID']
    
    result = {}
    result['MsgType'] = msgType['StartSingleGame_Result']
    
    selectedID = -1
    
    for id,room in server.roomMap.items():
        if room.mode == gameMode['SingleMode']:   
            selectedID = id
            selectedRoom = room
            break
        
    if selectedID == -1:
        selectedID = server.addRoom(gameMode['SingleMode'])
        selectedRoom = server.roomMap[selectedID]
    
    result['RoomID'] = selectedID
    '''result['BeanList'] = []
    for item in selectedRoom.beanList:
        result['BeanList'].append([item.type,item.color,item.x,item.y])'''
    result['RoomSeed'] = selectedRoom.seed
    
    result['PassTime'] = time.time() - selectedRoom.startTime
    
    result['SporeList'] = []
    for id,item in selectedRoom.sporeMap.items():
        result['SporeList'].append([id,item.id,item.x,item.y])
    
    result['PrickList'] = []
    for id,item in selectedRoom.prickMap.items():
        result['PrickList'].append([id,item.x,item.y])
    
    result['RivalList'] = []
    for playerName,player in selectedRoom.playerMap.items():
        if playerName != accountName:
            tmp = {}
            tmp['AccountName'] = playerName
            tmp['GameName'] = player.name
            tmp['VestmentID'] = player.vestmentID
            tmp['KeywordID'] = player.keywordID
            tmp['vx'] = player.vx
            tmp['vy'] = player.vy
            tmp['DivisionList'] = []
            for item in player.divisionList:
                tmp['DivisionList'].append([item.x,item.y,item.score,item.vx,item.vy])
            
            result['RivalList'].append(tmp)
            
    
    
    print 'Player %s start a single game' % (accountName)
    
    if False == selectedRoom.hasPlayer(accountName):
        selectedRoom.addPlayer(accountName,gameName,vestmentID,keywordID)
    
    playerInfo = selectedRoom.playerMap[accountName]
    
    result['Player'] = {}
    result['Player']['AccountName'] = accountName
    result['Player']['GameName'] = playerInfo.name
    result['Player']['VestmentID'] = playerInfo.vestmentID
    result['Player']['KeywordID'] = playerInfo.keywordID
    result['Player']['vx'] = playerInfo.vx
    result['Player']['vy'] = playerInfo.vy
    result['Player']['DivisionList'] = []
    for item in playerInfo.divisionList:
        result['Player']['DivisionList'].append([item.x,item.y,item.score,item.vx,item.vy])
    
    enterPlayerResult = {}
    enterPlayerResult['MsgType'] = msgType['EnterPlayer']
    enterPlayerResult['PlayerInfo'] = result['Player']
    
    return (True,result,enterPlayerResult)
    
    
def processUpdatePlayer(server,data):
    accountName = data['AccountName']
    roomID = data['RoomID']
    if server.roomMap.has_key(roomID):
        player = server.roomMap[roomID].playerMap[accountName]
        player.divisionList = []
        for division in data['DivisionList']:
            player.addDivision(division[0],division[1],division[2])
            
        return (True,data)
    else:
        return (False,data)

def processPlayerMove(data):
    return(True,data)

def processPlayerDivide(data):
    return(True,data)
    
def processSpitSpore(server,data):
    roomID = data['RoomID']
    accountName = data['AccountName']
    sporeCount = data['SporeCount']
    
    result = {}
    if server.roomMap.has_key(roomID):
        globalID = server.roomMap[roomID].globalID
        server.roomMap[roomID].globalID+=sporeCount
        print 'Player %s spit spore' % (accountName)
        result['MsgType'] = msgType['SpitSporeResult']
        result['AccountName'] = data['AccountName']
        result['GlobalID'] = globalID
        result['SporeCount'] = sporeCount
        return (True,result)
    else:
        return(False,result)


def processPlayerConcentrate(data):
    return (True,data)
    
def processUpdateSpore(server,data):
    roomID = data['RoomID']
    sporeList = data['SporeList']
    
    if server.roomMap.has_key(roomID):
        room = server.roomMap[roomID]
        for globalID,id,x,y in sporeList:
            if room.hasSpore(globalID):
                room.updateSpore(globalID,x,y)
            else:
                room.addSpore(globalID,id,x,y)
                
def processEatPrick(server,data):
    roomID = data['RoomID']
    prickID = data['GlobalID']
    
    if server.roomMap.has_key(roomID):
        room = server.roomMap[roomID]
        room.removePrick(prickID)
            
    
    
    
