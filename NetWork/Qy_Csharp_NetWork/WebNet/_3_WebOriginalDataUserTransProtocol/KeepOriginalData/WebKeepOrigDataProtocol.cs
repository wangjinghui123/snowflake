using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.WebNet.OriginalDataUserTransProtocol
{
    using Base;
    using System.Threading;
    using Tools.Debug;
    /// <summary>
    /// 本类为默认处理协议：数据不经过加工，直接抛出
    /// </summary>
    class WebKeepOrigDataProtocol : ITransProtocol
    {
        public event TransmissionEventHandle<IWebRequestQy, IPostEventArgs> PostMsgComeEvent;
        public event TransmissionEventHandle<IWebRequestQy, IPostEventArgs> PostMsgFailedEvent;
        public void PostMsg(IWebRequestQy sender, byte[] buffer,int timeoutMillseconds)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(
                (object state) =>
                {
                    sender.PostCompleteEvent += PostMsgComeEvent;
                    sender.PostFailedEvent += PostMsgFailedEvent;
                    sender.PostRequest(buffer,timeoutMillseconds);
                }), sender);
        }


    }
}
