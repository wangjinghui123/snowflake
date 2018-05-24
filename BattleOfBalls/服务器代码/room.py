''' Game Room'''

import random
from player import *
from bean import *
from prick import *
from spore import *
import time
import threading
from messageType import *

gameMode = {
    'SingleMode':1,
    'TeamMode':4,
    'SurvivalMode':8,
    'CustomMode':16}

MapWidth = 4800
MapHeigth = 3600
MapDivisionX = 6
MapDivisionY = 8
MapDivisionBeanNum = 100
ScreenWidth = 800
ScreenHeight = 450

class Room(threading.Thread):
    def __init__(self,roomID,mode,server):
        threading.Thread.__init__(self)
        self.roomID = roomID
        self.beanList = []
        self.prickMap = {}
        self.sporeMap = {}
        self.playerMap = {}
        self.mode = mode
        self.server = server
        self.globalID = 0
        self.initBean()
        self.timeTotal = 100
        self.startTime = time.time()
        self.lastTime = self.startTime
        self.startAddPrickTime = 10
        self.addPrickFlag = False
        self.addPrickTimeCount = 0
        self.addPrickIntervalTime = 5
        
    def initBean(self):
        self.seed = random.randint(1,1000)
        '''for i in range(MapDivisionY):
            for j in range(MapDivisionX):
                for m in range(MapDivisionBeanNum):
                    type = random.randint(3,6)
                    color = random.randint(1,6)
                    r1 = random.randint(0,ScreenWidth-1)
                    r2 = random.randint(0,ScreenHeight-1)
                    self.beanList.append(Bean(type,color,r1,r2))'''
        for i in range(MapDivisionY):
            for j in range(MapDivisionX):
                for m in range(MapDivisionBeanNum):
                    self.beanList.append(Bean())
                    
    def addPlayer(self,accountName,gameName,vestmentID,keywordID):
        x = random.randint(20,4800)
        y = random.randint(20,3600)
        self.playerMap[accountName] = Player(gameName,vestmentID,keywordID,x,y)
    
    def hasPlayer(self,accountName):
        return self.playerMap.has_key(accountName)
    
    def run(self):
        while True:
            nowTime = time.time()
            timeCount = nowTime - self.startTime
            
            if timeCount > self.startAddPrickTime:
                self.addPrickFlag = True
            
            if self.addPrickFlag == True:
                self.addPrickTimeCount -= nowTime - self.lastTime
                if self.addPrickTimeCount <= 0:
                    self.addPrick()
                    self.addPrickTimeCount += self.addPrickIntervalTime
                
            result = {}
            result['MsgType'] = msgType['UpdateTime']
            result['TimeCount'] = timeCount
            for k in self.playerMap:    
                if self.server.clientMap.has_key(k):
                    client = self.server.clientMap[k]
                    client.serverSynMessage(result)
            if timeCount >= self.timeTotal:
                break
            self.lastTime = nowTime
            time.sleep(1) 
        
        self.server.removeRoom(self.roomID)
        print "Room(%d) close" %(self.roomID)
            
    def addSpore(self,globalID,id,x,y):
        self.sporeMap[globalID] = Spore(id,x,y)
    
    def hasSpore(self,globalID):
        return self.sporeMap.has_key(globalID)
    
    def updateSpore(self,globalID,x,y):
        if self.sporeMap.has_key(globalID):
            spore = self.sporeMap[globalID]
            spore.x = x
            spore.y = y
        
    def addPrick(self):
        x = random.randint(125,MapWidth-125)
        y = random.randint(125,MapHeigth-125)
        self.prickMap[self.globalID] = Prick(x,y)
        
        result = {}
        result['MsgType'] = msgType['AddPrick']
        result['GlobalID'] = self.globalID
        result['X'] = x
        result['Y'] = y
        for k in self.playerMap:
            if self.server.clientMap.has_key(k):
                client = self.server.clientMap[k]
                client.serverSynMessage(result)
                
        self.globalID+=1
    
    def hasPrick(self,globalID):
        return self.prickMap.has_key(globalID)
    
    def removePrick(self,prickID):
        if self.prickMap.has_key(prickID):
            self.prickMap.pop(prickID)
            print "prick:%d removed" %(prickID)
        
        
        
        
        
