/******************************
*     传 输 层 方 法 定 义    *
*******************************/

namespace Qy_CSharp_NetWork.WebNet.Base
{
    interface IWebPost
    {
        event TransmissionEventHandle<IWebRequestQy, IPostEventArgs> PostCompleteEvent;
        event TransmissionEventHandle<IWebRequestQy, IPostEventArgs> PostFailedEvent;
        void PostRequest(byte[] buffer,int timeout);
    }
    interface IWebGet
    {
        event TransmissionEventHandle<IWebRequestQy, IGetEventArgs> GetCompleteEvent;
        event TransmissionEventHandle<IWebRequestQy, IGetEventArgs> GetFailedEvent;
        void GetRequest();
    }
    interface IWebPush
    {
        event TransmissionEventHandle<IWebRequestQy, IPushEventArgs> PushCompleteEvent;
        event TransmissionEventHandle<IWebRequestQy, IPushEventArgs> PushFailedEvent;
        void PushRequest();
    }
    interface IWebDelete
    {

        event TransmissionEventHandle<IWebRequestQy, IDeleteEventArgs> DeleteCompleteEvent;
        event TransmissionEventHandle<IWebRequestQy, IDeleteEventArgs> DeleteFailedEvent;
        void DeleteRequest();
    }
    interface ITimeOut
    {
        event TransmissionEventHandle<IWebRequestQy, ITimeOutEventArgs> TimeOutEvent;
    }
    interface ICreatRequest
    {
        void Create(string url);
    }
    interface IWebRequestQy : IWebGet, IWebPost, IWebPush, IWebDelete, ICreatRequest
    {


    }
}
