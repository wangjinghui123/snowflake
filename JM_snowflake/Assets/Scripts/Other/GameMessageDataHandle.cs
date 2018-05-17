using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Qy_CSharp_NetWork.Tools.Json;
using Qy_CSharp_NetWork.Component;

public delegate void RoomVerifyCompleteDelegate(object sender, string message);
public delegate void SomeOneIsTryLoginDelegate(string receiver, MsgTryLoginRes tryLogInfo);
public delegate void SomeOneIsLoginDelegate(object sender, LoginInfo newUser);
public delegate void SomeOneIsLogoutDelegate(object sender, LogoutInfo logOutUser);
public delegate void SomeOneUpMessageDelegate(object sender, ScoreInfo scoreInfo);
public class GameMessageDataHandle : MonoBehaviour
{
    public event RoomVerifyCompleteDelegate RoomVerifyCompleteEvent;
    public void NetEventFir(object sender, LAMEventArgs evAgs)
    {
        switch (evAgs.status)
        {
            case DATA_STATUS_CODE.DATA_BUILD_ROOM_T:
                Debug.Log("收到 建房 回复 成功！！！！！  :" + evAgs.message);
                break;
            case DATA_STATUS_CODE.DATA_BUILD_ROOM_F:
                Debug.LogWarning("收到 建房 回复 失败！！！！！  :" + evAgs.message);
                break;
            case DATA_STATUS_CODE.DATA_ROOM_VERIFY_T:
                Debug.Log("收到 验证 回复 成功！！！！！  :" + evAgs.message);
                if (RoomVerifyCompleteEvent != null)
                    RoomVerifyCompleteEvent(this, evAgs.message);
                break;
            case DATA_STATUS_CODE.DATA_ROOM_VERIFY_F:
                Debug.LogWarning("收到 验证 回复 失败！！！！！  :" + evAgs.message);
                break;
            case DATA_STATUS_CODE.DATA_ROOM_STATUS_T:
                Debug.Log("收到 更新状态 回复 成功！！！！！  :" + evAgs.message);
                break;
            case DATA_STATUS_CODE.DATA_ROOM_STATUS_F:
                Debug.LogWarning("收到 更新状态 回复 失败！！！！！  :" + evAgs.message);
                break;
            case DATA_STATUS_CODE.DATA_CUSTOM_MSG_T:
                Debug.Log("收到 自定义信息 成功！！！！！  :" + evAgs.message);
                _UserMessageHandle(evAgs.message);//用户数据处理
                break;
            case DATA_STATUS_CODE.DATA_CUSTOM_MSG_F:
                Debug.LogWarning("收到 自定义信息 失败！！！！！  :" + evAgs.message);
                break;
            case DATA_STATUS_CODE.DATA_SUMMARIZE_T:
                Debug.Log("收到 汇总数据 回复 成功！！！！！  :" + evAgs.message);
                _summarizedReturn = true;
                break;
            case DATA_STATUS_CODE.DATA_SUMMARIZE_F:
                Debug.LogWarning("收到 汇总数据 回复 失败！！！！！  :" + evAgs.message);
                _summarizedReturn = true;
                break;
            case DATA_STATUS_CODE.DATA_AWARD_LIST_T:
                Debug.LogWarning("收到 奖励列表 回复 成功！！！！！  :" + evAgs.message);
                _OnGetAwadeMessage(evAgs.message);
                break;
            case DATA_STATUS_CODE.DATA_AWARD_LIST_F:
                Debug.LogWarning("收到 奖励列表 回复 失败！！！！！  :" + evAgs.message);
                _OnGetAwadeMessage(null);
                break;
            case DATA_STATUS_CODE.ERROR_TRIGGER_CODE:
                Debug.LogWarning("链接 出现 错误！！！！！  :" + evAgs.message);
                break;
            case DATA_STATUS_CODE.LINK_USER_BROKEN:
                Debug.LogWarning("收到 H5 用户断开链接的信息    !!! ----:" + evAgs.message);
                JsonData jdata = JsonTools.GetJsonData(evAgs.message);
                LogoutInfo logoutInfo = new LogoutInfo();
                logoutInfo.userId = jdata["userId"].ToString();
                _logOutInfoList.Add(logoutInfo);
                _gameMessageList.Add(logoutInfo);
                if (SomeOneIsLogoutEvent != null)
                {
                    SomeOneIsLogoutEvent(this, logoutInfo);
                }
                break;
        }
    }
    public event SomeOneIsTryLoginDelegate SomeOneTryLoginEvent;
    public event SomeOneIsLoginDelegate SomeOneIsLoginEvent;
    public event SomeOneIsLogoutDelegate SomeOneIsLogoutEvent;
    public event SomeOneUpMessageDelegate SomeOneIsUpMessageEvent;
    private List<LoginInfo> _loginInfoList = new List<LoginInfo>();
    private List<LogoutInfo> _logOutInfoList = new List<LogoutInfo>();
    public List<EventArgs> gameMessageList { get { return _gameMessageList; } }
    private List<EventArgs> _gameMessageList = new List<EventArgs>();
    private void _UserMessageHandle(string message)
    {
        JsonData jsonData = JsonTools.GetJsonData(message);
        if (jsonData["type"] != null && jsonData["data"] != null && jsonData["userId"] != null)
        {
            switch ((RECEIVE_MSG_TYPE)int.Parse(jsonData["type"].ToString()))
            {
                case RECEIVE_MSG_TYPE.TRYLOGIN:
                    Debug.Log("Receive try login information .");

                    TryLonginInfo tryInfo = new TryLonginInfo();
                    tryInfo.userId = jsonData["userId"].ToString();
                    LoginInfo tempLoginInfo = _loginInfoList.Find((tempInfo) =>
                    {
                        return tempInfo.userId == tryInfo.userId;
                    });
                    MsgTryLoginRes resMsg = new MsgTryLoginRes();
                    if (tempLoginInfo == null)
                    {
                        resMsg.SetMessage(-1, 0, 0);
                    }
                    else
                    {
                        resMsg.SetMessage(int.Parse(tempLoginInfo.modelType), 0, 0);
                    }
                    if (SomeOneTryLoginEvent != null)
                    {
                        SomeOneTryLoginEvent(tryInfo.userId, resMsg);
                    }

                    break;
                case RECEIVE_MSG_TYPE.LOGIN:
                    Debug.Log("Receive 'Init' information .");
                    if (jsonData["data"]["avatar"] == null)
                    {
                        Debug.LogWarning("'LOGIN' info [data][avatar] is null!!! check  the messge from H5 !!");
                        return;
                    }
                    if (jsonData["data"]["nickname"] == null)
                    {
                        Debug.LogWarning("'LOGIN' info [data][nickName] is null!!! check  the messge from H5 !!");
                        return;
                    }
                    if (jsonData["data"]["model"] == null)
                    {
                        Debug.LogWarning("'LOGIN' info [data][model] is null!!! check  the messge from H5 !!");
                        return;
                    }
                    LoginInfo loginInfo = new LoginInfo();
                    loginInfo.userId = jsonData["userId"].ToString();
                    loginInfo.isNpc = ((int)InitiaItIsNpc.no).ToString();
                    loginInfo.nickName = jsonData["data"]["nickname"].ToString();
                    loginInfo.headimgurl = jsonData["data"]["avatar"].ToString();
                    loginInfo.modelType = jsonData["data"]["model"].ToString();
                    _loginInfoList.Add(loginInfo);
                    if (SomeOneIsLoginEvent != null)
                    {
                        //Debug.LogWarning("登陆信息 -id-[" + loginInfo.userId + "]"
                        //                + " -name-[" + loginInfo.nickName + "]:"
                        //                + DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.fff"));
                        SomeOneIsLoginEvent(this, loginInfo);
                    }
                    else
                    {
                        _gameMessageList.Add(loginInfo);
                    }
                    break;
                case RECEIVE_MSG_TYPE.LOGOUT:
                    Debug.Log("Receive 'logout' information .");
                    LogoutInfo logoutInfo = new LogoutInfo();
                    logoutInfo.userId = jsonData["userId"].ToString();
                    _logOutInfoList.Add(logoutInfo);
                    if (SomeOneIsLogoutEvent != null)
                    {
                        SomeOneIsLogoutEvent(this, logoutInfo);
                    }
                    else
                    {
                        _gameMessageList.Add(logoutInfo);
                    }
                    break;
                case RECEIVE_MSG_TYPE.SCORE:
                    Debug.Log("Receive 'score' information .");
                    if (jsonData["data"]["steps"] == null)
                    {
                        Debug.LogWarning("'SCORE' info [data][steps] is null!!! check  the messge from H5 !!");
                        return;
                    }
                    ScoreInfo scoreInfo = new ScoreInfo();
                    scoreInfo.userId = jsonData["userId"].ToString();
                    scoreInfo.score = jsonData["data"]["steps"].ToString();
                    if (SomeOneIsUpMessageEvent != null)
                    {
                        SomeOneIsUpMessageEvent(this, scoreInfo);
                    }
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Receive game message is wrong ,check the message from H5 !!!");
        }
    }


    public delegate void Awarde_Single_Delegate(object sender, SingleAwarde singleAwarde);
    public event Awarde_Single_Delegate Awarde_Single_Event;


    public event EventHandler RcvMsgOverEvent;
    private bool _singleReturn = false;
    private bool _summarizedReturn = true;
    public string firstUserId = "";
    public void MyGameOver()
    {
        Debug.Log("is game over ???????????" + _singleReturn + _summarizedReturn);
        if (_singleReturn && _summarizedReturn)
            if (RcvMsgOverEvent != null)
            {
                Debug.Log("yes !!!!!!!!!!!!!!!!!!!!!!!!!");
                RcvMsgOverEvent(null, null);
                RcvMsgOverEvent = null;
            }
    }
    void _OnGetAwadeMessage(string awardMessage)
    {
        
        if (string.IsNullOrEmpty(awardMessage))
        {
            if (Awarde_Single_Event != null)
                Awarde_Single_Event(this, null);
            Debug.Log("奖励信息空!!!!!!!!!!!!!!!");
            MyGameOver();
            return;
        }
        try
        {
            if (Awarde_Single_Event != null)
                Awarde_Single_Event(this, null);
            //JsonData jsonData = JsonTools.GetJsonData(awardMessage);
            //Debug.Log("----单奖处理----");
            //if (jsonData[firstUserId] == null)
            //{
            //    Debug.LogWarning("未发现奖励列表中含有 userId" + firstUserId);
            //    return;
            //}
            //List<SingleAwarde> singleAwardMsgList = jsonData[firstUserId].ToObject<List<SingleAwarde>>();

            //if (singleAwardMsgList.Count > 0)//返回的单个奖励数量大于0
            //{
            //    if (Awarde_Single_Event != null)
            //        Awarde_Single_Event(this, singleAwardMsgList[0]);
            //}
            _singleReturn = true;
            MyGameOver();
        }
        catch (Exception ee)
        {
            Debug.LogWarning(ee.Message);
            _singleReturn = true;
            MyGameOver();
        }
        //else if (jsonData["type"].ToString() == "2")
        //{
        //    _multyReturn = true;
        //    Debug.Log("----多奖处理----");
        //    List<List<MultyAwarde>> multyAwardMsg = jsonData["message"].ToObject<List<List<MultyAwarde>>>();
        //    Debug.Log(multyAwardMsg[0][0].awardName);
        //    MultyAwarde msg = new MultyAwarde();
        //    // msg.awardList = multyAwardMsg;
        //    if (Awarde_Multy_Event != null)
        //        Awarde_Multy_Event(this, msg);
        //}
    }
}

class PlayerBodyCompare : IComparer<PlayerBody>
{
    public int Compare(PlayerBody one, PlayerBody two)
    {
        return two.score.CompareTo(one.score);
    }
}
