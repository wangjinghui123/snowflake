using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qy_CSharp_NetWork.DataFormat.JisightLAM
{
    enum SND_MSG_TYPE
    {
        //心跳信息
        HEART_BEAT = 20000,
        //Post：房间验证
        ROOM_VERIFICATION = 21100,
        //Push：房间状态
        ROOM_STATUS = 22000,
        //Post：奖励申请
        AWARDE_REQUEST = 21200,
        //Push：汇总数据推送
        SUMMARIZE = 22100,
        //Push：交互信息
        PC_H5 = 40000,
    }
    enum REV_MSG_TYPE
    {
        DEFAULT = 0,
        //心跳信息
        HEART_BEAT = 20000,
        //房间验证返回
        ROOM_VERIFICATION_T = 21100,
        ROOM_VERIFICATION_F = 21101,
        //房间状态
        ROOM_STATUS_T = 22000,
        ROOM_STATUS_F = 22001,
        //奖励申请返回
        AWARD_LIST_T = 21200,
        AWARD_LIST_F = 21201,
        //汇总数据推送
        SUMMARIZE_T = 22100,
        SUMMARIZE_F = 22101,
        //H5断开链接
        H5_LINK_BROKEN = 23000,
        //PC_H5交互信息
        PC_H5_T = 40000,
        PC_H5_F = 40001,
    }

    enum NEED_VERIFY
    {
        DONT_REPLY = 0,
        NEED_REPLY = 1,
    }

}
