using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.DataFormat.JisightLAM
{
    /// <summary>
    /// 消息类型
    /// </summary>
    interface IType
    {
        string Tp { get; }
    }
    /// <summary>
    /// 消息确认回复：用于Debug
    /// </summary>
    interface IVerify
    {
        string Ve { get; }
        void SetVerify(NEED_VERIFY needVerify);
    }
    /// <summary>
    /// 时间戳
    /// </summary>
    interface ITime
    {
        string Tm { get; }
        void SetTime(string targetTimeTag = "");
    }
    /// <summary>
    /// 发送的消息是给谁？
    ///     "HostSever"     ---     主控服务器
    ///     "GameSever"     ---     游戏服务器
    ///     "Calculator"    ---     计算端
    ///     "userId"        ---     手机端玩家UserId
    /// </summary>
    interface IToWhere
    {
        string[] To { get; }
        void SetReceiver(string[] tagArray);
    }
    /// <summary>
    /// 消息
    /// </summary>
    interface IMessage
    {
        object Mg { get; }
    }

    interface IToJson
    {
        string ToJson();
    }

}
