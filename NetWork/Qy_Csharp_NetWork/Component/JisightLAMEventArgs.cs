using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.Component
{
    using DataFormat.JisightLAM;

    public delegate void JisightLAMEventHandler(object sender, LAMEventArgs evAgs);
    /// <summary>
    /// 可用于外部参考的 请求返回 状态信息类
    /// </summary>
    public class LAMEventArgs
    {
        /// <summary>
        ///  数据状态码
        /// </summary>
        public DATA_STATUS_CODE status { get; private set; }
        public string message { get; private set; }
        public void SetMessage(DATA_STATUS_CODE status, string msg)
        {
            this.status = status;
            message = msg;
        }
        public void SetMessage(DATA_STATUS_CODE status, int errcode)
        {
            this.status = status;
            message = "Error : " + errcode.ToString();
        }
    }



}
