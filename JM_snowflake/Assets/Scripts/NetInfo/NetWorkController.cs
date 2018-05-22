
//#define INPUT_PARAM
using UnityEngine;
using System;
using Qy_CSharp_NetWork.Component;
using System.Collections.Generic;

public delegate void GameMessageComeEvent(object sender, string msg);
public delegate void TryLoginMsgDelegate(string userId, ref MsgTryLoginRes tryMsg);
public class NetWorkController : MonoBehaviour
{
    public const string GAME_ID = "8";
    private CreateCode creatCode;
    private JisightUnityComponent_LAM _lookAtMe = null;
    private GameMessageDataHandle _gameDataHandle = null;
    private GameControllerAsTime _gameControllerAsTime;

    //内网外网设置
    private NET_OPTION m_netOption = NET_OPTION.WAN;

    public TryLoginMsgDelegate TryLoginEvent;
    public void StartNetWork(bool needDebug = false, bool istest = false)
    {
        creatCode = this.GetComponent<CreateCode>();
        string gamgId = "";
        string token = "";
        List<string> deviceIds = new List<string>();

        gamgId = GAME_ID;
        for (int idex = 0; idex <= 2; ++idex)
        {
            string tempId = QyCsharpProgramTools.Tools.GetEnvironmentValue("id" + idex.ToString());
            Debug.Log("尝试取参" + "【id" + idex.ToString() + "】: " + tempId);
            if (tempId != "")
            {
                deviceIds.Add(tempId);
            }
        }
        token = QyCsharpProgramTools.Tools.GetEnvironmentValue("token");
        Debug.Log("尝试取参" + "【token】: " + token);

        if (istest)
        {
            token = "WkE8fSqkq3Zm1Mfruj61uK0zj0OaScOF0znl";//经贸
            deviceIds.Clear();
            deviceIds.Add("4000");
            deviceIds.Add("6003");
            //token = "91yrf3QKQStxlIX3RLVOUwVJAXRk28FFLGwh";//自己
            //deviceIds.Clear();
            //deviceIds.Add("10000");
        }


        if (_lookAtMe == null)
            _lookAtMe = this.gameObject.AddComponent<JisightUnityComponent_LAM>();
        if (_gameDataHandle == null)
        {
            _gameDataHandle = this.gameObject.AddComponent<GameMessageDataHandle>();
        }
        _lookAtMe.needDebug = needDebug;
        _lookAtMe.token = token;
        _lookAtMe.gameId = gamgId;
        _lookAtMe.JLAM_Event_Fir += _gameDataHandle.NetEventFir;
        _gameDataHandle.SomeOneTryLoginEvent += ResponseTryLoginMsg;
        _gameDataHandle.RoomVerifyCompleteEvent += OnRoomVerify;
        //Debug.Log(m_netOption + "," + deviceIds[0] + "," + deviceIds[1]);
        _lookAtMe.StartLAM(m_netOption,12*1000, deviceIds.ToArray());
        //Debug.Log("开启连接");
    }


    public void StopNetWork()
    {
        _gameDataHandle.SomeOneTryLoginEvent -= ResponseTryLoginMsg;
        _lookAtMe.JLAM_Event_Fir -= _gameDataHandle.NetEventFir;
        _lookAtMe.Dispose();
    }
    private void OnRoomVerify(object sender, string url)
    {
        Debug.Log("收到验证成功");
        Debug.Log(sender +"\n"+"url:"+url );
        creatCode.CreatCode(url);     //返回生成二维码
        GameStarusIsReady();
    }
    public void GameStarusIsReady()
    {
        Debug.LogWarning(" --------------_GameStarusIsReady -------------- ");
        _lookAtMe.PushGameStatus("1", true, true);
    }
    public void GameStarusIsSundGlass()
    {
        //_uiAnimation.readyTime.readyTimeDoneEvent -= _GameStarusIsSundGlass;
        Debug.LogWarning("-------------- _GameStarusIsSundGlass -------------- ");
       creatCode.GameQrCodePosOnGame();//二维码游戏中位置

        _lookAtMe.PushGameStatus("2", true, true);
    }
    public void GameStarusIsGame()
    {
        Debug.LogWarning(" -------------- _GameStarusIsGame -------------- ");
        _lookAtMe.PushGameStatus("3", true, true);
    }
    public void GameStatusIsWm()
    {
        Debug.LogWarning(" -------------- _GameStarusIsWm -------------- ");
        _lookAtMe.PushGameStatus("5", true, true);
    }
    public void GameStarusIsWmII()
    {
        Debug.LogWarning(" -------------- _GameStarusIsWmII --------------");
        _lookAtMe.PushGameStatus("6", true, true);
        StopNetWork();
    }
    //用户尝试登陆的回复
    public void ResponseTryLoginMsg(string receiver, MsgTryLoginRes msg)
    {
        Debug.Log("用户尝试登陆信息回复：--- in --- ");
        List<string> receivers = new List<string>();
        receivers.Add(receiver);
        if (TryLoginEvent != null)
        {
            TryLoginEvent(receiver, ref msg);
        }

        Debug.Log(Qy_CSharp_NetWork.Tools.Json.JsonTools.ToJson(msg));
        _lookAtMe.PushMsgToOther(msg, receivers.ToArray(), false, true);
        Debug.Log("用户尝试登陆信息回复：--- out --- ");

    }
    //时时数据发送：仅包含了一个 名次信息
    public void SendGameRankToSomeOne(object sender, PlayerBody player)
    {
        Debug.Log("发送时时排名：----- in -----");
        MsgUserRankNubResInfo rankMsg = new MsgUserRankNubResInfo();
        rankMsg.SetMessage(player.currentRankTag);
        List<string> receivers = new List<string>();
        receivers.Add(player.userId);
        Debug.Log(Qy_CSharp_NetWork.Tools.Json.JsonTools.ToJson(rankMsg));
        _lookAtMe.PushMsgToOther(rankMsg, receivers.ToArray(), false, true);
        Debug.Log("发送时时排名：----- out -----");

    }
    //最后的排行榜发送
    public void SendLastRankList(List<PlayerBody> playerList)
    {
        Debug.Log("最终排行榜：----- in -----");
        MsgLastRankList msg = new MsgLastRankList();
        msg.SetMessage(playerList);
        List<string> receivers = new List<string>();
        receivers.Add("All");
        _lookAtMe.PushMsgToOther(msg, receivers.ToArray(), false, true);
        Debug.Log("最终排行榜：----- out -----");
    }
    //给主控的信息发送
    public void SendSummarizedInformation(List<PlayerBody> rankList)
    {
        Debug.Log("汇总数据：----- in -----");
        List<string> userIdList = new List<string>();
        for (int idex = 0; idex < rankList.Count; ++idex)
        {
            if (rankList[idex]._isNpc == InitiaItIsNpc.no)
            {
                userIdList.Add(rankList[idex].userId);
            }
        }
        _lookAtMe.PushRankListToMaster(userIdList, true, true);
        Debug.Log("汇总数据：----- out -----");
    }
    //发送奖励数据
    public void SendAwardMessage_Single(List<string> userIdList)
    {
        Debug.Log("单奖申请：----- in -----");

        Dictionary<string, int> awardList = new Dictionary<string, int>();
        for (int idex = 0; idex < userIdList.Count; ++idex)
        {
            awardList.Add(userIdList[idex], 1);
        }
        _lookAtMe.PostAwardeList(awardList, true);
        Debug.Log("单奖申请：----- out -----");

    }
    public void SendAwardMessage_Single(List<PlayerBody> userIdList)
    {
        Debug.Log("单奖申请：----- in -----");

        Dictionary<string, int> awardList = new Dictionary<string, int>();
        for (int idex = 0; idex < userIdList.Count; ++idex)
        {
            awardList.Add(userIdList[idex].userId, int.Parse(userIdList[idex].isNpc));
        }
        _lookAtMe.PostAwardeList_leo(awardList, true);
        Debug.Log("单奖申请：----- out -----");

    }

    public void SendAwardMessage_Mysterious(Dictionary<string, int> userIdCountAwardList, string mysterious)
    {
        _lookAtMe.PushMysteriousAwardeInfo(userIdCountAwardList, mysterious, true);
    }
    public void SendAwardMessage_Multy(int count)
    {
        Debug.Log("多奖申请：----- in -----");
        List<int> countList = new List<int>();
        countList.Add(count);
        countList.Add(count);
        countList.Add(count);
        countList.Add(count);
        countList.Add(count);
        countList.Add(count);
        countList.Add(count);
        countList.Add(count);
        _lookAtMe.PostAwardeList(countList, true);
        Debug.Log("多奖申请：----- out -----");
    }

}

