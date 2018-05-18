using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.Component
{
    public enum NET_OPTION
    {
        LAN = 0,
        WAN = 1
    }
    /// <summary>
    /// 数据状态码
    /// </summary>
    public enum DATA_STATUS_CODE
    {
        /*** 连接层 状态码 ***/
        /// <summary>
        /// 连接层出现问题
        /// </summary>
        ERROR_TRIGGER_CODE = 0,

        /*** 数据层 状态码 ***/
        /// <summary>
        /// 建房成功
        /// </summary>
        DATA_BUILD_ROOM_T = 200,
        /// <summary>
        /// 建房失败
        /// </summary>
        DATA_BUILD_ROOM_F = 201,

        /// <summary>
        /// 房间验证成功
        /// </summary>
        DATA_ROOM_VERIFY_T = 400,
        /// <summary>
        /// 房间验证失败
        /// </summary>
        DATA_ROOM_VERIFY_F = 401,

        /// <summary>
        /// 推送房间状态成功
        /// </summary>
        DATA_ROOM_STATUS_T = 410,
        /// <summary>
        /// 推送房间状态失败
        /// </summary>
        DATA_ROOM_STATUS_F = 411,

        /// <summary>
        /// 发送汇总成功
        /// </summary>
        DATA_SUMMARIZE_T = 420,
        /// <summary>
        /// 发送汇总失败
        /// </summary>
        DATA_SUMMARIZE_F = 421,

        /// <summary>
        /// 请求奖励成功
        /// </summary>
        DATA_AWARD_LIST_T = 430,
        /// <summary>
        /// 请求奖励失败
        /// </summary>
        DATA_AWARD_LIST_F = 431,

        /// <summary>
        /// 自定义信息接收成功
        /// </summary>
        DATA_CUSTOM_MSG_T = 440,
        /// <summary>
        /// 自定义信息接收失败
        /// </summary>
        DATA_CUSTOM_MSG_F = 441,

        /// <summary>
        /// 有链接断开
        /// </summary>
        LINK_USER_BROKEN = 500,

    }

    enum LINK_STATUS_CODE
    {
        REQUEST_T = 100,
        REQUEST_F = 101,

        SOCKET_CON_T = 300,
        SOCKET_CON_F = 301,
        SOCKET_SND_T = 310,
        SOCKET_SND_F = 311,
        SOCKET_REV_T = 320,
        SOCKET_REV_F = 321,
        SOCKET_CLS_T = 330,
        SOCKET_CLS_F = 331,
    }

}


