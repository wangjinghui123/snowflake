using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.DataFormat.JisightLAM
{
    delegate void SocketClearDataCallBack();

    class ClearDataFormat
    {
        public REV_MSG_TYPE Type { get { return m_type; } }
        private REV_MSG_TYPE m_type = REV_MSG_TYPE.DEFAULT;

        public string Time { get { return m_time; } }
        private string m_time = string.Empty;

        public string Msg { get { return m_msg; } }
        private string m_msg = string.Empty;

        public void SetType(REV_MSG_TYPE revType)
        {
            m_type = revType;
        }
        public void SetTime(string timeTag)
        {
            m_time = timeTag;
        }
        public void SetMsg(string message)
        {
            m_msg = message;
        }
    }




}
