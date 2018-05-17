#define Debug
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// 游戏物体控制类
    /// </summary>
    [SerializeField]
    private StepController _stepController;
    /// <summary>
    /// UI脚本
    /// </summary>
    [SerializeField]
    private UIAnimation _uiAnimation;
    [SerializeField]
    private MusicController _musicController;
    /// <summary>
    /// 用户控制脚本
    /// </summary>
    [SerializeField]
    private PlayerController _playerController;

    public float gameTime = 50;
    public int readyTime = 50;

    //在被调用启动是传过来的参数
    private GameControllerAsTime _gameControllerAsTime;

    public NetWorkController netWorkController { get { return _netWorkController; } }
    private NetWorkController _netWorkController;
    GameMessageDataHandle _messageDataHandle;
    public void InitiaTheGameAndStart(GameObject OBJ_Net, GameControllerAsTime timecontroller)
    {
        _netWorkController = OBJ_Net.GetComponent<NetWorkController>();
        _playerController.SndRankToUserEvent += _netWorkController.SendGameRankToSomeOne;
        _netWorkController.TryLoginEvent += _playerController.OnSomeOneTryLogin;

        _gameControllerAsTime = timecontroller;
        gameTime = _gameControllerAsTime.TimeOfGame;
        readyTime = _gameControllerAsTime.TimeOfReady;
        // OBJ_Net.GetComponent<CreateCode>().ShowQrCode();
        _stepController.InitializeTheGameObj();
        _uiAnimation.SetReadyAndRunTime(gameTime, readyTime);//游戏时间初始化
        _uiAnimation.InitiaUI();//排行榜UI初始化


        //初始化队列选手信息
        _messageDataHandle = OBJ_Net.GetComponent<GameMessageDataHandle>();
        m_InitLoginUser(_messageDataHandle.gameMessageList, readyTime);

        OnStartTheGame();
        NPC npc = _playerController.transform.GetComponent<NPC>();
        npc.InitiaNPC(10);
        Debug.Log("#################################### Game in ####################################");
    }
    private void m_InitLoginUser(List<EventArgs> argList, int pInitTime)
    {
        Debug.Log("场景跳转前加入人数："+argList.Count);
        for (int idex = 0; idex < argList.Count; ++idex)
        {
            if (argList[idex] is LoginInfo)
            {
                LoginInfo loginInfo = argList[idex] as LoginInfo;
                _playerController.SomeOneLogin(this, loginInfo);

            }
            else if (argList[idex] is LogoutInfo)
            {
                LogoutInfo logoutInfo = argList[idex] as LogoutInfo;
                _playerController.SomeOneLogOut(this, logoutInfo);
            }
        }
        argList.Clear();
    }

    /// <summary>
    /// 开始游戏的事件回调
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnStartTheGame()
    {
        //更改游戏状态事件
        _netWorkController.GameStarusIsSundGlass();

        //玩家管理信息事件回掉添加
        _messageDataHandle.SomeOneIsLoginEvent += _playerController.SomeOneLogin;
        _messageDataHandle.SomeOneIsLogoutEvent += _playerController.SomeOneLogOut;

        _stepController.StepTwoPlay();

        //动画事件添加
        _uiAnimation.StartTimeCount();
        _uiAnimation.readyTime.readyTimeDoneEvent += _OnReadyTimeDonePlayerCanDo;//时间监听以UI为准
        //准备阶段
        _gameControllerAsTime.GameTimeDoneEvent += m_OnGameTimeDone;
        //游戏物体动画添加
        _uiAnimation.readyTime.readyTimeDoneEvent += _playerController.PlayersStartRunning;
        ////游戏音乐添加
        _musicController.PlayeStartMusic();
    }

    //玩家分数传值生效
    private void _OnReadyTimeDonePlayerCanDo(object sender, EventArgs e)
    {
        _uiAnimation.readyTime.readyTimeDoneEvent -= _OnReadyTimeDonePlayerCanDo;
        _uiAnimation.readyTime.readyTimeDoneEvent -= _playerController.PlayersStartRunning;
        //_gameControllerAsTime.ReadyTimeDoneEvent -= _OnReadyTimeDonePlayerCanDo;
        //_gameControllerAsTime.ReadyTimeDoneEvent -= _playerController.PlayersStartRunning;
        _netWorkController.GameStarusIsGame();
        _messageDataHandle.SomeOneIsUpMessageEvent += _playerController.SomeOneScoreChange;

        //音乐更换
        _musicController.StopStartMusic();
        _musicController.PlayeBackGroundMusic();

        NPC npc = _playerController.transform.GetComponent<NPC>();
        npc.NPC_ChangeScore();
    }
    private void m_OnGameTimeDone(object sender, EventArgs e)
    {
        //取消gametime监听
        _gameControllerAsTime.GameTimeDoneEvent -= m_OnGameTimeDone;
        //生成终点
        _playerController.WinnerEvent += _OnWinnerEvent;
        _playerController.SprintToEndPoint(this, null);

    }

    private void _OnWinnerEvent(object sender, EventArgs e)
    {
        //取消玩家撞线事件
        _playerController.WinnerEvent -= _OnWinnerEvent;
        _messageDataHandle.SomeOneIsLogoutEvent -= _playerController.SomeOneLogOut;

        //停止NPC刷分
        NPC npc = _playerController.transform.GetComponent<NPC>();
        npc.NPC_StopScoreCount();

        //推送游戏状态5
        _netWorkController.GameStatusIsWm();

        //取消玩家游戏事件
        _messageDataHandle.SomeOneIsLoginEvent -= _playerController.SomeOneLogin;
        _messageDataHandle.SomeOneIsUpMessageEvent -= _playerController.SomeOneScoreChange;
        Debug.LogWarning("登陆、得分事件监听取消");

        //播放喝彩音乐
        _musicController.StopBackGroundMusic();
        _musicController.PlayeCheerMusicMusic();

        _playerController.LatestEvent += _SendLastMsgToSever;

    }

    private void _SendLastMsgToSever(object sender, EventArgs sss)
    {
        _messageDataHandle.RcvMsgOverEvent += _CloseLookAtMe;
        _messageDataHandle.Awarde_Single_Event += _uiAnimation.AwadeAnimation;

        List<PlayerBody> temPlayers = _playerController.RankList;
        //发送汇总信息
        List<PlayerBody> playersCutNpc = new List<PlayerBody>();
        for (int idex = 0; idex < temPlayers.Count; ++idex)
        {
            temPlayers[idex].endTime = (int)(Time.realtimeSinceStartup * 1000);
            if (temPlayers[idex]._isNpc == InitiaItIsNpc.no)
            {
                playersCutNpc.Add(temPlayers[idex]);
            }
        }

        //_netWorkController.SendSummarizedInformation(playersCutNpc);
        //发送排行榜给玩家
        _netWorkController.SendLastRankList(temPlayers);
        //神秘大奖
        //string mysterious = _GetMys();
        //if (mysterious != "")
        //{
        //    int mysteriousTime = 0;
        //    bool isSucc = int.TryParse(mysterious, out mysteriousTime);
        //    if (isSucc)
        //    {
        //        var targetTime = mysteriousTime - GameControllerAsTime.allOfTheGameTime;
        //        Dictionary<string, int> mysteriousDic = new Dictionary<string, int>();
        //        for (int idex = 0; idex < temPlayers.Count; ++idex)
        //        {
        //            if (temPlayers[idex]._isNpc == InitiaItIsNpc.no)
        //            {
        //                mysteriousDic.Add(temPlayers[idex].userId, 1);
        //            }
        //        }
        //        _netWorkController.SendAwardMessage_Mysterious(mysteriousDic, targetTime.ToString());
        //    }
        //}

        //请求单奖
        //List<string> awardeList = new List<string>();
        //for (int idex = 0; idex < temPlayers.Count; ++idex)
        //{
        //    if (temPlayers[idex]._isNpc == InitiaItIsNpc.no)
        //    {
        //        awardeList.Add(temPlayers[idex].userId);
        //    }
        //}
        //if (awardeList.Count > 0)
        //{
        //    _messageDataHandle.firstUserId = awardeList[0];
        //}
        //_netWorkController.SendAwardMessage_Single(awardeList);

        //
        if (playersCutNpc.Count > 0)
        {
            _messageDataHandle.firstUserId = playersCutNpc[0].userId;
        }
        _netWorkController.SendAwardMessage_Single(temPlayers);

    }

    //断开连接
    private void _CloseLookAtMe(object sender, EventArgs sss)
    {
        _netWorkController.GameStarusIsWmII();
    }
    private string _GetMys()
    {
        string[] arguments = Environment.GetCommandLineArgs();
        //string[] arguments = new string[] { "mysterious=1000" };
        foreach (string arg in arguments)
        {
            if (arg.Contains("="))
            {
                string[] str = arg.Split('=');
                if (str.Length == 2 && str[0] != null && str[1] != null)
                {
                    if (str[0].Equals("mysterious"))
                    {
                        return str[1];
                    }
                }
            }
        }
        return "";
    }

}




