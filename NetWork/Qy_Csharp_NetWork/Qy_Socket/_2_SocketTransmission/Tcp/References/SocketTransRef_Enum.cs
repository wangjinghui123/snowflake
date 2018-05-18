using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Qy_CSharp_NetWork.Qy_Socket.Transmission
{

    /// <summary>
    /// 事件触发类型：0-不触发，1-出发完成，2-触发失败
    /// </summary>
    /// 
    enum FIRING_D_C_F
    {
        DEFAULT = 0,
        FIR_COMPLETE = 1,
        FIR_FAILED = 2
    }


}
