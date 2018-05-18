using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Qy_CSharp_NetWork.Qy_Socket
{
    using Base;
    using Tools.Debug;
    using Transmission;
    using OriginalDataUserTransProtocol;
    using System.Threading;
    class SocketClientController
    {
        public SocketClientController(CLIENT_SOCKET_TYPE socketType, SOCKET_DATA_PROTOCOL protocolType)
        {
            //链接选择：未来改枚举
            switch (socketType)
            {
                case CLIENT_SOCKET_TYPE.TCP:
                    m_clientSocket = new ClientSocket();
                    break;
                case CLIENT_SOCKET_TYPE.UDP:
                    break;
                default:
                    break;
            }
            if (m_clientSocket == null)
                DebugTool.LogError("m_clientSocket is null.");
            //链接事件
            m_clientSocket.AnsyncConnectTo_Complete_Event += m_StartConnectedCompleteCallBack;
            m_clientSocket.AnsyncConnectTo_Failed_Event += m_StartConnectedFailedCallBack;
            //协议注册：未来改枚举
            switch (protocolType)
            {
                case SOCKET_DATA_PROTOCOL.HEAD_BODY:
                    m_transProtocol = new HeadBodyTcpProtocol();
                    break;
                default:
                    break;
            }
            m_transProtocol.SetTargetSocket(m_clientSocket);
            //协议事件
            m_transProtocol.SendMsgCompleteEvent += m_ProtocolSendCompleteCallback;
            m_transProtocol.SendMsgFailedEvent += m_ProtocolSendFailedCallback;
            m_transProtocol.RecMsgCompleteEvent += m_ProtocolRecvCompleteCallback;
            m_transProtocol.RecMsgFailedEvent += m_ProtocolRecvFailedCallback;
        }

        public event ClientCotrollerEventDelegate FirSomeEvent;

        private IClientSocketQy m_clientSocket;
        private ITransProtocol m_transProtocol;
        public void StartConnected(string ip, int port)
        {
            m_clientSocket.SetEndPoint(ip, port);
            m_clientSocket.TryAnsyncConnectTo();
        }
        public void SendMessage(string sndMsg)
        {
            DebugTool.LogTag("SocketClientController", "Ready to send in SocketController ：" + sndMsg);
            byte[] buffer = Encoding.UTF8.GetBytes(sndMsg);
            m_transProtocol.SendMessage(buffer);
        }
        public void ReceiveMessage()
        {
            m_transProtocol.ContinueReceiveMsg();
        }
        public void Dispose()
        {
            m_transProtocol.StopReceiveMessage();
            m_clientSocket.Dispose();
        }


        /// <summary>
        /// 非协议内连接事件回调
        /// </summary>
        /// <param name="sender">触发该事件的SocketQY</param>
        /// <param name="eventArgs">参数</param>
        private void m_StartConnectedCompleteCallBack(ISocketQy sender, ISocketTransEventArgs eventArgs)
        {
            DebugTool.LogTag("ClientController", "ConnectedCompleteCallBack:" + eventArgs.ErrMessage);
            m_FirSomeEvent(EVENT_TYPE.CONNECT, COMPLETE_OR_FAILED.COMPLETE, eventArgs.ErrMessage);
        }
        /// <summary>
        /// 非协议内连接事件回调
        /// </summary>
        /// <param name="sender">触发该事件的SocketQY</param>
        /// <param name="eventArgs">参数</param>
        private void m_StartConnectedFailedCallBack(ISocketQy sender, ISocketTransEventArgs eventArgs)
        {
            DebugTool.LogTag("ClientController", "StartConnectedFailedCallBack:" + eventArgs.ErrMessage);
            m_FirSomeEvent(EVENT_TYPE.CONNECT, COMPLETE_OR_FAILED.FAILED, eventArgs.ErrMessage);

        }
        /// <summary>
        /// 协议发送事件失败回调
        /// </summary>
        /// <param name="sender">触发该事件的SocketQY</param>
        /// <param name="eventArgs">参数</param>
        private void m_ProtocolSendCompleteCallback(ISocketQy sender, ISocketSndEventArgs eventArgs)
        {
            DebugTool.LogTag("ClientController", "Send Complete CallBack:" + eventArgs.ErrMessage);
            m_FirSomeEvent(EVENT_TYPE.SEND, COMPLETE_OR_FAILED.COMPLETE, eventArgs.ErrMessage);

        }
        /// <summary>
        /// 协议发送事件失败回调
        /// </summary>
        /// <param name="sender">触发该事件的SocketQY</param>
        /// <param name="eventArgs">参数</param>
        private void m_ProtocolSendFailedCallback(ISocketQy sender, ISocketSndEventArgs eventArgs)
        {
            DebugTool.LogTag("ClientController", "Send Failed CallBack:" + eventArgs.ErrMessage);
            m_FirSomeEvent(EVENT_TYPE.SEND, COMPLETE_OR_FAILED.FAILED, eventArgs.ErrMessage);

        }
        /// <summary>
        /// 协议发送事件失败回调
        /// </summary>
        /// <param name="sender">触发该事件的SocketQY</param>
        /// <param name="eventArgs">参数</param>
        private void m_ProtocolRecvCompleteCallback(ISocketQy sender, ISocketRecEventArgs eventArgs)
        {
            string recStr = Encoding.ASCII.GetString(eventArgs.ReceiveBuffer);
            DebugTool.LogTag("ClientController", "Recv Complete CallBack:" + recStr);
            m_FirSomeEvent(EVENT_TYPE.RECEIVE, COMPLETE_OR_FAILED.COMPLETE, recStr);

        }
        /// <summary>
        /// 协议发送事件失败回调
        /// </summary>
        /// <param name="sender">触发该事件的SocketQY</param>
        /// <param name="eventArgs">参数</param>
        private void m_ProtocolRecvFailedCallback(ISocketQy sender, ISocketRecEventArgs eventArgs)
        {
            DebugTool.LogTag("ClientController", "Recv Failed CallBack:" + eventArgs.ErrMessage);
            m_FirSomeEvent(EVENT_TYPE.RECEIVE, COMPLETE_OR_FAILED.FAILED, eventArgs.ErrMessage);

        }
        private void m_FirSomeEvent(EVENT_TYPE eventType, COMPLETE_OR_FAILED isComplete, string message)
        {
            if (FirSomeEvent != null)
            {
                SocketClieCtrlEventArgs eventArgs = new SocketClieCtrlEventArgs();
                eventArgs.SetParam(eventType, isComplete, message);
                FirSomeEvent(this, eventArgs);
            }
        }


    }
}
