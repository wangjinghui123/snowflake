using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.WebNet
{
    using Tools.Debug;
    using Base;
    using Transmission;
    using OriginalDataUserTransProtocol;
    class WebEasyHttpQy
    {
        public event WebRequestDelegate RequestCompleteEvent;
        public event WebRequestDelegate RequestFailedEvent;
        public WebEasyHttpQy(WEB_REQUEST_TYPE webNetType = WEB_REQUEST_TYPE.HTTP,
                            WEB_DATA_PROTOCOL webDataProtocol = WEB_DATA_PROTOCOL.ORIGINAL)
        {
            switch (webNetType)
            {
                case WEB_REQUEST_TYPE.HTTP:
                    m_WebRequest = new HttpWebRequestQy();
                    break;
                default:
                    DebugTool.LogError("'WEB_REQUEST_TYPE' can not clear. ");
                    break;
            }
            switch (webDataProtocol)
            {
                case WEB_DATA_PROTOCOL.ORIGINAL:
                    m_protocol = new WebKeepOrigDataProtocol();
                    break;
                default:
                    DebugTool.LogError("'WEB_DATA_PROTOCOL' can not clear. ");
                    break;
            }
        }
        //请求：可重载其他类型
        private IWebRequestQy m_WebRequest;
        //协议：可重载其他类型
        private ITransProtocol m_protocol;

        public void PostMessage(string url, string message,int timeout)
        {
            m_WebRequest.Create(url);
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            m_protocol.PostMsgComeEvent += PostMessageCompleteCallBack;
            m_protocol.PostMsgFailedEvent += PostMessageFailedCallBack;
            m_protocol.PostMsg(m_WebRequest, buffer, timeout);//协议应用
        }

        private void PostMessageFailedCallBack(IWebRequestQy sender, IPostEventArgs eventArgs)
        {
            if (RequestFailedEvent != null)
            {
                IPostEventArgs _args = eventArgs as IPostEventArgs;
                WebRequestArgsQy pushArg = new WebRequestArgsQy();
                pushArg.SetMessage(_args.ErrMessage);
            }
        }

        private void PostMessageCompleteCallBack(IWebRequestQy sender, IPostEventArgs eventArgs)
        {
            if (RequestCompleteEvent != null)
            {
                string str = Encoding.UTF8.GetString(eventArgs.ReceiveBuffer);
                WebRequestArgsQy pushArg = new WebRequestArgsQy();
                pushArg.SetMessage(str);
                RequestCompleteEvent(this, pushArg);
            }
        }

    }
}
