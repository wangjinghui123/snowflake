

using System;
using System.Collections.Generic;

public enum GAME_MESSAGE_TYPE
{
    TRY_LOGIN_RESP = 100,
    GAME_RANK_LIST = 101,
    LAST_RANK_LIST = 102,
}

public class MsgTryLoginRes             //用户尝试登陆信息
{
    public int type
    {
        get { return (int)m_type; }
    }
    public object data
    {
        get { return m_data; }
    }
    private GAME_MESSAGE_TYPE m_type = GAME_MESSAGE_TYPE.TRY_LOGIN_RESP;
    private RankInfo m_data = new RankInfo();
    public void SetMessage(int modelType, int steps, int rankNum)
    {
        m_data.model = modelType;
        m_data.steps = steps;
        m_data.rankNum = rankNum;
    }
    public class RankInfo
    {
        public int model = 0;
        public int steps = 0;
        public int rankNum = 0;
    }
}
public class MsgUserRankNubResInfo                //用户排名发送信息    
{
    public int type
    {
        get { return (int)m_type; }
    }
    public object data
    {
        get { return m_data; }
    }
    private GAME_MESSAGE_TYPE m_type = GAME_MESSAGE_TYPE.GAME_RANK_LIST;
    private RankInfo m_data = new RankInfo();
    public void SetMessage(int rankNum)
    {
        m_data.rankNum = rankNum;
    }
    public class RankInfo
    {
        public int rankNum = 0;
    }
}
public class MsgLastRankList                //最终排行榜信息   
{
    public int type
    {
        get { return (int)m_type; }

        set { m_type = (GAME_MESSAGE_TYPE)value; }
    }
    public object data
    {
        get { return userIdList; }

        set { userIdList = value as List<Data>; }
    }
    private GAME_MESSAGE_TYPE m_type = GAME_MESSAGE_TYPE.LAST_RANK_LIST;
    private List<Data> userIdList = new List<Data>();
    public void SetMessage(List<PlayerBody> bodyList)
    {
        for (int idex = 0; idex < bodyList.Count; ++idex)
        {
            Data tempData = new Data();
            tempData.nickname = bodyList[idex].nickName;
            tempData.avatar = bodyList[idex].headPortriat.headPortraitUrl;
            tempData.steps = bodyList[idex].score;
            tempData.time = bodyList[idex].endTime - bodyList[idex].startTime;
            tempData.userId = bodyList[idex].userId;
            userIdList.Add(tempData);
        }
    }
    public class Data
    {
        public string nickname = "";
        public string avatar = "";
        public int steps = 0;
        public int time = 0;
        public string userId = "";
    }
}


public enum modleType
{
    Boy_Orange = 1,
    Boy_Green = 0,
    Ninja_White = 2,
    Nijia_Red = 3,
    Laohei_Green = 4,
    Laohei_Black = 5
}

public enum RECEIVE_MSG_TYPE
{
    TRYLOGIN = 200,
    LOGIN = 201,
    LOGOUT = 202,
    SCORE = 203,
}

public class LoginInfo : EventArgs
{
    public LoginInfo() { }
    public LoginInfo(string nickName, string userId, string headPortraitUrl, modleType modleType)
    {
        _nickName = nickName;
        _userId = userId;
        _headPortraitUrl = headPortraitUrl;
        _modleType = modleType;
    }
    public string isNpc { get { return ((int)_isNpc).ToString(); } set { _isNpc = (InitiaItIsNpc)int.Parse(value); } }
    private InitiaItIsNpc _isNpc = InitiaItIsNpc.no;

    public string nickName
    {
        get
        {
            return _nickName;
        }
        set
        {
            _nickName = value;
        }
    }
    private string _nickName = string.Empty;

    public string userId
    {
        get
        {
            return _userId;
        }
        set
        {
            _userId = value;
        }
    }
    private string _userId = string.Empty;

    public string headimgurl
    {
        get
        {
            return _headPortraitUrl;
        }
        set
        {
            _headPortraitUrl = value;
        }
    }
    private string _headPortraitUrl;

    public string modelType
    {
        get
        {
            return ((int)_modleType).ToString();
        }
        set
        {
            _modleType = (modleType)int.Parse(value);
        }
    }
    private modleType _modleType;
}
public class TryLonginInfo : EventArgs
{
    public string userId;
}
public class LogoutInfo : EventArgs
{
    public string userId;
}
public class ScoreInfo : EventArgs
{
    public string userId;
    public string score;

}

public class SingleAwarde
{
    public string user_id { get; set; }
    public string awardName { get; set; }
    public string record_id { get; set; }
    public string awardImg { get; set; }
    public string awardBtn { get; set; }
}


public class MultyAwarde
{
    public string awardName { get; set; }
    public string record_id { get; set; }
    public string awardImg { get; set; }
    public string awardBtn { get; set; }
    public string url_callback { get; set; }
    public string param_callback { get; set; }
    public string award_type { get; set; }
    public string award_id { get; set; }
}
