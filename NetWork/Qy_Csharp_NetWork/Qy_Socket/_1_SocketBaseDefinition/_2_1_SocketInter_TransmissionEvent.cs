/************************************
*     传 输 层 事 件 关 系 定 义    *
*************************************/

namespace Qy_CSharp_NetWork.Qy_Socket.Base
{
    delegate void TransmissionEventHandle<T, U>(T sender, U eventArgs) //传输层事件委托
                                   where T : ISocketQy
                                   where U : ISocketTransEventArgs;
    interface ISocketTransEventArgs//传输层委托 参数
    {
        string ErrMessage { get; }
        void SetErrMessage(string paramStr);
    }
    interface ISocketConEventArgs : ISocketTransEventArgs // 链接回调 参数
    {

    }

    interface ISocketSndEventArgs : ISocketTransEventArgs// 发送回调 参数
    {
        string Tag { get; }
        int BufferSize { get; }
        byte[] SendBuffer { get; }
        void SetBuffer(byte[] buffer);
        void SetTag(string tag);

    }
    interface ISocketRecEventArgs : ISocketTransEventArgs// 接收回调 参数
    {
        string Tag { get; set; }
        int NeedRecvBufferSize { get; set; }
        byte[] ReceiveBuffer { get; }
        void SetBuffer(byte[] buffer);
        void AddBuffer(byte[] buffer);
    }
}
