using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.DataFormat.JisightLAM
{
    public class MsgFromMaster
    {
        public MsgFromMaster()
        {
            WMIp = string.Empty;
            WMPort = string.Empty;
            roomId = string.Empty;
            gameSign = string.Empty;
            deviceId = string.Empty;
        }
        public string WMIp { get; set; }
        public string WMPort { get; set; }
        public string roomId { get; set; }
        public string gameSign { get; set; }
        public string deviceId { get; set; }
    }

}
