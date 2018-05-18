using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;


namespace Qy_CSharp_NetWork.WebNet.Transmission
{
    using Base;
    using Tools.Debug;
    class HttpWebRequestQy : IWebRequestQy
    {
        public event TransmissionEventHandle<IWebRequestQy, IPostEventArgs> PostCompleteEvent;
        public event TransmissionEventHandle<IWebRequestQy, IPostEventArgs> PostFailedEvent;
        public event TransmissionEventHandle<IWebRequestQy, IGetEventArgs> GetCompleteEvent;
        public event TransmissionEventHandle<IWebRequestQy, IGetEventArgs> GetFailedEvent;
        public event TransmissionEventHandle<IWebRequestQy, IPushEventArgs> PushCompleteEvent;
        public event TransmissionEventHandle<IWebRequestQy, IPushEventArgs> PushFailedEvent;
        public event TransmissionEventHandle<IWebRequestQy, IDeleteEventArgs> DeleteCompleteEvent;
        public event TransmissionEventHandle<IWebRequestQy, IDeleteEventArgs> DeleteFailedEvent;

        private HttpWebRequest m_httpRequest;
        public void Create(string url)
        {
            m_httpRequest = WebRequest.Create(url) as HttpWebRequest;
        }
        public void PostRequest(byte[] postBuffer,int timeout)
        {
            m_Post(postBuffer,timeout);
        }


        private void m_Post(byte[] postBuffer,int timeout)
        {
            HttpWebPostArgs _postEventArgs = new HttpWebPostArgs();
            string errorMessage = string.Empty;
            FIRING_D_C_F _firEventType = FIRING_D_C_F.DEFAULT;
            DebugTool.Log("m_Post(): buffer message in.");
            m_httpRequest.KeepAlive = false;
            m_httpRequest.Method = "POST";
            m_httpRequest.Timeout = timeout;
            //m_httpRequest.ContentType = "application/x-www-form-urlencoded";
            m_httpRequest.ContentType = "application/json";
            //TODO  超时处理
            Stream _writer;
            try
            {
                _writer = m_httpRequest.GetRequestStream(); //任何尝试访问该方法时，不在子代类中重写该方法时：会引发NotImplementedException
                _writer.Write(postBuffer, 0, postBuffer.Length);
                _writer.Close();
                DebugTool.Log("m_Post(): buffer has posted.");
            }
            catch (WebException ee)
            {
                errorMessage = "---  WebException  --- :"
                            + "	之前已调用 Abort。-或 -请求的超时期限到期。-或 -处理请求时出错。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentNullException ee)
            {
                errorMessage = "---  ArgumentNullException  --- :"
                            + "buffer is null"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentOutOfRangeException ee)
            {
                errorMessage = "---  ArgumentOutOfRangeException  --- :"
                            + "offset 或 count 为负。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentException ee)
            {
                errorMessage = "---  ArgumentException  --- :"
                            + "总和 offset 和 count 大于缓冲区长度。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (IOException ee)
            {
                errorMessage = "---  IOException  --- :"
                            + "将出现 I / O 错误，如找不到指定的文件。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (NotSupportedException ee)
            {
                errorMessage = "---  NotSupportedException  --- :"
                            + "流不支持写入。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (Exception ee)
            {
                errorMessage = "---  Unknown exception  --- :"
                            + "Firing other unknown exception !!!!!!"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            if (_firEventType == FIRING_D_C_F.FIR_FAILED)
            {
                _postEventArgs.SetErrMessage(errorMessage);
                m_PostEventFiring(_firEventType, _postEventArgs, "m_Post");
                return;
            }

            try
            {
            HttpWebResponse _httpResponse = m_httpRequest.GetResponse() as HttpWebResponse;
                Stream _responseStream = _httpResponse.GetResponseStream();
                ///TODO：2016/12/19这句话报错，考虑后期流媒体怎么处理！！！！！
                ///------------解决方案：转码一次，后期考虑字节的直接操作，性能 性能 性能！！！；
                /// int length = (int)_responseStream.Length;
                StreamReader _reader = new StreamReader(_responseStream);
                string aa = _reader.ReadToEnd();
                byte[] buffer = Encoding.UTF8.GetBytes(aa);
                DebugTool.Log("RquestMessage buffer length ：" + buffer.Length);
                _responseStream.Close();
                _postEventArgs.SetBuffer(buffer);
                _firEventType = FIRING_D_C_F.FIR_COMPLETE;
                errorMessage = "---  no mistake  ---";
            }
            catch (WebException ee)
            {
                errorMessage = "---  WebException  --- :"
                            + "	之前已调用 Abort。-或 -请求的超时期限到期。-或 -处理请求时出错。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentNullException ee)
            {
                errorMessage = "---  ArgumentNullException  --- :"
                            + "buffer is null"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentOutOfRangeException ee)
            {
                errorMessage = "---  ArgumentOutOfRangeException  --- :"
                            + "offset 或 count 为负。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (ArgumentException ee)
            {
                errorMessage = "---  ArgumentException  --- :"
                            + "总和 offset 和 count 大于缓冲区长度。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (IOException ee)
            {
                errorMessage = "---  IOException  --- :"
                            + "将出现 I / O 错误，如找不到指定的文件。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (NotSupportedException ee)
            {
                errorMessage = "---  NotSupportedException  --- :"
                            + "流不支持读取。"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            catch (Exception ee)
            {
                errorMessage = "---  Unknown exception  --- :"
                            + "Firing other unknown exception !!!!!!"
                            + ee.Message;
                _firEventType = FIRING_D_C_F.FIR_FAILED;
            }
            _postEventArgs.SetErrMessage(errorMessage);
            m_PostEventFiring(_firEventType, _postEventArgs, "m_ReceiveMsgCallBack");
        }
        public void GetRequest()
        {
            m_GetEventFiring(FIRING_D_C_F.DEFAULT, null, "GetRequest");
        }
        public void PushRequest()
        {
            m_GetEventFiring(FIRING_D_C_F.DEFAULT, null, "PushRequest");

        }
        public void DeleteRequest()
        {
            m_GetEventFiring(FIRING_D_C_F.DEFAULT, null, "DeleteRequest");

        }
        private void m_GetEventFiring(FIRING_D_C_F firType, IGetEventArgs stateArgs, string funcionTag)
        {
            switch (firType)
            {
                case FIRING_D_C_F.DEFAULT:

                    break;
                case FIRING_D_C_F.FIR_COMPLETE:
                    DebugTool.LogTag(funcionTag, firType.ToString(), "#59A7CEFF");
                    if (GetCompleteEvent != null)
                    {
                        IGetEventArgs _args = stateArgs as IGetEventArgs;
                        GetCompleteEvent(this, _args);
                    }
                    break;
                case FIRING_D_C_F.FIR_FAILED:
                    DebugTool.LogTag(funcionTag, firType.ToString() + stateArgs.ErrMessage, "#BB7979FF");
                    if (GetFailedEvent != null)
                    {
                        IGetEventArgs _args = stateArgs as IGetEventArgs;
                        GetFailedEvent(this, _args);
                    }
                    break;
                default:

                    break;
            }
        }
        private void m_PostEventFiring(FIRING_D_C_F firType, IWebTransEventArgs stateArgs, string funcionTag)
        {
            switch (firType)
            {
                case FIRING_D_C_F.DEFAULT:

                    break;
                case FIRING_D_C_F.FIR_COMPLETE:
                    DebugTool.LogTag(funcionTag, firType.ToString(), "#59A7CEFF");
                    if (PostCompleteEvent != null)
                    {
                        IPostEventArgs _args = stateArgs as IPostEventArgs;
                        PostCompleteEvent(this, _args);
                    }
                    break;
                case FIRING_D_C_F.FIR_FAILED:
                    DebugTool.LogTag(funcionTag, firType.ToString() + stateArgs.ErrMessage, "#BB7979FF");
                    if (PostFailedEvent != null)
                    {
                        IPostEventArgs _args = stateArgs as IPostEventArgs;
                        PostFailedEvent(this, _args);
                    }
                    break;
                default:

                    break;
            }
        }
        private void m_PushEventFiring(FIRING_D_C_F firType, IPushEventArgs stateArgs, string funcionTag)
        {
            switch (firType)
            {
                case FIRING_D_C_F.DEFAULT:

                    break;
                case FIRING_D_C_F.FIR_COMPLETE:
                    DebugTool.LogTag(funcionTag, firType.ToString(), "#59A7CEFF");
                    if (PushCompleteEvent != null)
                    {
                        IPushEventArgs _args = stateArgs as IPushEventArgs;
                        PushCompleteEvent(this, _args);
                    }
                    break;
                case FIRING_D_C_F.FIR_FAILED:
                    DebugTool.LogTag(funcionTag, firType.ToString() + stateArgs.ErrMessage, "#BB7979FF");
                    if (PushFailedEvent != null)
                    {
                        IPushEventArgs _args = stateArgs as IPushEventArgs;
                        PushFailedEvent(this, _args);
                    }
                    break;
                default:

                    break;
            }
        }
        private void m_DeleteEventFiring(FIRING_D_C_F firType, IDeleteEventArgs stateArgs, string funcionTag)
        {
            switch (firType)
            {
                case FIRING_D_C_F.DEFAULT:

                    break;
                case FIRING_D_C_F.FIR_COMPLETE:
                    DebugTool.LogTag(funcionTag, firType.ToString(), "#59A7CEFF");
                    if (DeleteCompleteEvent != null)
                    {
                        IDeleteEventArgs _args = stateArgs as IDeleteEventArgs;
                        DeleteCompleteEvent(this, _args);
                    }
                    break;
                case FIRING_D_C_F.FIR_FAILED:
                    DebugTool.LogTag(funcionTag, firType.ToString() + stateArgs.ErrMessage, "#BB7979FF");
                    if (DeleteFailedEvent != null)
                    {
                        IDeleteEventArgs _args = stateArgs as IDeleteEventArgs;
                        DeleteFailedEvent(this, _args);
                    }
                    break;
                default:

                    break;
            }

        }

    }

}
