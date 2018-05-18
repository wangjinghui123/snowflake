
/**********************************
*     协 议 层 数 据 包 定 义     *
***********************************/

namespace Qy_CSharp_NetWork.WebNet.Base
{
    interface ITransProtocol
    {
        event TransmissionEventHandle<IWebRequestQy, IPostEventArgs> PostMsgComeEvent;
        event TransmissionEventHandle<IWebRequestQy, IPostEventArgs> PostMsgFailedEvent;
        void PostMsg(IWebRequestQy sender, byte[] buffer,int timeout);
    }

}
