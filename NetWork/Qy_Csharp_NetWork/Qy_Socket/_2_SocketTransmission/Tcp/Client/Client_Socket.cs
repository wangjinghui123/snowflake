/***************************************************************************
*                                 * 客户端链接类 *                         *
*                                                                          *
*   1.实例化该类，对该类进行操作，之所以不选择静态，源于其客户端可能       *
*     持有N多ClientSocket实例（考虑将来添加多个Socket的管理功能，或许      *
*     内部自我持有N多Socket。）                                            *
*   2.操作实例会触发 ‘成功’、‘失败’的，注册相关事件回调使用即可。      *
*   3.抛出异常仅限于Debug时使用，用户需保证交互时收到正确数据。            *
*                                                                          *
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.Text;
using System.Security;
using System.Linq;

namespace Qy_CSharp_NetWork.Qy_Socket.Transmission
{
    using Base;
    using Tools.Debug;
    /// <summary>
    /// 用于数据传输的结构: 该结构的定义主要用于在回调时所能排除一些回调空引用。
    /// 考虑：  如果程序结束，回调参数是否为空？若为空其执行代码必然空引用。
    ///         endConnect\endReceive\endSend所在的线程是否已被杀死？
    /// </summary>
    class SocketAnsynParam
    {
        public Socket workSocket { get; set; }

        public ISocketTransEventArgs transEventArgs { get; set; }
    }
    /// <summary>
    /// 客户端Socket类，定义了一些基础Socket操作。
    /// TODO：考虑将来对UDP、线程堵塞式、文件流的扩展。
    /// </summary>
    class ClientSocket : IClientSocketQy
    {
        /// <summary>
        /// 构造函数、参数选填。当不填写参数，采用默认值：
        /// AddressFamily.InterNetwork、SocketType.Stream、ProtocolType.Tcp
        /// </summary>
        /// <param name="addressFamily">当不填写参数，采用默认值：AddressFamily.InterNetwork</param>
        /// <param name="soketType">当不填写参数，采用默认值：SocketType.Stream</param>
        /// <param name="protocolType">当不填写参数，采用默认值：ProtocolType.Tcp</param>
        public ClientSocket(AddressFamily addressFamily = AddressFamily.InterNetwork,
                            SocketType soketType = SocketType.Stream,
                            ProtocolType protocolType = ProtocolType.Tcp)
        {
            m_clientSocket = new Socket(addressFamily, soketType, protocolType);

        }

        private System.Net.Sockets.Socket m_clientSocket;
        private IPAddress m_ipAddress;
        private IPEndPoint m_ipEndPoint;

        public event TransmissionEventHandle<ISocketQy, ISocketConEventArgs> AnsyncConnectTo_Complete_Event;
        public event TransmissionEventHandle<ISocketQy, ISocketConEventArgs> AnsyncConnectTo_Failed_Event;
        public event TransmissionEventHandle<ISocketQy, ISocketSndEventArgs> AnsyncSendMsg_Complete_Event;
        public event TransmissionEventHandle<ISocketQy, ISocketSndEventArgs> AnsyncSendMsg_Failed_Event;
        public event TransmissionEventHandle<ISocketQy, ISocketRecEventArgs> AnsyncRecMsg_Complete_Event;
        public event TransmissionEventHandle<ISocketQy, ISocketRecEventArgs> AnsyncRecMsg_Failed_Event;

        private const int RECEIVE_BUFFER_SIZE = 128;
        private List<byte> m_lastBuffer = new List<byte>();            //receive 结束后 剩余的数据缓存
        private byte[] m_tempRecvBuffer = new byte[RECEIVE_BUFFER_SIZE];        //每次 receive 回来的数据

        public bool SetEndPoint(string ip, int port)
        {
            bool paramIsRight;
            if (Regex.IsMatch(ip, @"[0-255]?.[0-255]?.[0-255]?"))
            {
                m_ipAddress = IPAddress.Parse(ip);
                if (port >= IPEndPoint.MinPort && port <= IPEndPoint.MaxPort)
                {
                    m_ipEndPoint = new IPEndPoint(m_ipAddress, port);
                    paramIsRight = true;
                }
                else
                {
                    //端口输入有误
                    paramIsRight = false;
                    DebugTool.LogWarning("--> TryConnectTo() function. 'Port' value is out of range. \n" +
                                       "Please check 'out bool : paramIsRight' to handle your 'port'.");
                }
            }
            else
            {
                //Ip输入有误
                paramIsRight = false;
                DebugTool.LogWarning(" --> TryConnectTo() function. 'Ip' format is wrong ." +
                                    "Please check 'out bool : paramIsRight' to handle your 'Ip'");
            }
            return paramIsRight;
        }

        #region <<<<<<<<<    Connect Function    >>>>>>>>>
        public void TryAnsyncConnectTo()
        {
            m_BeginConnect(m_ipEndPoint);
        }
        private void m_BeginConnect(EndPoint endPoint)
        {
            DebugTool.LogTag("<<<<<<< CON ENP IN CLIENT >>>>>>>", "*-*-*-*-*[ in ]*-*-*-*-*");

            FIRING_D_C_F _firEventType = FIRING_D_C_F.DEFAULT;
            string errorMessage = string.Empty;
            ISocketConEventArgs _conEventArgs = new ConEventArgs();
            SocketAnsynParam stateParam = new SocketAnsynParam();
            stateParam.workSocket = m_clientSocket;
            stateParam.transEventArgs = _conEventArgs;
            try
            {
                m_clientSocket.BeginConnect(endPoint,
                                            new AsyncCallback(m_ConnectCallBack),
                                            stateParam
                                            );
            }
            catch (ArgumentNullException ee)
            {
                errorMessage = "×××  ArgumentNullException  ××× :"
                                + "remoteEP is null. Error code :"
                                + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (SocketException ee)
            {
                errorMessage = "×××  SocketException  ××× :"
                                + "试图访问套接字时发生错误:"
                                + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;

            }
            catch (ObjectDisposedException ee)
            {
                errorMessage = "×××  ObjectDisposedException  ××× :"
                               + "Socket 已关闭。"
                               + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;

            }
            catch (SecurityException ee)
            {
                errorMessage = "×××  SecurityException  ××× :"
                               + "调用堆栈上部的调用方无权执行所请求的操作。"
                               + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;

            }
            catch (InvalidOperationException ee)
            {
                errorMessage = "×××  InvalidOperationException  ××× :"
                               + "Socket 为 Listen。"
                               + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;

            }
            catch (Exception ee)
            {
                errorMessage = "×××  Unknown exception  ××× :"
                               + "Firing other unknown exception !!!!!!"
                               + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }

            _conEventArgs.SetErrMessage(errorMessage);
            m_ConnectEventFiring(_firEventType, _conEventArgs, "m_BeginConnect");
        }
        private void m_ConnectCallBack(IAsyncResult asyncResult)
        {
            FIRING_D_C_F _firEventType = FIRING_D_C_F.DEFAULT;
            string errorMessage = string.Empty;
            SocketAnsynParam _ansynParam = asyncResult.AsyncState as SocketAnsynParam;
            ISocketConEventArgs _conEventArgs = _ansynParam.transEventArgs as ISocketConEventArgs;
            try
            {
                _ansynParam.workSocket.EndConnect(asyncResult);
                errorMessage = "√√√ no mistake √√√";
                _firEventType = FIRING_D_C_F.FIR_COMPLETE;
            }
            catch (ArgumentNullException ee)
            {
                errorMessage = "×××  ArgumentNullException  ×××:"
                               + " asyncResult 为 null。"
                               + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentException ee)
            {
                errorMessage = "×××  ArgumentException  ×××:"
                               + " BeginConnect 方法调用未返回 asyncResult。"
                               + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (SocketException ee)
            {
                errorMessage = "×××  SocketException  ×××:"
                               + " 试图访问套接字时发生错误。"
                               + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ObjectDisposedException ee)
            {
                errorMessage = "×××  ObjectDisposedException  ×××:"
                               + "Socket 已关闭。"
                               + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (InvalidOperationException ee)
            {
                errorMessage = "×××  InvalidOperationException  ×××:"
                               + "先前曾为异步连接调用过 EndConnect。"
                               + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (Exception ee)
            {
                errorMessage = "×××  Unknown exception  ××× :"
                                + "Firing other unknown exception !!!!!!"
                                + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            DebugTool.LogTag("<<<<<<< CON ENP IN CLIENT >>>>>>>", "*-*-*-*-*[ out ]*-*-*-*-*");
            _conEventArgs.SetErrMessage(errorMessage);
            m_ConnectEventFiring(_firEventType, _conEventArgs, "m_ConnectCallBack");
        }
        #endregion

        #region <<<<<<<<<    Send Measage Function    >>>>>>>>>

        public void TryAnsyncSendMessage(byte[] buffer, string tag)
        {
            DebugTool.LogTag(">>>>>>> SEN MSG IN CLIENT >>>>>>>", "↓↓↓↓↓↓ [ in ][" + tag + ":" + buffer.Length + "]↓↓↓↓↓↓ ", "#3487E8FF");
            m_SendMessage(buffer, tag);
        }
        private void m_SendMessage(byte[] buffer, string tag, SocketFlags flags = SocketFlags.None)
        {
            DebugTool.LogTag("m_SendMessage", " ---- in ---- ", "#3487E8FF");
            FIRING_D_C_F firEventType = FIRING_D_C_F.DEFAULT;
            string errorMessage = string.Empty;
            ISocketSndEventArgs sndEventArgs = new SndEventArgs();
            sndEventArgs.SetBuffer(buffer);
            sndEventArgs.SetTag(tag);
            SocketAnsynParam stateParam = new SocketAnsynParam();
            stateParam.workSocket = m_clientSocket;
            stateParam.transEventArgs = sndEventArgs;
            try
            {
                SocketError socketErrorCode;
                m_clientSocket.BeginSend(sndEventArgs.SendBuffer,
                                        0,
                                        sndEventArgs.BufferSize,
                                        flags,
                                        out socketErrorCode,
                                        new AsyncCallback(m_SendMsgCallBack),
                                        stateParam
                                        );
                if (socketErrorCode != SocketError.Success)
                    DebugTool.LogWarning("[SEND MESAGE] Socket error : [" + socketErrorCode.ToString() + "] ,Check it !!!!!!!");
            }
            catch (ArgumentNullException ee)
            {
                errorMessage = "×××  ArgumentNullException  ×××:"
                               + "buffers 为 null。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentException ee)
            {
                errorMessage = "×××  ArgumentException  ×××:"
                               + "buffers 为空。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (SocketException ee)
            {
                errorMessage = "×××  SocketException  ×××:"
                               + "试图访问套接字时发生错误。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ObjectDisposedException ee)
            {
                errorMessage = "×××  ObjectDisposedException  ×××:"
                               + "Socket 已关闭。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (Exception ee)
            {
                errorMessage = "×××  Unknown exception  ××× :"
                               + "Firing other unknown exception !!!!!!"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }

            sndEventArgs.SetErrMessage(errorMessage);
            m_SendMessageEventFiring(firEventType, sndEventArgs, "m_SendMessage:");

        }
        private void m_SendMsgCallBack(IAsyncResult asyncResult)
        {
            DebugTool.LogTag("m_SendMsgCallBack", " ---- in ---- ", "#3487E8FF");
            string errorMessage = string.Empty;
            SocketAnsynParam ansynParam = asyncResult.AsyncState as SocketAnsynParam;
            ISocketSndEventArgs sndEventArgs = ansynParam.transEventArgs as ISocketSndEventArgs;
            FIRING_D_C_F firEventType = FIRING_D_C_F.DEFAULT;
            try
            {
                SocketError socketErrorCode;
                int sndBufferSize = ansynParam.workSocket.EndSend(asyncResult, out socketErrorCode);
                if (socketErrorCode != SocketError.Success)
                    DebugTool.LogWarning("[SEND MESAGE CALLBACK] Socket error : [" + socketErrorCode.ToString() + "]");
                if (sndBufferSize == sndEventArgs.BufferSize)
                {
                    errorMessage = "√√√ no mistake √√√ SendComplete buffer lenth is :[" + sndBufferSize + "]";
                    firEventType = FIRING_D_C_F.FIR_COMPLETE;
                }
                else
                {
                    DebugTool.LogWarning("Real 'SEND' buffer != needSize   NeedSndLengrh ：[" + sndEventArgs.BufferSize + "]*-*-*-*-*");
                    DebugTool.LogWarning("Real 'SEND' buffer != needSize   RealSndLengrh ：[" + sndBufferSize + "]*-*-*-*-*");
                    DebugTool.LogWarning("源：." + Encoding.UTF8.GetString(sndEventArgs.SendBuffer, 0, sndEventArgs.BufferSize) + ".");
                    errorMessage = "×××  receive bytes size is wrong.  ××× :"
                                    + "需接收数据与接收数据字节不匹配!";
                    DebugTool.LogWarning(errorMessage);
                    firEventType = FIRING_D_C_F.FIR_FAILED;
                    //TODO:返回值校验的逻辑处理：查看Socket底层后再做补充 ！！！
                }
            }
            catch (ArgumentNullException ee)
            {
                errorMessage = "×××  ArgumentNullException  ×××:"
                               + "asyncResult 为 null。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentException ee)
            {
                errorMessage = "×××  ArgumentException  ×××:"
                               + "BeginSend 方法调用后未返回 asyncResult。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (SocketException ee)
            {
                errorMessage = "×××  SocketException  ×××:"
                               + "试图访问套接字时发生错误。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;

            }
            catch (ObjectDisposedException ee)
            {
                errorMessage = "×××  ObjectDisposedException  ×××:"
                               + "Socket 已关闭。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (InvalidOperationException ee)
            {
                errorMessage = "×××  InvalidOperationException  ×××"
                               + "先前为异步发送已调用过 EndSend。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (Exception ee)
            {
                errorMessage = "×××  Unknown exception  ××× :"
                              + "Firing other unknown exception !!!!!!"
                              + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            sndEventArgs.SetErrMessage(errorMessage);
            m_SendMessageEventFiring(firEventType, sndEventArgs, "m_SendMsgCallBack");
            DebugTool.LogTag(">>>>>>> SEN MSG IN CLIENT >>>>>>>", "↑↑↑↑↑↑↑ [ out ][" + sndEventArgs.Tag + ":" + sndEventArgs.BufferSize + "]↑↑↑↑↑↑↑ ", "#3487E8FF");
        }
        #endregion

        #region <<<<<<<<<    Receive Measage Fuction    >>>>>>>>>
        public void TryAnsyncReceiveMessage(int bufferSize, string tag)
        {
            DebugTool.LogTag("<<<<<<< REV MSG IN CLIENT <<<<<<<", "↓↓↓↓↓↓ [ in ][" + tag + ":" + bufferSize + "]↓↓↓↓↓↓ ", "#3C9553FF");
            DebugTool.LogTag("m_ReceiveMessage", " ---- in ---- ", "#3C9553FF");
            ISocketRecEventArgs recEventArgs = new RecEventArgs();
            recEventArgs.NeedRecvBufferSize = bufferSize;
            recEventArgs.Tag = tag;
            SocketAnsynParam stateParam = new SocketAnsynParam();
            stateParam.workSocket = m_clientSocket;
            stateParam.transEventArgs = recEventArgs;
            m_ReceiveMessage(stateParam);
        }

        private void m_ReceiveMessage(SocketAnsynParam stateParam, SocketFlags flags = SocketFlags.None)
        {
            ISocketRecEventArgs recvEventArgs = stateParam.transEventArgs as ISocketRecEventArgs;

            string errorMessage = string.Empty;
            FIRING_D_C_F firEventType = FIRING_D_C_F.DEFAULT;

            if (m_lastBuffer.Count >= recvEventArgs.NeedRecvBufferSize)              //当上一次剩余的缓存数据足够本次使用时
            {
                DebugTool.LogTag("m_ReceiveMessage", "need buffer size :[" + recvEventArgs.NeedRecvBufferSize + "]", "#3C9553FF");
                DebugTool.LogTag("m_ReceiveMessage", "last buffer length :[" + m_lastBuffer.Count + "]", "#3C9553FF");
                DebugTool.LogTag("m_ReceiveMessage", "so dont need begin receive .", "#3C9553FF");
                byte[] buffer = new byte[recvEventArgs.NeedRecvBufferSize];
                m_lastBuffer.CopyTo(0, buffer, 0, recvEventArgs.NeedRecvBufferSize);
                m_lastBuffer.RemoveRange(0, recvEventArgs.NeedRecvBufferSize);
                recvEventArgs.SetBuffer(buffer);
                errorMessage = "√√√ no mistake √√√ ";
                firEventType = FIRING_D_C_F.FIR_COMPLETE;
            }
            else
            {
                try
                {
                    SocketError socketErrorCode;
                    m_clientSocket.BeginReceive(m_tempRecvBuffer,
                                               0,
                                               m_tempRecvBuffer.Length,
                                               flags,
                                               out socketErrorCode,
                                               new AsyncCallback(m_ReceiveMsgCallBack),
                                               stateParam
                                               );
                    if (socketErrorCode != SocketError.Success)
                        DebugTool.LogWarning("[RECV MESAGE] Socket error : [" + socketErrorCode.ToString() + "] ,Check it !!!!!!!");
                }
                catch (ArgumentNullException ee)
                {
                    errorMessage = "×××  ArgumentNullException  ×××:"
                                   + "buffers 为 null。"
                                   + ee.Message;
                    firEventType = FIRING_D_C_F.FIR_FAILED;
                }
                catch (SocketException ee)
                {
                    errorMessage = "×××  SocketException  ×××:"
                                    + "试图访问套接字时发生错误。"
                                    + ee.Message;
                    firEventType = FIRING_D_C_F.FIR_FAILED;
                }
                catch (ObjectDisposedException ee)
                {
                    errorMessage = "×××  ObjectDisposedException  ×××:"
                                    + "Socket 已关闭。"
                                    + ee.Message;
                    firEventType = FIRING_D_C_F.FIR_FAILED;
                }
                catch (ArgumentOutOfRangeException ee)
                {
                    errorMessage = "×××  ArgumentOutOfRangeException  ×××:"
                                   + "offset 小于 0。-或 -offset 大于 buffer 的长度。-或 -size 小于 0。-或 -size 大于 buffer 的长度减去 offset 参数的值。"
                                   + ee.Message;
                    firEventType = FIRING_D_C_F.FIR_FAILED;
                }
                catch (Exception ee)
                {
                    errorMessage = "×××  Unknown exception  ××× :"
                                 + "Firing other unknown exception !!!!!!"
                                 + ee.Message;
                    firEventType = FIRING_D_C_F.FIR_FAILED;
                }
            }
            recvEventArgs.SetErrMessage(errorMessage);
            m_ReceiveMessageEventFiring(firEventType, recvEventArgs, "m_ReceiveMessage");

        }
        private void m_ReceiveMsgCallBack(IAsyncResult asyncResult)
        {
            DebugTool.LogTag("m_ReceiveMsgCallBack", " ---- in ---- ", "#3C9553FF");
            FIRING_D_C_F firEventType = FIRING_D_C_F.DEFAULT;
            string errorMessage = string.Empty;
            SocketAnsynParam ansynParam = asyncResult.AsyncState as SocketAnsynParam;
            ISocketRecEventArgs recEventArgs = ansynParam.transEventArgs as ISocketRecEventArgs;
            try
            {
                SocketError socketErrorCode;
                int realRevbufSize = ansynParam.workSocket.EndReceive(asyncResult, out socketErrorCode);
                if (socketErrorCode != SocketError.Success)
                    DebugTool.LogWarning("[RECV MESAGE] Socket error : [" + socketErrorCode.ToString() + "] ,Check it !!!!!!!");

                if (realRevbufSize == 0)
                {
                    errorMessage = "×××  receive bytes size is '0'  ××× :"
                                  + "远程链接已调用shutDown\\close.";
                    firEventType = FIRING_D_C_F.FIR_FAILED;
                }
                else
                {
                    DebugTool.LogTag("m_ReceiveMsgCallBack", "Current receive buffer length ：[" + realRevbufSize + "]", "#3C9553FF");
                    byte[] bytes = new byte[realRevbufSize];
                    Buffer.BlockCopy(m_tempRecvBuffer, 0, bytes, 0, realRevbufSize);
                    m_lastBuffer.AddRange(bytes);

                    if (m_lastBuffer.Count >= recEventArgs.NeedRecvBufferSize)
                    {
                        DebugTool.LogTag("m_ReceiveMsgCallBack", "Real 'RECV' buffer > needSize   NeedRecLength ：[" + recEventArgs.NeedRecvBufferSize + "]", "#3C9553FF");
                        DebugTool.LogTag("m_ReceiveMsgCallBack", "Real 'RECV' buffer > needSize   RealRecLengrh ：[" + m_lastBuffer.Count + "]", "#3C9553FF");
                        byte[] buffer = new byte[recEventArgs.NeedRecvBufferSize];
                        m_lastBuffer.CopyTo(0, buffer, 0, recEventArgs.NeedRecvBufferSize);
                        m_lastBuffer.RemoveRange(0, recEventArgs.NeedRecvBufferSize);
                        recEventArgs.SetBuffer(buffer);
                        errorMessage = "√√√ no mistake √√√ ";
                        firEventType = FIRING_D_C_F.FIR_COMPLETE;

                    }
                    else if (m_lastBuffer.Count < recEventArgs.NeedRecvBufferSize)
                    {
                        DebugTool.LogTag("m_ReceiveMsgCallBack", "Real 'RECV' buffer < needSize   NeedRecLength ：[" + recEventArgs.NeedRecvBufferSize + "]", "#3C9553FF");
                        DebugTool.LogTag("m_ReceiveMsgCallBack", "Real 'RECV' buffer < needSize   RealRecLengrh ：[" + m_lastBuffer.Count + "]", "#3C9553FF");
                        m_ReceiveMessage(ansynParam);
                        return;
                    }
                }
            }
            catch (ArgumentNullException ee)
            {
                errorMessage = "×××  ArgumentNullException  ×××:"
                              + "asyncResult 为 null。"
                              + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentException ee)
            {
                errorMessage = "×××  ArgumentException  ×××:"
                               + "方法调用后未返回 asyncResult。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ObjectDisposedException ee)
            {
                errorMessage = "×××  ObjectDisposedException  ×××:"
                               + "Socket 已关闭。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (InvalidOperationException ee)
            {
                errorMessage = "×××  InvalidOperationException  ×××:"
                              + "先前曾为异步读取调用过 EndReceive。"
                              + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (SocketException ee)
            {
                errorMessage = "×××  SocketException  ×××:"
                               + " 试图访问套接字时发生错误。"
                               + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (Exception ee)
            {
                errorMessage = "×××  Unknown exception  ××× :"
                            + "Firing other unknown exception !!!!!!"
                            + ee.Message;
                firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            DebugTool.LogTag("<<<<<<< REV MSG IN CLIENT <<<<<<<", "↑↑↑↑↑↑↑ [ out ][" + recEventArgs.Tag + ":" + recEventArgs.NeedRecvBufferSize + "]↑↑↑↑↑↑↑ ", "#3C9553FF");
            recEventArgs.SetErrMessage(errorMessage);
            m_ReceiveMessageEventFiring(firEventType, recEventArgs, "m_ReceiveMsgCallBack");
        }
        #endregion

        #region<<<<<<<<<    Firing Event    >>>>>>>>>
        private void m_ConnectEventFiring(FIRING_D_C_F firType, ISocketConEventArgs conArgs, string funcionTag)
        {
            switch (firType)
            {
                case FIRING_D_C_F.DEFAULT:

                    break;
                case FIRING_D_C_F.FIR_COMPLETE:
                    DebugTool.LogTag(funcionTag, firType.ToString(), "#59A7CEFF");
                    if (AnsyncConnectTo_Complete_Event != null)
                    {
                        ISocketConEventArgs _args = conArgs;
                        AnsyncConnectTo_Complete_Event(this, _args);
                    }
                    break;
                case FIRING_D_C_F.FIR_FAILED:
                    DebugTool.LogTag(funcionTag, firType.ToString() + conArgs.ErrMessage, "#BB7979FF");
                    if (AnsyncConnectTo_Failed_Event != null)
                    {
                        ISocketConEventArgs _args = conArgs;
                        AnsyncConnectTo_Failed_Event(this, conArgs);
                    }
                    break;
                default:

                    break;
            }
        }
        private void m_SendMessageEventFiring(FIRING_D_C_F firType, ISocketSndEventArgs sendArgs, string funcionTag)
        {
            switch (firType)
            {
                case FIRING_D_C_F.DEFAULT:
                    DebugTool.LogTag(funcionTag, "buffer has send . please wait callback.", "#3487E8FF");
                    break;
                case FIRING_D_C_F.FIR_COMPLETE:
                    DebugTool.LogTag(funcionTag, firType.ToString(), "#3487E8FF");
                    if (AnsyncSendMsg_Complete_Event != null)
                    {
                        ISocketSndEventArgs _args = sendArgs;
                        AnsyncSendMsg_Complete_Event(this, _args);
                    }

                    break;
                case FIRING_D_C_F.FIR_FAILED:
                    DebugTool.LogTag(funcionTag, firType.ToString() + sendArgs.ErrMessage, "#BB7979FF");
                    if (AnsyncSendMsg_Failed_Event != null)
                    {
                        ISocketSndEventArgs _args = sendArgs;
                        AnsyncSendMsg_Failed_Event(this, _args);
                    }
                    break;
                default:

                    break;
            }
        }
        private void m_ReceiveMessageEventFiring(FIRING_D_C_F firType, ISocketRecEventArgs recArgs, string funcionTag)
        {
            switch (firType)
            {
                case FIRING_D_C_F.DEFAULT:
                    DebugTool.LogTag(funcionTag, "begin receive . please wait callback.", "#3C9553FF");
                    break;
                case FIRING_D_C_F.FIR_COMPLETE:
                    DebugTool.LogTag(funcionTag, firType.ToString(), "#3C9553FF");
                    if (AnsyncRecMsg_Complete_Event != null)
                    {
                        ISocketRecEventArgs _args = recArgs;
                        AnsyncRecMsg_Complete_Event(this, _args);
                    }
                    break;
                case FIRING_D_C_F.FIR_FAILED:
                    DebugTool.LogTag(funcionTag, firType.ToString() + recArgs.ErrMessage, "#BB7979FF");
                    if (AnsyncRecMsg_Failed_Event != null)
                    {
                        ISocketRecEventArgs _args = recArgs;
                        AnsyncRecMsg_Failed_Event(this, _args);
                    }
                    break;
                default:

                    break;
            }
        }

        #endregion

        #region <<<<<<<<<    Dispose Fuction    >>>>>>>>>
        public void Dispose()
        {
            DebugTool.LogTag("ClientSocket", "Disposed !!!!!!!!", "#BB7979FF");
            if (m_clientSocket.Connected)
            {
                m_clientSocket.Shutdown(SocketShutdown.Both);
                m_clientSocket.Close();
            }
        }
        #endregion

    }


}
