using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.Qy_Socket.Transmission
{
    using Base;
    class ConEventArgs : ISocketConEventArgs
    {
        public string ErrMessage
        {
            get { return m_ErrStr; }
        }
        private string m_ErrStr;
        public void SetErrMessage(string paramStr)
        {
            m_ErrStr = paramStr;
        }
    }
    class SndEventArgs : ISocketSndEventArgs
    {
        public string ErrMessage
        {
            get { return m_ErrStr; }
        }
        public byte[] SendBuffer
        {
            get
            {
                return m_buffer;
            }
        }
        public int BufferSize
        {
            get
            {
                return m_buffer.Length;
            }
        }

        public string Tag
        {
            get
            {
                return m_tag;
            }
        }

        private string m_ErrStr;
        private byte[] m_buffer;
        private string m_tag;
        public void SetErrMessage(string paramStr)
        {
            m_ErrStr = paramStr;
        }
        public void SetBuffer(byte[] buffer)
        {
            m_buffer = buffer;
        }

        public void SetTag(string tag)
        {
            m_tag = tag;
        }
    }
    class RecEventArgs : ISocketRecEventArgs
    {
        public int NeedRecvBufferSize
        {
            get { return m_needBufferSize; }
            set { m_needBufferSize = value; }
        }
        public string ErrMessage
        {
            get { return m_ErrStr; }
        }
        public byte[] ReceiveBuffer
        {
            get { return m_buffer.ToArray(); }
        }
        public string Tag
        {
            get { return m_tag; }
            set { m_tag = value; }
        }
        private string m_ErrStr = "";
        private int m_needBufferSize = 0;
        private List<byte> m_buffer = new List<byte>();
        private string m_tag = "";

        public void SetErrMessage(string paramStr)
        {
            m_ErrStr = paramStr;
        }

        public void SetBuffer(byte[] buffer)
        {
            m_buffer = new List<byte>(buffer);
        }

        public void AddBuffer(byte[] buffer)
        {
            m_buffer.AddRange(buffer);
        }
    }
}
