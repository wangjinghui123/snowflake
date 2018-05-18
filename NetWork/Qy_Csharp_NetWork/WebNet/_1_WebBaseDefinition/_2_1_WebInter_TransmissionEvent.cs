/************************************
*     传 输 层 事 件 关 系 定 义    *
*************************************/

namespace Qy_CSharp_NetWork.WebNet.Base
{
    delegate void TransmissionEventHandle<T, U>(T sender, U eventArgs) //传输层事件委托
                                   where T : IWebRequestQy
                                   where U : IWebTransEventArgs;
    interface IWebTransEventArgs
    {
        string ErrMessage { get; }
        void SetErrMessage(string errMessage);
    }
    interface IPostEventArgs : IWebTransEventArgs
    {
        int BufferSize { get; }
        byte[] ReceiveBuffer { get; }
        void SetBuffer(byte[] buffer);
    }
    interface IGetEventArgs : IWebTransEventArgs
    {
        int BufferSize { get; }
        byte[] ReceiveBuffer { get; }
        void SetBuffer(byte[] buffer);
    }
    interface IPushEventArgs : IWebTransEventArgs
    {

    }
    interface IDeleteEventArgs : IWebTransEventArgs
    {

    }
    interface ITimeOutEventArgs : IWebTransEventArgs
    {

    }
}
