/******************************
*     传 输 层 关 系 定 义    *
*******************************/

using System;
namespace Qy_CSharp_NetWork.Qy_Socket.Base
{

    interface ISocketQy : ITryAnsyncSndMsgQy, ITryAnsyncRecMsgQy, IDisposable
    {

    }
    interface IClientSocketQy : ISocketQy, ITryAnsyncConnectQy
    {

    }
    interface ISeverSocketQy : ISocketQy, ITryAnsyncAcceptQy
    {

    }

    interface ITryAnsyncConnectQy//异步链接发起
    {
        event TransmissionEventHandle<ISocketQy, ISocketConEventArgs> AnsyncConnectTo_Complete_Event;
        event TransmissionEventHandle<ISocketQy, ISocketConEventArgs> AnsyncConnectTo_Failed_Event;
        bool SetEndPoint(string ip, int port);
        void TryAnsyncConnectTo();
    }
    interface ITryAnsyncAcceptQy//异步监听消息
    {
        event TransmissionEventHandle<ISocketQy, ISocketTransEventArgs> AnsyncAccept_Complete_Event;
        event TransmissionEventHandle<ISocketQy, ISocketTransEventArgs> AnsyncAccept_Failed_Event;
        void TryAnsyncConnectTo();
    }
    interface ITryAnsyncSndMsgQy//异步发送消息
    {
        event TransmissionEventHandle<ISocketQy, ISocketSndEventArgs> AnsyncSendMsg_Complete_Event;
        event TransmissionEventHandle<ISocketQy, ISocketSndEventArgs> AnsyncSendMsg_Failed_Event;
        void TryAnsyncSendMessage(byte[] bytes, string tag = "None");
    }
    interface ITryAnsyncRecMsgQy//异步接收消息
    {
        event TransmissionEventHandle<ISocketQy, ISocketRecEventArgs> AnsyncRecMsg_Complete_Event;
        event TransmissionEventHandle<ISocketQy, ISocketRecEventArgs> AnsyncRecMsg_Failed_Event;
        void TryAnsyncReceiveMessage(int bytes, string tag = "None");
    }

}
