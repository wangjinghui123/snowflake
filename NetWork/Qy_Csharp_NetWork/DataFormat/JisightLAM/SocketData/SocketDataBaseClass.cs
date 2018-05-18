

namespace Qy_CSharp_NetWork.DataFormat.JisightLAM
{
    using System;
    using System.Collections.Generic;
    using Tools.Json;
    abstract class PushMsg : IType, IVerify, ITime, IToWhere, IMessage, IToJson
    {
        public abstract string Tp { get; }
        public string Ve { get { return ((int)m_verify).ToString(); } }
        public string Tm { get { return m_now.ToString(); } }
        public string[] To { get { return m_toUsers.ToArray(); } }
        public abstract object Mg { get; }

        private NEED_VERIFY m_verify = NEED_VERIFY.DONT_REPLY;
        private string m_now = DateTime.MinValue.ToString("yyyy-MM-dd_HH:mm:ss.fff");
        private List<string> m_toUsers = new List<string>();
        public void SetTime(string targetTimeTag = "")
        {
            if (targetTimeTag == "")
                m_now = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.fff");
            else
                m_now = targetTimeTag;

        }

        public void SetReceiver(string[] tagArray)
        {
            for (int _idex = 0; _idex < tagArray.Length; ++_idex)
            {
                m_toUsers.Add(tagArray[_idex]);
            }
        }
        public void SetVerify(NEED_VERIFY needVerify)
        {
            m_verify = needVerify;
        }
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
    abstract class PostGetMsg : IType, ITime, IToWhere, IMessage, IToJson
    {
        public abstract string Tp { get; }
        public string Tm { get { return m_now.ToString(); } }
        public string[] To { get { return m_toUsers.ToArray(); } }
        public abstract object Mg { get; }

        private string m_now = DateTime.MinValue.ToString("yyyy-MM-dd_HH:mm:ss.fff");
        private List<string> m_toUsers = new List<string>();
        public void SetTime(string targetTimeTag = "")
        {
            if (targetTimeTag == "")
                m_now = DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss.fff");
            else
                m_now = targetTimeTag;
        }
        public void SetReceiver(string[] tagArray)
        {
            for (int _idex = 0; _idex < tagArray.Length; ++_idex)
            {
                m_toUsers.Add(tagArray[_idex]);
            }
        }
        public string ToJson()
        {
            return JsonTools.ToJson(this);
        }

    }

}
