
/*********************************
*     存 取 数 据 关 系 定 义    *
*    ( 协 议 层 接 口 定 义 )    *
**********************************/

namespace Qy_CSharp_NetWork.Qy_Socket.Base
{
    interface ITransProtocol
    {
        event TransmissionEventHandle<ISocketQy, ISocketSndEventArgs> SendMsgCompleteEvent;
        event TransmissionEventHandle<ISocketQy, ISocketSndEventArgs> SendMsgFailedEvent;
        event TransmissionEventHandle<ISocketQy, ISocketRecEventArgs> RecMsgCompleteEvent;
        event TransmissionEventHandle<ISocketQy, ISocketRecEventArgs> RecMsgFailedEvent;
        ISocketQy TargetSocketArray { get; }
        void SetTargetSocket(ISocketQy workSocket);
        void SendMessage(byte[] buffer);
        void ContinueReceiveMsg();
        void StopReceiveMessage();
    }




}
