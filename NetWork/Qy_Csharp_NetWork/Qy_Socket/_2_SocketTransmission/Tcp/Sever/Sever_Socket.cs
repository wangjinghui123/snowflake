/***************************************************************************
*                                 * 服务端链接类 *                         *
*                                                                          *
*   1.实例化该类，对该类进行操作，之所以不选择静态，源于其服务端可能       *
*     持有N多ClientSocket实例（考虑将来添加多个Socket的管理功能，或许      *
*     内部自我持有N多Socket。）                                            *
*   2.操作实例会触发 ‘成功’、‘失败’的，注册相关事件回调使用即可。      *
*   3.抛出异常仅限于Debug时使用，用户需保证交互时收到正确数据。            *
*                                                                          *
****************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Qy_CSharp_NetWork.Qy_Socket.Transmission
{
    //class SeverSocket : ISeverSocketQy
    //{
    //    public event InnerOperationDelagate<ISocketQy, IInnerEventArgs> AnsyncAccept_Complete_Event;
    //    public event InnerOperationDelagate<ISocketQy, IInnerEventArgs> AnsyncAccept_Failed_Event;
    //    public event InnerOperationDelagate<ISocketQy, IInnerEventArgs> AnsyncRecMsg_Complete_Event;
    //    public event InnerOperationDelagate<ISocketQy, IInnerEventArgs> AnsyncRecMsg_Failed_Event;
    //    public event InnerOperationDelagate<ISocketQy, IInnerEventArgs> AnsyncSendMsg_Complete_Event;
    //    public event InnerOperationDelagate<ISocketQy, IInnerEventArgs> AnsyncSendMsg_Failed_Event;

    //    public void Dispose()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void TryAnsyncConnectTo()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void TryAnsyncReceiveMessage(int bytes)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void TryAnsyncSendMessage(byte[] bytes)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
