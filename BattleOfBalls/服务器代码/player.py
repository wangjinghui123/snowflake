
class PlayerDivision:
    def __init__(self,name,x,y,vestmentID,keywordID,score,vx,vy):
        self.name = name
        self.x = x
        self.y = y
        self.vestmentID = vestmentID
        self.keywordID = keywordID
        self.score = score
        self.vx = vx
        self.vy = vy 
        
class Player:
    def __init__(self,name,vestmentID,keywordID,x,y):
        self.name = name
        self.vestmentID = vestmentID
        self.keywordID = keywordID
        self.vx = 0
        self.vy = 0 
        self.divisionList = []
        self.addDivision(x,y)
        
    def addDivision(self,x,y,score = 10):
        self.divisionList.append(PlayerDivision(self.name,x,y,self.vestmentID,self.keywordID,score,self.vx,self.vy))
        
    