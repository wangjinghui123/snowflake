/***************************************************************************
*                             * 信息发送接收协议*                          *
*                                                                          *
*   1.该类定义了数据发送的  格式：                                         *
*       数据包 = byte[16]:_1024_G_1024_M_1024_K_1024_B    按字符位运算     *
*                          ---- G ---- M ---- K ---- B                     *
*                                                                          *
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Qy_CSharp_NetWork.Qy_Socket.OriginalDataUserTransProtocol
{
    using Base;
    using Tools.Debug;

    class HeadBodyTcpProtocol : ITransProtocol
    {
        private const int B_STANDARD_VALUE = 1;//B对应的_单位
        private const int SCALE_OF_BYTE_SYSTEM = 1024;//进制
        private const int K_STANDARD_VALUE = B_STANDARD_VALUE * SCALE_OF_BYTE_SYSTEM;//K对应的单位
        private const int M_STANDARD_VALUE = K_STANDARD_VALUE * SCALE_OF_BYTE_SYSTEM;//M对应的单位
        private const int MAX_LENTH_OF_DATA = M_STANDARD_VALUE * SCALE_OF_BYTE_SYSTEM - 1; //数据包最大长处
        private const int HEAD_DATA_UNIT = 4;//1024数字位数：4位
        private const int HEAD_DATA_NUMB = 3;//包含 3 个1024
        private const int HEAD_BUFFER_SIZE = HEAD_DATA_UNIT * HEAD_DATA_NUMB;//头包大小
        private const string SEAM_STRING = "0";//填补字符

        private int m_M_num = 0;
        private int m_K_num = 0;
        private int m_B_num = 0;

        private enum HEAD_BODY_PACKET
        {
            HEAD = 0,
            BODY = 1
        }
        public bool IsReceiving
        {
            get
            {
                if (m_recMsgThread != null && m_recMsgThread.IsAlive)
                    return true;
                else
                    return false;
            }
        }

        private HEAD_BODY_PACKET m_nextHeadOrBody = HEAD_BODY_PACKET.HEAD;
        private int m_nextPackageSize = HEAD_BUFFER_SIZE;
        private EventWaitHandle m_recEventWaitHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
        private Thread m_recMsgThread = null;
        private string m_M_str = string.Empty;
        private string m_K_str = string.Empty;
        private string m_B_str = string.Empty;

        public event TransmissionEventHandle<ISocketQy, ISocketSndEventArgs> SendMsgCompleteEvent;
        public event TransmissionEventHandle<ISocketQy, ISocketSndEventArgs> SendMsgFailedEvent;
        public event TransmissionEventHandle<ISocketQy, ISocketRecEventArgs> RecMsgCompleteEvent;
        public event TransmissionEventHandle<ISocketQy, ISocketRecEventArgs> RecMsgFailedEvent;

        public ISocketQy TargetSocketArray
        {
            get
            {
                return m_tgWorkSocket;
            }
        }

        private ISocketQy m_tgWorkSocket;
        public void SetTargetSocket(ISocketQy workSocket)
        {
            if (workSocket == null)
            {
                DebugTool.LogException(new NullReferenceException("workSocket is null.Check your SetTargetSocket()."));
                return;
            }
            m_tgWorkSocket = workSocket;
            m_tgWorkSocket.AnsyncSendMsg_Complete_Event += m_SendMessageCompleteCallBack;
            m_tgWorkSocket.AnsyncSendMsg_Failed_Event += m_SendMessageFaliedCallBack;
            m_tgWorkSocket.AnsyncRecMsg_Complete_Event += m_ReceiveMsgCompleteCallBack;
            m_tgWorkSocket.AnsyncRecMsg_Failed_Event += m_ReceiveMsgFailedCallBack;
        }


        #region   <<<<<<<<<   SendMessageHandle    >>>>>>>>>
        private List<byte[]> m_taskList = new List<byte[]>();
        private bool isSndComplete = true;
        /// <summary>
        /// Jisight数据发送规则
        /// </summary>
        /// <param name="bytes">发送数据</param>
        public void SendMessage(byte[] dataBytes)
        {
            if (dataBytes != null)
            {
                m_taskList.Add(dataBytes);
            }
            if (isSndComplete == false || m_taskList.Count == 0)
            {
                return;
            }
            else
            {
                m_SndMessageAsQueue();
            }
        }
        private void m_SndMessageAsQueue()
        {
            isSndComplete = false;
            int bodyLength = m_taskList[0].Count();
            byte[] headBytes = m_GetHeadBytes(bodyLength);
            if (headBytes != null)
            {
                DebugTool.LogTag("Jisight_Tcp_Send Head:", headBytes.Length.ToString());
                DebugTool.LogTag("Jisight_Tcp_Send Body:", bodyLength.ToString());
                byte[] intactBytes = new byte[headBytes.Length + bodyLength];
                headBytes.CopyTo(intactBytes, 0);
                m_taskList[0].CopyTo(intactBytes, headBytes.Length);
                m_tgWorkSocket.TryAnsyncSendMessage(intactBytes);
            }
            else
            {
                DebugTool.LogWarning("SendMessageHandle() : Send data size is out of rang. So cansole send.");
            }
            m_taskList.RemoveAt(0);
            if (m_taskList.Count > 0)
            {
                m_SndMessageAsQueue();
            }
            else
            {
                isSndComplete = true;
            }
        }
        private void m_SendMessageCompleteCallBack(ISocketQy sender, ISocketSndEventArgs evetArgs)
        {
            DebugTool.LogTag("HeadBodyTcpProtocol", "Complete Call Back in 'Protocol'.");
            if (SendMsgCompleteEvent != null)
            {
                SendMsgCompleteEvent(sender, evetArgs);
            }
        }
        private void m_SendMessageFaliedCallBack(ISocketQy sender, ISocketSndEventArgs evetArgs)
        {
            DebugTool.LogTag("HeadBodyTcpProtocol", "Failed Call Back in 'Protocol'.");
            if (SendMsgFailedEvent != null)
            {
                SendMsgFailedEvent(sender, evetArgs);
            }
        }
        private byte[] m_GetHeadBytes(int lengthNum)
        {
            byte[] targetBytes;
            if (lengthNum <= MAX_LENTH_OF_DATA && lengthNum >= 0)
            {
                StringBuilder strBuilder = new StringBuilder();
                m_M_num = lengthNum / M_STANDARD_VALUE;
                m_K_num = (lengthNum - m_M_num * M_STANDARD_VALUE) / K_STANDARD_VALUE;
                m_B_num = lengthNum - m_M_num * M_STANDARD_VALUE - m_K_num * K_STANDARD_VALUE;

                strBuilder.Append(m_Get_HEAD_DATA_UNIT_Str(m_M_num));
                strBuilder.Append(m_Get_HEAD_DATA_UNIT_Str(m_K_num));
                strBuilder.Append(m_Get_HEAD_DATA_UNIT_Str(m_B_num));
                targetBytes = Encoding.UTF8.GetBytes(strBuilder.ToString());

                int tempBufferSize = targetBytes.Length;
                if (tempBufferSize != HEAD_BUFFER_SIZE)
                {
                    targetBytes = null;
                }
            }
            else
            {
                //长度超过最大限定范围
                targetBytes = null;
            }
            return targetBytes;
        }
        private string m_Get_HEAD_DATA_UNIT_Str(int number)
        {
            string strValue = number.ToString();
            if (strValue.Length < HEAD_DATA_UNIT)
            {
                int _delta = HEAD_DATA_UNIT - strValue.Length;
                for (int _strIdex = 0; _strIdex < _delta; _strIdex++)
                {
                    strValue = strValue.Insert(0, SEAM_STRING);
                }
            }
            return strValue;
        }
        #endregion

        #region   <<<<<<<<<   ReceiveMessageHandle    >>>>>>>>>
        public void ContinueReceiveMsg()
        {
            if (m_tgWorkSocket == null)
            {
                DebugTool.LogWarning("Jisight_Tcp_BeginReceive(): your param is null ,check it please.");
                return;
            }
            if (m_recMsgThread == null)
            {
                ThreadStart threadstart = new ThreadStart(
                        () =>
                        {
                            while (true)
                            {
                                if (m_tgWorkSocket != null)
                                {
                                    m_recEventWaitHandle.Reset();
                                    m_BenginReceive(m_tgWorkSocket, m_nextHeadOrBody, m_nextPackageSize);
                                }
                                else
                                {
                                    DebugTool.LogWarning("'receiveObj' in Jisight_Tcp_BeginReceive is null."
                                                       + "So stop receive message from 'receiveObj'");
                                    break;
                                }
                            }
                        });
                m_recMsgThread = new Thread(threadstart);
            }
            if (m_recMsgThread.IsAlive)
            {
                DebugTool.LogWarning("Receive funcion is working.Don't use this function again please !");
                return;
            }
            m_recMsgThread.Start();
        }
        private void m_ReceiveMsgCompleteCallBack(ISocketQy sender, ISocketRecEventArgs eventArgs)
        {
            if (eventArgs.Tag == "HEAD")
            {
                m_ReceiveHeadDataCallBack(sender, eventArgs);
            }
            else if (eventArgs.Tag == "BODY")
            {
                m_ReceiveBodyDataCallBack(sender, eventArgs);
            }
        }
        private void m_ReceiveMsgFailedCallBack(ISocketQy sender, ISocketRecEventArgs eventArgs)
        {
            if (RecMsgFailedEvent != null)
            {
                RecMsgFailedEvent(sender, eventArgs);
            }
        }

        private void m_BenginReceive(ISocketQy receiveObj, HEAD_BODY_PACKET choosePackage, int bufferSize)
        {
            switch (choosePackage)
            {
                case HEAD_BODY_PACKET.HEAD:
                    DebugTool.Log("start receive HEAD ：-------- thread.waitOne! ↓↓↓↓↓");
                    receiveObj.TryAnsyncReceiveMessage(bufferSize, "HEAD");
                    break;
                case HEAD_BODY_PACKET.BODY:
                    DebugTool.Log("start receive BODY ：-------- thread.waitOne! ↓↓↓↓↓");
                    receiveObj.TryAnsyncReceiveMessage(bufferSize, "BODY");
                    break;
                default:
                    break;
            }
            m_recEventWaitHandle.WaitOne();
        }
        private void m_ReceiveHeadDataCallBack(ISocketQy sender, ISocketRecEventArgs args)
        {
            int currentBufferSize = args.NeedRecvBufferSize;
            if (currentBufferSize != HEAD_BUFFER_SIZE)
            {
                DebugTool.LogWarning("m_ReceiveHeadDataCallBack(): rec buffer size != 'HEAD_BUFFER_SIZE' . Please check your operate in receive.");
                if (RecMsgFailedEvent != null)
                {
                    RecMsgFailedEvent(sender, args);
                }
                DebugTool.LogWarning("'I Has stop your receive function !' ------- from writer. ");
                StopReceiveMessage();
                return;
            }
            string responseStr = Encoding.UTF8.GetString(args.ReceiveBuffer, 0, args.NeedRecvBufferSize);
            DebugTool.LogTag("HeadBodyTcpProtocol", "'HEAD_PACKAGE' has received. value of the package is :[" + responseStr + "]");
            int nextBufferSize;
            bool isSuccess = m_GetIntFromStrWith_SCALE_OF_BYTE_SYSTEM(responseStr, out nextBufferSize);
            if (!isSuccess)
            {
                DebugTool.LogWarning("m_ReceiveHeadDataCallBack():the 'HeadBuffer' from sender is not a appoint value.\n"
                                   + "HeadBuffer to string :---." + responseStr + ".---");
                if (RecMsgFailedEvent != null)
                {
                    RecMsgFailedEvent(sender, args);
                }
                StopReceiveMessage();
                DebugTool.LogWarning("'I Has stop your receive function !' ------- from writer.");
            }
            else
            {
                DebugTool.LogTag("HeadBodyTcpProtocol", "'HEAD_PACKAGE' say next body size is:[" + nextBufferSize + "]");
                m_nextHeadOrBody = HEAD_BODY_PACKET.BODY;
                m_nextPackageSize = nextBufferSize;
                m_recEventWaitHandle.Set();
                DebugTool.LogTag("HeadBodyTcpProtocol", "start receive [HEAD] ：-------- thread.set      ↑↑↑↑↑\n ");
            }
        }
        private void m_ReceiveBodyDataCallBack(ISocketQy sender, ISocketRecEventArgs args)
        {
            int currentBufferSize = args.NeedRecvBufferSize;
            if (currentBufferSize != m_nextPackageSize)
            {
                DebugTool.LogWarning("m_ReceiveBodyDataCallBack(): rec buffer size != 'BODY_BUFFER_SIZE' . Please check your operate in receive.");
                DebugTool.LogWarning("'I Has stop your receive function !' ------- from writer. ");
                if (RecMsgFailedEvent != null)
                {
                    RecMsgFailedEvent(sender, args);
                }
                StopReceiveMessage();
                return;
            }
            DebugTool.Log("'BODY_PACKAGE' has received.");
            m_nextHeadOrBody = HEAD_BODY_PACKET.HEAD;
            m_nextPackageSize = HEAD_BUFFER_SIZE;
            DebugTool.Log("start receive [BODY] ：-------- thread.set      ↑↑↑↑↑\n ");
            m_recEventWaitHandle.Set();
            if (RecMsgCompleteEvent != null)
            {
                RecMsgCompleteEvent(sender, args);
            }
        }
        private bool m_GetIntFromStrWith_SCALE_OF_BYTE_SYSTEM(string fromStr, out int targetValue)
        {
            bool _n_tryPaseSuc = false;
            targetValue = 0;
            for (int idex = 0; idex < HEAD_DATA_NUMB; ++idex)
            {
                int tempValue;
                _n_tryPaseSuc = int.TryParse(fromStr.Substring(HEAD_DATA_UNIT * idex, HEAD_DATA_UNIT), out tempValue);
                if (!_n_tryPaseSuc)
                {
                    return false;
                }
                int fourInt = tempValue * (int)Math.Pow(SCALE_OF_BYTE_SYSTEM, HEAD_DATA_NUMB - 1 - idex);
                targetValue += fourInt;
            }
            DebugTool.Log("Receive Head  4_Bytes: " + targetValue);
            return true;
        }

        #endregion
        public void StopReceiveMessage()
        {
            if (m_recMsgThread != null && m_recMsgThread.IsAlive)
            {
                m_recMsgThread.Abort();
                DebugTool.Log("end receive in protocol.");
            }
        }


    }

}
