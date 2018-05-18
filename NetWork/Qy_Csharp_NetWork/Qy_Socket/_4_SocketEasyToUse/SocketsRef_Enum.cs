using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.Qy_Socket
{
    enum CLIENT_SOCKET_TYPE
    {
        TCP = 0,
        UDP = 1
    }
    enum SOCKET_DATA_PROTOCOL
    {
        HEAD_BODY = 0,
    }
    /// <summary>
    /// 事件类型
    /// </summary>
    enum EVENT_TYPE
    {
        DEFAULT = 0,
        CONNECT = 1,
        SEND = 2,
        RECEIVE = 3,
    }
    /// <summary>
    /// 成功和失败的标记
    /// </summary>
    enum COMPLETE_OR_FAILED
    {
        DEFAULT = 0,
        COMPLETE = 1,
        FAILED = 2,
    }





}
