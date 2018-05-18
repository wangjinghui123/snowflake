/***********************************************
*               Jisight Look At Me             *
*   1.Metadata:                                *
*       (1): Socket.                           *
*       (2): Json.Net.                         *
*       (3): WebRequest.                       *
************************************************/


using UnityEngine;
using System.Collections.Generic;
namespace Qy_CSharp_NetWork.Component
{
    public class JisightUnityComponent_LAM : MonoBehaviour
    {
        /// <summary>
        /// 消息事件
        /// </summary>
        public event JisightLAMEventHandler JLAM_Event_Fir;
        public string token
        {
            get { return m_token; }
            set { m_token = value; }
        }
        public string gameId
        {
            get { return m_gameId; }
            set { m_gameId = value; }
        }
        public bool needDebug
        {
            get { return m_jisightLAM.needDebug; }
            set { m_jisightLAM.needDebug = value; }
        }
        public bool isConnected
        {
            get
            {
                if (m_jisightLAM != null)
                    return m_jisightLAM.connected;
                else
                    return false;
            }
        }
        private List<LAMEventArgs> m_eventQueueList;
        private string m_token = "";
        private string m_gameId = "";
        private JisightLAM m_jisightLAM = new JisightLAM();
        private void Awake()
        {
            m_jisightLAM.JLAM_Event_Fir += m_ReqConSndRevComplete;
            m_eventQueueList = new List<LAMEventArgs>();
        }
        /// <summary>
        /// 发起链接
        /// </summary>
        /// <param name="netOption">内网、外网</param>
        /// <param name="deveiceId">设备Id</param>
        public void StartLAM(NET_OPTION netOption, int timeout, string[] deveiceId = null)
        {
            //网络设置：内网、外网
            m_jisightLAM.NetOption = netOption;


            m_jisightLAM.Start(m_token, m_gameId, timeout, deveiceId);
        }
        private void OnDestroy()
        {
            Dispose();
        }
        void Update()
        {
            if (m_eventQueueList.Count > 0)
            {
                int count = m_eventQueueList.Count;
                for (int idex = 0; idex < count; ++idex)
                {
                    if (m_eventQueueList[0] == null)
                    {
                        m_eventQueueList.RemoveAt(0);
                        continue;
                    }
                    LAMEventArgs _tempArgs = new LAMEventArgs();
                    _tempArgs.SetMessage(m_eventQueueList[0].status, m_eventQueueList[0].message);
                    if (JLAM_Event_Fir != null)
                    {
                        JLAM_Event_Fir(this, _tempArgs);
                    }
                    m_eventQueueList.RemoveAt(0);
                }
            }
        }
        private void m_ReqConSndRevComplete(object sender, LAMEventArgs evAgs)
        {
            LAMEventArgs _tempArgs = new LAMEventArgs();
            _tempArgs.SetMessage(evAgs.status, evAgs.message);
            m_eventQueueList.Add(_tempArgs);
        }
        /// <summary>
        /// 向游戏服务器推送游戏状态
        /// </summary>
        /// <param name="status">设置游戏服务器状态</param>
        /// <param name="verify">是否需要服务器回执</param>
        /// <param name="timeTag">是否携带时间戳</param>
        public void PushGameStatus(string status, bool verify = false, bool timeTag = true)
        {
            if (m_jisightLAM.connected)
                m_jisightLAM.PushGameStatus(status, verify, timeTag);
            else
                Debug.LogWarning("look at me is not connected , please check the link ");

        }
        /// <summary>
        /// 向 H5 端发送信息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="receivers">接收者的id ,如果填写["All"]则为群发</param>
        /// <param name="verify">是否需要服务器回执</param>
        /// <param name="timeTag">是否携带时间戳</param>
        public void PushMsgToOther(object msg, string[] receivers, bool verify = false, bool timeTag = true)
        {
            if (m_jisightLAM.connected)
                m_jisightLAM.PushMsgToOther(msg, receivers, verify, timeTag);
            else
                Debug.LogWarning("look at me is not connected , please check the link ");
        }
        /// <summary>
        /// 向游戏服务器发送排行榜
        /// </summary>
        /// <param name="userIdList">玩家排行表</param>
        /// <param name="verify">是否需要回执</param>
        /// <param name="timeTag">是否携带时间戳</param>
        public void PushRankListToMaster(List<string> userIdList, bool verify = false, bool timeTag = true)
        {
            if (m_jisightLAM.connected)
                m_jisightLAM.PushRankListToMaster(userIdList, verify, timeTag);
            else
                Debug.LogWarning("look at me is not connected , please check the link ");
        }
        /// <summary>
        /// 请求多个奖励
        /// </summary>
        /// <param name="userId_count_Dic"></param>
        /// <param name="timeTag"></param>
        public void PostAwardeList(Dictionary<string, int> userId_count_Dic, bool timeTag = true)
        {
            if (m_jisightLAM.connected)
                m_jisightLAM.PostAwardeList(userId_count_Dic, timeTag);
            else
                Debug.LogWarning("look at me is not connected , please check the link ");
        }

        /// <summary>
        /// 请求多个奖励
        /// </summary>
        /// <param name="userId_count_Dic"></param>
        /// <param name="timeTag"></param>
        public void PostAwardeList_leo(Dictionary<string, int> userId_count_Dic, bool timeTag = true)
        {
            if (m_jisightLAM.connected)
                m_jisightLAM.PostAwardeList_leo(userId_count_Dic, timeTag);
            else
                Debug.LogWarning("look at me is not connected , please check the link ");
        }
        /// <summary>
        /// 请求单个奖励
        /// </summary>
        /// <param name="countList"></param>
        /// <param name="timeTag"></param>
        public void PostAwardeList(List<int> countList, bool timeTag = true)
        {
            if (m_jisightLAM.connected)
                m_jisightLAM.PostAwardeList(countList, timeTag);
            else
                Debug.LogWarning("look at me is not connected , please check the link ");
        }
        /// <summary>
        /// 请求特种奖励
        /// </summary>
        /// <param name="userIdCountAwardList"></param>
        /// <param name="mysterious"></param>
        /// <param name="timeTag"></param>
        public void PushMysteriousAwardeInfo(Dictionary<string, int> userIdCountAwardList, string mysterious, bool timeTag = true)
        {
            if (m_jisightLAM.connected)
                m_jisightLAM.PostAwardeList(userIdCountAwardList, mysterious, timeTag);
            else
                Debug.LogWarning("look at me is not connected , please check the link ");
        }
        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            m_jisightLAM.Dispose();
        }
    }
}