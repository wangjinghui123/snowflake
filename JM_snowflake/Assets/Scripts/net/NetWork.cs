using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qy_CSharp_NetWork.WebNet;
using Qy_CSharp_NetWork.Component;
using System;
using Qy_CSharp_NetWork.Tools.Json;
using Newtonsoft.Json.Linq;

namespace WJH
{



    public class NetWork : MonoBehaviour
    {

        const string GameID = "8";

        //启动服务
        private JisightUnityComponent_LAM _lookAtMe = null;
        private GameMessageDataHandle _gameDataHandle = null;
        private NET_OPTION m_netOption = NET_OPTION.WAN;
        [SerializeField]
        private GameControllerAsTime _gameControllerAsTime;
        private CreateNode creatCode;
        public JisightUnityComponent_LAM LookAtMe
        {
            get
            {
                if (_lookAtMe == null)
                    _lookAtMe = this.gameObject.AddComponent<JisightUnityComponent_LAM>();
                return _lookAtMe;
            }
        }

        public GameMessageDataHandle GameDataHandle
        {
            get
            {
                if (_gameDataHandle == null)
                    _gameDataHandle = this.gameObject.AddComponent<GameMessageDataHandle>();
                return _gameDataHandle;
            }
        }

        public CreateNode CreatCode
        {
            get
            {
                if (creatCode == null)
                {
                    creatCode = GetComponent<CreateNode>();
                }
                return creatCode;
            }
        }
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Application.targetFrameRate = 60;

            Application.runInBackground = true;
            Cursor.visible = false;
        }
        // Use this for initialization
        void Start()
        {
            _gameControllerAsTime.StartGameEvent += StartGame;
            _gameControllerAsTime.Init(true, 15);
        }
        public void StartGame(object sender, EventArgs msg)
        {
            Debug.LogError(msg);
            StartWork();
        }
        public void StartWork(bool needBug = false, bool istest = true)
        {
            List<string> deviceIds = new List<string>();
            string token = "";
            string gamgId = "";

            gamgId = GameID;
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
                token = "WkE8fSqkq3Zm1Mfruj61uK0zj0OaScOF0znl";//
                deviceIds.Clear();
                deviceIds.Add("4000");
                deviceIds.Add("6005");
                //token = "91yrf3qkqstxlix3rlvouwvjaxrk28fflgwh";//自己
                //deviceIds.Clear();
                //deviceIds.Add("10000");
            }
            Debug.Log("token:   " + token);
            string ret = "";
            for (int i = 0; i < deviceIds.Count; i++)
            {
                ret += deviceIds[i] + "\n";
            }
            Debug.Log(ret);

            LookAtMe.needDebug = needBug;
            LookAtMe.token = token;
            LookAtMe.gameId = gamgId;
            print("初始化成功");
            LookAtMe.JLAM_Event_Fir += GameDataHandle.NetEventFir;
            //二维码生成
            _gameDataHandle.TryLoginIn += TryLoginInPlayer;
            _gameDataHandle.RoomVerifyCompleteEvent += OnRoomVerify;
            GameDataHandle.GameReady += GameDataHandle_GameReady;



            _gameDataHandle.LoginOut += LogginOutPlayer;
            GameDataHandle.LoginIn += GameDataHandle_LoginIn;
            //Debug.Log(m_netOption + "," + deviceIds[0] + "," + deviceIds[1]);
            LookAtMe.StartLAM(m_netOption, 12 * 1000, deviceIds.ToArray());

        }
        /// <summary>
        /// 玩家选择角色登录事件注册
        /// </summary>
        /// <param name="obj"></param>
        private void GameDataHandle_LoginIn(JsonData obj)
        {
            JToken data = obj["data"];
            string head = data.Value<string>("avatar");
            string nickname = data.Value<string>("nickname");
            string model = data.Value<string>("model");
            string userId = obj["userId"].ToString();
            //Debug.LogError(head+ nickname+model+ userId);
            PlayerModule.Instance.Add(userId, new PlayerData(nickname, userId, head, (modleType)(int.Parse(model))));

        }

        private void GameDataHandle_GameReady()
        {
            Debug.LogError("游戏准备好了");
            creatCode.GameQrCodePosOnGame();//二维码游戏中位置
            LookAtMe.PushGameStatus("2", true, true);
        }

        /// <summary>
        /// 登录回复
        /// </summary>
        /// <param name="jsonData"></param>
        private void TryLoginInPlayer(JsonData jsonData)
        {

            LoginInfo tryInfo = new LoginInfo();
            tryInfo.UserId = jsonData["userId"].ToString();
            PlayerData tempLoginInfo = PlayerModule.Instance.GetPlayerDataByID(tryInfo.UserId);
            MsgTryLoginRes resMsg = new MsgTryLoginRes();
            if (tempLoginInfo==null)
            {
                resMsg.SetMessage(-1, 0, 0);
            }
            else
            {
                resMsg.SetMessage(tempLoginInfo.modelType, 0, 0);
            }
            
            Debug.Log("用户尝试登陆信息回复：--- in --- ");
            List<string> receivers = new List<string>();

            receivers.Add(tryInfo.UserId);
            if (_gameDataHandle.TryLoginEvent != null)
            {
                _gameDataHandle.TryLoginEvent(tryInfo.UserId, resMsg);
            }

            //Debug.Log(Qy_CSharp_NetWork.Tools.Json.JsonTools.ToJson(msg));
            _lookAtMe.PushMsgToOther(resMsg, receivers.ToArray(), false, true);
            Debug.Log("用户尝试登陆信息回复：--- out --- ");

        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="data"></param>
        public void LogginOutPlayer(JsonData data)
        {
            //获取到断开连接的用户
            string userId = (string)data["userId"];
            Debug.LogError("玩家" + userId + "退出了游戏");
            PlayerModule.Instance.RemovePlayer(userId);

        }
        /// <summary>
        /// 尝试登录回复
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="msg"></param>
        public void ResponseTryLoginMsg(string receiver, MsgTryLoginRes msg)
        {
            Debug.Log("用户尝试登陆信息回复：--- in --- ");
            List<string> receivers = new List<string>();
            receivers.Add(receiver);

            Debug.Log(Qy_CSharp_NetWork.Tools.Json.JsonTools.ToJson(msg));
            LookAtMe.PushMsgToOther(msg, receivers.ToArray(), false, true);
            Debug.Log("用户尝试登陆信息回复：--- out --- ");

        }

        private void OnRoomVerify(object sender, string url)
        {
            Debug.Log("收到验证成功");
            CreatCode.CreatCode(url);     //返回生成二维码
            StartCoroutine(Wait(5));
            GameStarusIsReady();
        }
        /// <summary>
        /// 向服务器发送准备的信息,不发会项目一直在准备中
        /// </summary>
        public void GameStarusIsReady()
        {
            Debug.LogError(" --------------_GameStarusIsReady -------------- ");
            LookAtMe.PushGameStatus("1", true, true);
            SceneController.Instance.ChangeScene("Scene02", ChangeSceneHandler);
        }
        /// <summary>
        /// 切换到场景回调
        /// </summary>
        public void ChangeSceneHandler()
        {
            ///游戏准备好了要做的事情
            GameDataHandle.GameReadyImpl();

            ///倒计时结束
            GameController.Instance.OnReadyEndTime += (() =>
            {
                GameStarusIsGame();
            });

            GameController.Instance.InitTime();
            //_uiAnimation.readyTime.readyTimeDoneEvent -= _GameStarusIsSundGlass;
            Debug.LogError("-------------- _GameStarusIsSundGlass -------------- ");

        }
        private void OnDestroy()
        {
            LookAtMe.Dispose();
            Debug.Log("游戏退出");
        }

        public IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
        }
        public void GameStarusIsGame()
        {
            Debug.LogWarning(" -------------- _GameStarusIsGame -------------- ");//倒计时结束
            LookAtMe.PushGameStatus("3", true, true);
        }
        public void GameStatusIsWm()
        {
            Debug.LogWarning(" -------------- _GameStarusIsWm -------------- ");//游戏结束
            LookAtMe.PushGameStatus("5", true, true);
        }
        public void GameStarusIsWmII()
        {
            Debug.LogWarning(" -------------- _GameStarusIsWmII --------------");//断开长连接
            LookAtMe.PushGameStatus("6", true, true);
        }
    }

}