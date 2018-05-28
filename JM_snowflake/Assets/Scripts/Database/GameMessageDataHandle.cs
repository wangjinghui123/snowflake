using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Qy_CSharp_NetWork.Tools.Json;
using Qy_CSharp_NetWork.Component;


namespace WJH
{
    public enum GAME_MESSAGE_TYPE
    {
        TRY_LOGIN_RESP = 100,
        TRY_LOGOUT_RESP,
    }
    public class LoginInfo : EventArgs
    {
        private string userId;

        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                this.userId = value;
            }
        }
    }
    public class LogoutInfo : EventArgs
    {
        private string userId;

        public string UserId
        {
            get
            {
                return userId;
            }
        }
    }

    public class ScoreInfo : EventArgs
    {

    }


    public enum Message_Type
    {
        TryLogin = 200,//尝试登陆
        Login = 201,//登陆成功
        Loginout,
    }

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
                    Debug.LogError("收到 建房 回复 成功！！！！！  :" + evAgs.message);
                    break;
                case DATA_STATUS_CODE.DATA_BUILD_ROOM_F:
                    Debug.LogError("收到 建房 回复 失败！！！！！  :" + evAgs.message);
                    break;
                case DATA_STATUS_CODE.DATA_ROOM_VERIFY_T:
                    Debug.LogError("收到 验证 回复 成功！！！！！  :" + evAgs.message);
                    if (RoomVerifyCompleteEvent != null)
                        RoomVerifyCompleteEvent(this, evAgs.message);
                    break;
                case DATA_STATUS_CODE.DATA_ROOM_VERIFY_F:
                    Debug.LogError("收到 验证 回复 失败！！！！！  :" + evAgs.message);
                    break;
                case DATA_STATUS_CODE.DATA_ROOM_STATUS_T:
                    Debug.LogError("收到 更新状态 回复 成功！！！！！  :" + evAgs.message);
                    if (UpdateStatusEvent != null)
                    {
                        UpdateStatusEvent(evAgs.message);
                    }
                    else
                    {
                        Debug.LogError("无状态更新");
                    }
                    break;
                case DATA_STATUS_CODE.DATA_ROOM_STATUS_F:
                    Debug.LogError("收到 更新状态 回复 失败！！！！！  :" + evAgs.message);
                    break;
                case DATA_STATUS_CODE.DATA_CUSTOM_MSG_T:
                    Debug.LogError("收到 自定义信息 成功！！！！！  :" + evAgs.message);
                    _UserMessageHandle(evAgs.message);//用户数据处理
                                                      //UpdateLoginPlayer
                    break;
                case DATA_STATUS_CODE.DATA_CUSTOM_MSG_F:
                    Debug.LogError("收到 自定义信息 失败！！！！！  :" + evAgs.message);
                    break;
                case DATA_STATUS_CODE.DATA_SUMMARIZE_T:
                    Debug.LogError("收到 汇总数据 回复 成功！！！！！  :" + evAgs.message);
                    break;
                case DATA_STATUS_CODE.DATA_SUMMARIZE_F:
                    Debug.LogError("收到 汇总数据 回复 失败！！！！！  :" + evAgs.message);
                    break;
                case DATA_STATUS_CODE.DATA_AWARD_LIST_T:
                    Debug.LogError("收到 奖励列表 回复 成功！！！！！  :" + evAgs.message);
                    _OnGetAwadeMessage(evAgs.message);
                    break;
                case DATA_STATUS_CODE.DATA_AWARD_LIST_F:
                    Debug.LogError("收到 奖励列表 回复 失败！！！！！  :" + evAgs.message);
                    _OnGetAwadeMessage(null);
                    break;
                case DATA_STATUS_CODE.ERROR_TRIGGER_CODE:
                    Debug.LogError("链接 出现 错误！！！！！  :" + evAgs.message);
                    break;
                case DATA_STATUS_CODE.LINK_USER_BROKEN:
                    Debug.LogError("收到 H5 用户断开链接的信息    !!! ----:" + evAgs.message);
                    JsonData jdata = JsonTools.GetJsonData(evAgs.message);
                    if (LoginOut != null)
                    {
                        LoginOut(jdata);
                    }
                    break;
            }
        }

        private void _OnGetAwadeMessage(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public event Action<string> UpdateStatusEvent;


        private List<LoginInfo> _loginInfoList = new List<LoginInfo>();
        private List<LogoutInfo> _logOutInfoList = new List<LogoutInfo>();
        public List<EventArgs> gameMessageList { get { return _gameMessageList; } }
        private List<EventArgs> _gameMessageList = new List<EventArgs>();
        /// <summary>
        /// 收到玩家信息进行处理
        /// </summary>
        /// <param name="message"></param>
        private void _UserMessageHandle(string message)
        {
            JsonData jsonData = JsonTools.GetJsonData(message);
            //取得消息的头部，
            Message_Type type = (Message_Type)(int)jsonData["type"];
            Debug.LogError("消息类型" + type);
            switch (type)
            {
                case Message_Type.TryLogin:

                    string UserId = jsonData["userId"].ToString();
                    LoginInfo tempLoginInfo = PlayerModule.Instance.GetTryLogininfo(UserId);
                    MsgTryLoginRes resMsg = new MsgTryLoginRes();
                    if (tempLoginInfo == null)
                    {
                        resMsg.SetMessage(-1, 0, 0);
                    }
                    else
                    {
                        resMsg.SetMessage(-1, 0, 0);
                    }
                    if (TryLoginIn != null)
                    {
                        TryLoginIn(jsonData);
                    }
                    break;
                case Message_Type.Login:
                    if (LoginIn != null)
                    {
                        LoginIn(jsonData);
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 尝试登陆事件 
        /// </summary>
        public event Action<JsonData> TryLoginIn;
        /// <summary>
        /// 玩家选好角色登录
        /// </summary>
        public event Action<JsonData> LoginIn;

        /// <summary>
        /// 尝试登陆回复后的事件
        /// </summary>
        public Action<string, MsgTryLoginRes> TryLoginEvent;
        /// <summary>
        /// 游戏准备好了
        /// </summary>
        public event Action GameReady;
        //有玩家退出事件
        public event Action<JsonData> LoginOut;
        public event Action<string> UpdateLoginPlayer;

        /// <summary>
        /// 游戏准备好调用这个
        /// </summary>
        public void GameReadyImpl()
        {
            if (GameReady != null)
            {
                GameReady();
            }
        }
    }

}