using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.WebNet.Transmission
{
    using Base;

    class HttpWebPostArgs : IPostEventArgs
    {
        public string ErrMessage
        {
            get
            {
                return m_ErrStr;
            }
        }
        public void SetErrMessage(string errorMessage)
        {
            m_ErrStr = errorMessage;
        }
        public int BufferSize
        {
            get
            {
                return m_bytes.Length;
            }
        }
        public byte[] ReceiveBuffer
        {
            get
            {
                return m_bytes;
            }
        }
        public void SetBuffer(byte[] buffer)
        {
            m_bytes = buffer;
        }
        private byte[] m_bytes;
        private string m_ErrStr;
    }






}
