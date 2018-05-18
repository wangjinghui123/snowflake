using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.WebNet
{
    delegate void WebRequestDelegate(object sender, WebRequestArgsQy eventArgs);
    /// <summary>
    /// 机制内传参信息，不可暴露到外部
    /// </summary>
    class WebRequestArgsQy 
    {
        public string Message { get { return m_message; } }
        private string m_message = string.Empty;
        public void SetMessage(string message)
        {
            m_message = message;
        }
    }






}
