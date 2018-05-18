using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.Component
{
    using Qy_Socket;
    using WebNet;
    using Tools.Debug;
    using DataFormat.JisightLAM;
    using Tools.Json;
    using Tools.Other;
    using System.Threading;

    public class JisightLAM
    {
        public JisightLAM()
        {
            m_SocketClientController.FirSomeEvent += m_SocketEventFirCallBack;
            m_EasyToUse_Web.RequestCompleteEvent += m_RequestComleteCallBack;
            m_EasyToUse_Web.RequestFailedEvent += m_RequestFailedCallBack;
        }
        /******************************************
         *          网络选项：内网、外网          *
         ******************************************/
        public NET_OPTION NetOption { get { return m_netOption; } set { m_netOption = value; } } /* pubulic  网络选项 */
        private NET_OPTION m_netOption = NET_OPTION.LAN;
        private const string POST_URL_LAN = @"http://192.168.0.102/records/create";
        //private const string POST_URL_WAN = @"http://www.lookatme-plus.com/records/create";
        private const string POST_URL_WAN = @"http://game.jm-cbox.com/records/create";

        //

        //private const string WEB_BASE_URL = @"http://www.lookatme-plus.com/wx?"; //码
        private const string WEB_BASE_URL = @"http://game.jm-cbox.com/wx?"; //码


        /******************************************
         *          底层操作项：request\socket    *
         ******************************************/
        private WebEasyHttpQy m_EasyToUse_Web = new WebEasyHttpQy();
        private SocketClientController m_SocketClientController =
                          new SocketClientController(CLIENT_SOCKET_TYPE.TCP,
                                                     SOCKET_DATA_PROTOCOL.HEAD_BODY);
        /******************************************
         *                  事件项              *
         ******************************************/
        public JisightLAMEventHandler JLAM_Event_Fir;                             /* pubulic  链接错误 触发事件 */
        private MsgFromMaster m_masterMsg;

        /******************************************
         *           数据处理格式选择             *
         ******************************************/
        private JsonDataHandle m_datahandle = new JsonDataHandle();
        /******************************************
         *                输出控制                *
         ******************************************/

        public bool needDebug
        {
            get { return DebugTool.NeedLog; }
            set { DebugTool.NeedLog = value; }
        }
        /******************************************
         *                用户参数                *
         ******************************************/
        private string m_token = "";
        private string m_gameId = "";
        private string m_deviceId = "";
        /******************************************
         *                是否链接                *
         ******************************************/
        public bool connected { get { return m_isConnected; } }
        private bool m_isConnected = false;
        /// <summary>
        /// 启动 Look At Me 服务
        /// </summary>
        /// <param name="url">接口Url</param>
        /// <param name="token">调用方的授权码</param>
        /// <param name="gameId">游戏不同，该值由游戏制作方确定</param>
        /// <param name="deviceIds">设备编号，如果不填自动获取 MAC</param>
        public void Start(string token, string gameId, int timeout, string[] deviceIds = null)
        {
            string _url = "";
            m_token = token;
            m_gameId = gameId;
            switch (m_netOption)
            {
                case NET_OPTION.LAN:
                    _url = POST_URL_LAN;
                    break;
                case NET_OPTION.WAN:
                    _url = POST_URL_WAN;
                    break;
            }
            SndToMasterData sndMessage = null;
            #region   若填写deviceIds，则以填写的值为准，否则自动获取该机器Mac；
            if (deviceIds == null || deviceIds.Length == 0)
            {
                string[] _macOrDevice = GetMacAdress.GetAllMacs();
                sndMessage = new SndToMasterData(token,
                                                 gameId,
                                                 _macOrDevice,
                                                DEVICED_TYPD.MACS);
            }
            else
            {
                switch (deviceIds.Length)
                {
                    case 1:
                        sndMessage = new SndToMasterData(m_token,
                                                         m_gameId,
                                                         deviceIds,
                                                         DEVICED_TYPD.DEVICE_ONE);
                        break;
                    case 2:
                        sndMessage = new SndToMasterData(m_token,
                                                         m_gameId,
                                                         deviceIds,
                                                         DEVICED_TYPD.DEVICE_TWO);
                        break;
                    case 3:
                        sndMessage = new SndToMasterData(m_token,
                                                         m_gameId,
                                                         deviceIds,
                                                         DEVICED_TYPD.DEVICE_THREE);

                        break;
                    default:
                        sndMessage = new SndToMasterData(m_token,
                                                         m_gameId,
                                                         deviceIds,
                                                         DEVICED_TYPD.DEVICE_ONE);
                        DebugTool.LogWarning("deviceIds is wrong.");
                        string argStr = "deviceIds param is wrong.";
                        m_DataEventTrigger(DATA_STATUS_CODE.ERROR_TRIGGER_CODE, argStr);
                        return;
                }
            }
            #endregion
            string msg = JsonTools.ToJson(sndMessage);
            m_PostRequest(_url, msg, timeout);
        }

        #region   <<<<<<   Request   >>>>>>>
        private void m_PostRequest(string url, string message, int timeout)
        {
            DebugTool.Log("Post to master ：" + message);
            m_EasyToUse_Web.PostMessage(url, message, timeout);
        }
        private void m_RequestComleteCallBack(object sender, WebRequestArgsQy eventArgs)
        {
            DebugTool.Log("Request CallBack .");
            bool isMesRight = m_datahandle.WebDataHandle(eventArgs.Message, out m_masterMsg);
            if (isMesRight)
            {
                m_deviceId = m_masterMsg.deviceId;
                m_DataEventTrigger(DATA_STATUS_CODE.DATA_BUILD_ROOM_T,
                                   "Build room success.: your device：" + m_deviceId + "\n"
                                   + "RoomId : " + m_masterMsg.roomId + "\n"
                                   + "gameSign : " + m_masterMsg.gameSign);
                //主控回调数据正确，可触发成功事件
                StartSocketNetWork(m_masterMsg.WMIp, int.Parse(m_masterMsg.WMPort));
            }
            else
            {
                //主控回调数据错误，可触发失败事件
                m_DataEventTrigger(DATA_STATUS_CODE.DATA_BUILD_ROOM_F, "Build room failed : " + eventArgs.Message);
            }
        }
        private void m_RequestFailedCallBack(object sender, WebRequestArgsQy eventArgs)
        {
            m_LinkFailedEventTrigger((int)LINK_STATUS_CODE.REQUEST_F);
        }
        #endregion

        #region   <<<<<<   Socket   >>>>>>>
        public void StartSocketNetWork(string ip, int port)
        {
            ////TODO:Test;
            //string str = "{\"status\":\"0\",\"message\":{ \"WMIp\":\"192.168.0.110\",\"WMPort\":\"4236\",\"roomId\":\"d20160808143002706[*LSR*]3d2aadba4c7f82a36\",\"gameSign\":\"0949bf8aeb3030e2f19db48e6d5d13e3\",\"deviceId\":\"d20160808143002706\"}}";
            //bool isMesRight = datahandle.WebDataHandle(str, out m_masterMsg);
            ////TODO:Test;

            m_SocketClientController.StartConnected(ip, port);
        }
        /// <summary>
        /// 事件选择器
        /// </summary>
        /// <param name="sender">私有变量m_SocketClientController</param>
        /// <param name="args">应用层参数</param>
        private void m_SocketEventFirCallBack(object sender, SocketClieCtrlEventArgs args)
        {
            switch (args.EventType)
            {
                case EVENT_TYPE.DEFAULT:
                    DebugTool.LogError("【LAM】m_SocketSendEventHandle" + "EVENT HANDLE:");
                    break;
                case EVENT_TYPE.CONNECT:
                    m_SocketConnectedEventHandle(args.IsComplete, args.Message);
                    break;
                case EVENT_TYPE.SEND:
                    m_SocketSendEventHandle(args.IsComplete, args.Message);
                    break;
                case EVENT_TYPE.RECEIVE:
                    m_SocketReceiveEventHandle(args.IsComplete, args.Message);
                    break;
                default:
                    break;
            }
        }

        private void m_SocketConnectedEventHandle(COMPLETE_OR_FAILED isComplete, string message)
        {
            DebugTool.LogTag("【LAM】m_SocketConnectedEventHandle", "---CON CALL BACK:" + message);
            switch (isComplete)
            {
                case COMPLETE_OR_FAILED.DEFAULT:
                    DebugTool.LogError("【LAM】m_SocketConnectedEventHandle" + "---CON CALL BACK:" + message);
                    break;
                case COMPLETE_OR_FAILED.COMPLETE:
                    m_SocketClientController.ReceiveMessage();
                    m_PushRoomVerification();
                    break;
                case COMPLETE_OR_FAILED.FAILED:
                    m_LinkFailedEventTrigger((int)LINK_STATUS_CODE.SOCKET_CON_F);
                    break;
            }
        }

        private void m_SocketSendEventHandle(COMPLETE_OR_FAILED isComplete, string message)
        {
            DebugTool.LogTag("【LAM】m_SocketSendEventHandle", "SND CALL BACK:" + message);
            switch (isComplete)
            {
                case COMPLETE_OR_FAILED.DEFAULT:
                    DebugTool.LogError("【LAM】m_SocketSendEventHandle" + "---SND CALL BACK:" + message);
                    break;
                case COMPLETE_OR_FAILED.COMPLETE:
                    //socket数据处理器直接处理数据
                    break;
                case COMPLETE_OR_FAILED.FAILED:
                    //发送失败逻辑处理
                    m_LinkFailedEventTrigger((int)LINK_STATUS_CODE.SOCKET_SND_F);
                    break;
            }
        }
        private void m_SocketReceiveEventHandle(COMPLETE_OR_FAILED isComplete, string message)
        {
            DebugTool.LogTag("【LAM】m_SocketReceiveEventHandle", "REC CALL BACK:" + message);
            switch (isComplete)
            {
                case COMPLETE_OR_FAILED.DEFAULT:
                    DebugTool.LogError("【LAM】m_SocketReceiveEventHandle" + "---REC CALL BACK:" + message);
                    break;
                case COMPLETE_OR_FAILED.COMPLETE:
                    ClearDataFormat _clearData;
                    bool _isSucc = m_datahandle.SocketStrDataHandle(message, out _clearData);
                    //if (_clearData.Type != REV_MSG_TYPE.HEART_BEAT)
                    //{
                    //    DebugTool.LeoLog("<color=red>>>>>>>>:" + message + "</color>");
                    //}
                    if (_isSucc)
                    {
                        switch (_clearData.Type)
                        {
                            case REV_MSG_TYPE.DEFAULT:
                                DebugTool.LogError("【LAM】case :0" + "---REC CALL BACK:" + message + "\n"
                                                    + "type is not clear.");
                                break;
                            case REV_MSG_TYPE.HEART_BEAT:                      //20000 ---- 收到心跳信息
                                DebugTool.Log("【LAM】case :20000" + "HeatBeat:");
                                m_HeartBeatReturn(_clearData.Time);
                                return;
                            case REV_MSG_TYPE.ROOM_VERIFICATION_T:              //21100 ---- 收到房间验证 正确
                                DebugTool.LogTag("【LAM】case :21100", "Room verification right.");
                                string codeStr = WEB_BASE_URL + "deviceId=" + m_deviceId + "&gameId=" + m_gameId + "&socketType=1";
                                m_isConnected = true;
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_ROOM_VERIFY_T, codeStr);
                                break;
                            case REV_MSG_TYPE.ROOM_VERIFICATION_F:              //21101 ---- 收到房间验证 错误
                                DebugTool.LogTag("【LAM】case :21101", "Room verification wrong.");
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_ROOM_VERIFY_F, "Verification failed.");
                                break;
                            case REV_MSG_TYPE.AWARD_LIST_T:                     //21200 ---- 收到 奖励申请 正确
                                DebugTool.LogTag("【LAM】case :21200", "奖励申请 回复");
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_AWARD_LIST_T, _clearData.Msg);
                                break;
                            case REV_MSG_TYPE.AWARD_LIST_F:                     //21201 ---- 收到 奖励申请 错误
                                DebugTool.LogTag("【LAM】case :21201", "奖励申请 回复");
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_AWARD_LIST_F, "Request award list failed.");
                                break;
                            case REV_MSG_TYPE.ROOM_STATUS_T:                    //22000 ---- 房间状态推送 正确
                                DebugTool.LogTag("【LAM】case :22000", "房间状态 回复");
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_ROOM_STATUS_T, _clearData.Msg);
                                break;
                            case REV_MSG_TYPE.ROOM_STATUS_F:                    //22001 ---- 房间状态推送 错误
                                DebugTool.LogTag("【LAM】case :22001", "房间状态 回复");
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_ROOM_STATUS_F, "Push room status failed.");
                                break;
                            case REV_MSG_TYPE.SUMMARIZE_T:                      //22100 ---- 收到 排行榜 正确
                                DebugTool.LogTag("【LAM】case :22100", "汇总数据 回复");
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_SUMMARIZE_T, _clearData.Msg);
                                break;
                            case REV_MSG_TYPE.SUMMARIZE_F:                      //22101 ---- 收到 排行榜 错误
                                DebugTool.LogTag("【LAM】case :22101", "汇总数据 回复");
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_SUMMARIZE_F, "Push rank list failed.");
                                break;
                            case REV_MSG_TYPE.PC_H5_T:                          //40000 ---- 用户数据 正确
                                DebugTool.LogTag("【LAM】case :40000", "用户数据 回复");
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_CUSTOM_MSG_T, _clearData.Msg);
                                break;
                            case REV_MSG_TYPE.PC_H5_F:                          //40001 ---- 用户数据 错误
                                DebugTool.LogTag("【LAM】case :40001", "用户数据 回复");
                                m_DataEventTrigger(DATA_STATUS_CODE.DATA_CUSTOM_MSG_F, "Recv user message failed.");
                                break;
                            case REV_MSG_TYPE.H5_LINK_BROKEN:                        //23000 ---- H5 断链 链接
                                DebugTool.LogTag("【LAM】case :23000", "用户断开链接信息");
                                m_DataEventTrigger(DATA_STATUS_CODE.LINK_USER_BROKEN, _clearData.Msg);
                                break;
                            default:
                                DebugTool.LogWarning("【LAM】Can not clear the type");
                                break;
                        }
                    }
                    else
                    {
                        DebugTool.LogWarning("m_SocketSendEventHandle: Some format is wrong.check the message :" + message);
                    }
                    break;
                case COMPLETE_OR_FAILED.FAILED:
                    UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!:" + message);
                    //接收失败逻辑处理
                    m_LinkFailedEventTrigger((int)LINK_STATUS_CODE.SOCKET_REV_F);
                    break;
            }
        }

        #endregion

        private void m_HeartBeatReturn(string timeTag)
        {

            MsgPushHeartBeat heartBeat = new MsgPushHeartBeat();
            heartBeat.SetTime("[Sever]:" + timeTag + " [Client]:" + DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.fff"));
            heartBeat.SetReceiver(new string[] { "GameSever" });
            m_SocketClientController.SendMessage(heartBeat.ToJson());
        }
        private void m_PushRoomVerification()
        {
            MsgPostRoomVerification _roomMsg = new MsgPostRoomVerification(
                                                    m_masterMsg.deviceId,
                                                    m_masterMsg.roomId,
                                                    m_masterMsg.gameSign);
            _roomMsg.SetTime();
            _roomMsg.SetReceiver(new string[] { "GameSever" });
            m_SocketClientController.SendMessage(_roomMsg.ToJson());
        }
        public void PushGameStatus(string status, bool verify = false, bool timeTag = true)
        {
            MsgPushGameStatus gameStatus = new MsgPushGameStatus(status);
            if (verify)
                gameStatus.SetVerify(NEED_VERIFY.NEED_REPLY);
            if (timeTag)
                gameStatus.SetTime();
            gameStatus.SetReceiver(new string[] { "GameSever" });
            m_SocketClientController.SendMessage(gameStatus.ToJson());
        }
        public void PushMsgToOther(object msg, string[] receivers, bool verify = false, bool timeTag = true)
        {
            MsgPushMsgToOther gameMsg = new MsgPushMsgToOther(msg);
            if (verify)
                gameMsg.SetVerify(NEED_VERIFY.NEED_REPLY);
            if (timeTag)
                gameMsg.SetTime();
            gameMsg.SetReceiver(receivers);
            m_SocketClientController.SendMessage(gameMsg.ToJson());
        }
        public void PushRankListToMaster(List<string> userIdList, bool verify = false, bool timeTag = true)
        {
            Dictionary<int, string> rank_userId_dic = new Dictionary<int, string>();
            for (int idex = 0; idex < userIdList.Count; ++idex)
            {
                rank_userId_dic.Add(idex + 1, userIdList[idex]);
            }
            MsgPushRankList rankListMsg = new MsgPushRankList(rank_userId_dic);
            if (verify)
                rankListMsg.SetVerify(NEED_VERIFY.NEED_REPLY);
            if (timeTag)
                rankListMsg.SetTime();
            string msg = rankListMsg.ToJson();
            m_SocketClientController.SendMessage(msg);
        }

        public void PostAwardeList(Dictionary<string, int> userId_count_Dic, bool timeTag = true)
        {
            MsgPostAwadeList awardListInfo = new MsgPostAwadeList(userId_count_Dic);
            if (timeTag)
                awardListInfo.SetTime();
            awardListInfo.SetReceiver(new string[] { "HostSever" });
            m_SocketClientController.SendMessage(awardListInfo.ToJson());
        }
        public void PostAwardeList_leo(Dictionary<string, int> userId_count_Dic, bool timeTag = true)
        {
            MsgPostAwadeList_Leo awardListInfo = new MsgPostAwadeList_Leo(userId_count_Dic);
            if (timeTag)
                awardListInfo.SetTime();
            awardListInfo.SetReceiver(new string[] { "HostSever" });
            //DebugTool.LeoLog(awardListInfo.ToJson());
            m_SocketClientController.SendMessage(awardListInfo.ToJson());
        }
        public void PostAwardeList(List<int> countList, bool timeTag = true)
        {
            MsgPostAwadeList awardListInfo = new MsgPostAwadeList(countList);
            if (timeTag)
                awardListInfo.SetTime();
            awardListInfo.SetReceiver(new string[] { "HostSever" });
            m_SocketClientController.SendMessage(awardListInfo.ToJson());
        }
        public void PostAwardeList(Dictionary<string, int> userIdCountAwardList, string mysterious, bool timeTag = true)
        {
            MsgPostAwadeList awardListInfo = new MsgPostAwadeList(userIdCountAwardList, mysterious);
            if (timeTag)
                awardListInfo.SetTime();
            awardListInfo.SetReceiver(new string[] { "HostSever" });
            m_SocketClientController.SendMessage(awardListInfo.ToJson());
        }


        #region <<<<<<   EventTrigger   >>>>>>>
        private void m_DataEventTrigger(DATA_STATUS_CODE status, string msg)
        {
            LAMEventArgs eventArgs = new LAMEventArgs();
            eventArgs.SetMessage(status, msg);
            if (JLAM_Event_Fir != null)
                JLAM_Event_Fir(this, eventArgs);
        }
        private void m_LinkFailedEventTrigger(int msg)
        {
            DATA_STATUS_CODE status = DATA_STATUS_CODE.ERROR_TRIGGER_CODE;
            m_isConnected = false;
            LAMEventArgs eventArgs = new LAMEventArgs();
            eventArgs.SetMessage(status, msg);
            if (JLAM_Event_Fir != null)
                JLAM_Event_Fir(this, eventArgs);
        }
        #endregion

        public void Dispose()
        {
            m_SocketClientController.Dispose();
        }

    }
}
