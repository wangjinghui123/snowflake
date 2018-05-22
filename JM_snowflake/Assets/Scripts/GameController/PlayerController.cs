//#define ISCAR

//人物和汽车替换需要 切换ISCAR宏定义(PlayerBody和PlayerController两个脚本)
//第二个场景的对应PlayersCar/Players激活
//替换PlayerController 上的进用
//替换AutoCamera 上的target


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using System;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

public delegate void SndRankToUserDelegate(object sender, PlayerBody body);
public class PlayerController : MonoBehaviour
{
    public event SndRankToUserDelegate SndRankToUserEvent;

    public UIAnimation UI_Animation;
    public TrackerMove playerTrackerMove;
    public TrackerMove endPointTrackerMove;



    [SerializeField]
    public GameObject playerParent;
    [SerializeField]
    private GameObject _Boy_Orange;
    [SerializeField]
    private GameObject _Boy_Green;
    [SerializeField]
    private GameObject _Nijia_Red;
    [SerializeField]
    private GameObject _Ninja_White;
    [SerializeField]
    private GameObject _Laohei_Green;
    [SerializeField]
    private GameObject _Laohei_Black;

    /// <summary>
    /// 用户管理List
    /// </summary>
    private List<PlayerBody> _userList
    {
        get
        {
            if (infoList == null)
            {
                Log.LogColor("infolist init");
                infoList = new List<PlayerBody>();
            }
            return infoList;
        }
        set
        {
            infoList = value;
        }
    }
    private List<PlayerBody> infoList;

    private const int MAX_COUNT_FRAME = 15;
    private int currentFrame = 0;
    void Update()
    {
        if (gameIsPlaying)
        {
            //游戏阶段的逻辑
            if (isEndRank || _userList == null || _userList.Count == 0)
                return;
            if (currentFrame < MAX_COUNT_FRAME)
            {
                ++currentFrame;
                return;
            }
            else
            {
                currentFrame = 0;
                _RankThePlayers();
            }
        }
        else if (!isEndRank)
        {
            //准备阶段的UI排布
            UI_Animation.ShowUserInfoOnRunk(_userList);
        }
    }

    bool gameIsPlaying = false;
    public void PlayersStartRunning(object sender, EventArgs e)
    {
        for (int i = 0; i < _userList.Count; i++)
        {
            _userList[i].num_Animator = 1;
            _userList[i].itCanMove = true;

        }
        gameIsPlaying = true;
        playerTrackerMove.isStart = true;
        endPointTrackerMove.isStart = true;
    }
    public void PlayerStopRunning(object sender, EventArgs e)
    {
        playerTrackerMove.isStart = false;
        endPointTrackerMove.isStart = false;
        gameIsPlaying = false;

#if ISCAR
        for (int i = 0; i < _userList.Count; i++)
        {
            _userList[i].itCanMove = false;

        }
#endif
    }

    public void OnSomeOneTryLogin(string userId, ref MsgTryLoginRes msg)
    {
        PlayerBody tempPlayer = _FindThePlayer(userId);
        if (tempPlayer == null)
        {
            return;
        }
        else
        {
            msg.SetMessage((int)tempPlayer.modelType, tempPlayer.score, tempPlayer.currentRankTag);
        }
    }


    public void SomeOneLogin(object sender, LoginInfo initiaInfo)
    {
        _OnNewUserLogin(initiaInfo);
    }
    private void _OnNewUserLogin(LoginInfo initiaUerInformation)
    {
        PlayerBody templayer = _FindThePlayer(initiaUerInformation.userId);
        if (templayer == null)
        {
            GameObject tempPraf;
            modleType m_Type = (modleType)int.Parse(initiaUerInformation.modelType);
            switch (m_Type)
            {
                #region 选择模型
                case modleType.Boy_Orange:
                    tempPraf = _Boy_Orange;
                    break;
                case modleType.Boy_Green:
                    tempPraf = _Boy_Green;
                    break;
                case modleType.Nijia_Red:
                    tempPraf = _Nijia_Red;
                    break;
                case modleType.Ninja_White:
                    tempPraf = _Ninja_White;
                    break;
                case modleType.Laohei_Green:
                    tempPraf = _Laohei_Green;
                    break;
                case modleType.Laohei_Black:
                    tempPraf = _Laohei_Black;
                    break;
                default:
                    Debug.LogWarning("Functiong _OnNewUserLogin(): Type of modle can not found.");
                    return;
                    #endregion
            }
            GameObject Player = Instantiate(tempPraf);
            PlayerBody newPlayer = Player.GetComponent<PlayerBody>();
            newPlayer.modelType = m_Type;
            newPlayer.isNpc = initiaUerInformation.isNpc;
            newPlayer.nickName = initiaUerInformation.nickName;
            newPlayer.userId = initiaUerInformation.userId;
            newPlayer.status = userStatus.isGame;
            newPlayer.headPortriat.headPortraitUrl = initiaUerInformation.headimgurl;
            newPlayer.startTime = (int)(Time.realtimeSinceStartup * 1000);

            _userList.Add(newPlayer);
            newPlayer.lastRankTag = _userList.Count - 1;
            newPlayer.currentRankTag = _userList.Count - 1;//玩家名次设置


            if (newPlayer._isNpc == InitiaItIsNpc.no)
            {
                if (SndRankToUserEvent != null)
                {
                    SndRankToUserEvent(this, newPlayer);
                }
            }
            _userList[_userList.Count - 1].name = initiaUerInformation.userId;
            _userList[_userList.Count - 1].transform.SetParent(playerParent.transform);
#if ISCAR
            if (_userList.Count > 0 && _userList.Count <= 4)//4个赛道
#else
            if (_userList.Count > 0 && _userList.Count <= 8)//4个赛道
#endif
            {
                _userList[_userList.Count - 1].trackeTag = _userList.Count;

                _userList[_userList.Count - 1].transform.localRotation = Quaternion.identity;


            }
            else
            {
                _userList[_userList.Count - 1].trackeTag = 0;
                _userList[_userList.Count - 1].transform.localPosition = Vector3.zero;
                _userList[_userList.Count - 1].transform.localRotation = Quaternion.identity;
            }
            _userList[_userList.Count - 1].gameObject.SetActive(true);
            if (!gameIsPlaying)
            {
                _userList[_userList.Count - 1].num_Animator = 0;
#if ISCAR
                _userList[_userList.Count - 1].itCanMove = false;
#endif

            }

            else
            {
                _userList[_userList.Count - 1].num_Animator = 1;
#if ISCAR
                _userList[_userList.Count - 1].itCanMove = true;
#endif

            }



        }
        else
        {
            templayer.status = userStatus.isGame;
            templayer.gameObject.SetActive(true);
            //if (templayer._isNpc == InitiaItIsNpc.no)
            //    if (SndRankToUserEvent != null)
            //    {
            //        SndRankToUserEvent(this, templayer);
            //    }
            Debug.Log("Function _OnNewUserLogin(): " + initiaUerInformation.userId + " come back,");
        }
    }
    public void SomeOneLogOut(object sender, LogoutInfo tempSomeOne)
    {
        _OnUserLogout(this.gameObject, tempSomeOne);
    }
    private void _OnUserLogout(object sender, LogoutInfo tempSomeOne)
    {
        PlayerBody logoutPlayer = _FindThePlayer(tempSomeOne.userId);
        if (logoutPlayer == null)
        {
            Debug.LogWarning("Function  _OnUserLogout(): Can not Find The player userId " + tempSomeOne.userId);
        }
        else
        {
            logoutPlayer.status = userStatus.logout;
            // logoutPlayer.gameObject.SetActive(false);
        }
    }
    public void SomeOneScoreChange(object sender, ScoreInfo socreInfo)
    {
        _OnUserMessageUpdate(this.gameObject, socreInfo);
    }
    private void _OnUserMessageUpdate(object sender, ScoreInfo scoreInfo)
    {
        if (gameIsPlaying)
        {
            PlayerBody tempPlayer = _FindThePlayer(scoreInfo.userId);
            if (tempPlayer == null)
            {
                //  Debug.LogWarning("Fuction _OnUserMessageUpdate() : Can't find the player " + scoreInfo.userId);
                return;
            }
            if (!tempPlayer.userId.Contains("NPC"))
                Debug.Log("^^^^^^^^^^^^^^^^^^^^^^^^^^^^update message ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            if (tempPlayer.status == userStatus.isGame)
            {
                tempPlayer.score += int.Parse(scoreInfo.score);
                if (!tempPlayer.userId.Contains("NPC"))
                {
                    Debug.Log("Fuction _OnUserMessageUpdate() : Come in " + scoreInfo.score);
                }
            }
        }

    }


    List<int> lastRank = new List<int>();
    List<int> currRank = new List<int>();
    List<PlayerBody> runOutUser = new List<PlayerBody>();
    List<PlayerBody> runInUser = new List<PlayerBody>();
    List<PlayerBody> runStayUser = new List<PlayerBody>();
    public List<PlayerBody> RankList = new List<PlayerBody>();

    private bool isEndRank = false;

    /// <summary>
    /// 可排名的情况下（游戏过程中）屏幕中显示的人数
    /// </summary>
    /// 
#if ISCAR
    int inscreenUserNum = 4 - 1;
#else
    int inscreenUserNum = 8 - 1;
#endif


    private void _RankThePlayers()
    {
        RankList = _CompareTheUserScore(_userList);
        for (int i = 0; i < _userList.Count; i++)
        {
            int last = _userList[i].lastRankTag;
            lastRank.Add(last);
        }
        UI_Animation.ShowUserInfoOnRunk(RankList);

        for (int i = 0; i < _userList.Count; i++)
        {
            int cur = _userList[i].currentRankTag;
            currRank.Add(cur);
        }
        //重新分配位置a
        for (int i = 0; i < _userList.Count; i++)
        {
            if (lastRank[i] == currRank[i])   //如果本次排名与上次排名相同，该人不动
                continue;
            else
            {
                if (lastRank[i] <= inscreenUserNum && currRank[i] <= inscreenUserNum)
                {
                    runStayUser.Add(_userList[i]);
                }
                else if (lastRank[i] <= inscreenUserNum && currRank[i] > inscreenUserNum)
                {
                    runOutUser.Add(_userList[i]);
                }
                else if (lastRank[i] > inscreenUserNum && currRank[i] <= inscreenUserNum)
                {
                    runInUser.Add(_userList[i]);
                }
            }

            if (!_userList[i].userId.Contains("NPC"))
            {
                if (SndRankToUserEvent != null)
                {
                    SndRankToUserEvent(this, _userList[i]);
                }
            }

        }
        for (int i = 0; i < runStayUser.Count; i++)
        {
            runStayUser[i].RunToTargetPosition();
        }
        //Debug.LogWarning("——Cut!!!————————"+runOutUser.Count+","+runInUser.Count+","+runStayUser.Count+"————————");

        if (runOutUser.Count == runInUser.Count && runOutUser.Count != 0)
        {
            int count = runOutUser.Count;
            for (int i = 0; i < count; i++)
            {
                runInUser[i].trackeTag = runOutUser[i].trackeTag;
#if ISCAR
#else
                runOutUser[i].trackeTag = 0;
#endif

                runInUser[i].RunInScreen();
                runOutUser[i].RunOutScreen();
            }
        }
        lastRank.Clear();
        currRank.Clear();
        runOutUser.Clear();
        runStayUser.Clear();
        runInUser.Clear();
        // Debug.Log("_RankThePlayers Done **********************************************");
    }
    private int _CheckInScreenTrackTag(List<PlayerBody> sortList)
    {
        int InTag = 0;
        if (sortList.Count < 4)
        {
            Debug.LogWarning("wraing");
            return 0;
        }
        for (int i = 1; i < 5; i++)
        {
            bool exsit = false;
            for (int j = 0; j < 4; j++)
            {
                if (sortList[j].trackeTag == i)
                {
                    exsit = true;
                    break;
                }
            }
            if (!exsit)
            {
                InTag = i;
                break;
            }
        }
        return InTag;
    }


    #region 终点动态更新算法

    /// <summary>
    /// 终点预设
    /// </summary>
    public GameObject endPointPrefab;
    public GameObject endPointParent;
    GameObject endPoint;
    /// <summary>
    /// 终点位置自适应算法
    /// </summary>
    public void SprintToEndPoint(object sender, EventArgs e)
    {
        _SprintToEndPoint();
    }
    private void _SprintToEndPoint()
    {
        //显示终点
        // endPointParent.transform.GetComponent<TrackerMove>().enabled = false;
        endPoint = Instantiate(endPointPrefab);
        endPoint.transform.SetParent(endPointParent.transform);
        endPoint.transform.localPosition = Vector3.zero;
        endPoint.transform.localRotation = Quaternion.identity;
        endPoint.transform.localScale = new Vector3(1, 1, 1);
        endPoint.transform.SetParent(endPointParent.transform.parent);
        endPoint.SetActive(true);
    }
    #endregion
    public EventHandler WinnerEvent;
    public EventHandler LatestEvent;
    public Transform PlayEndAnimation(GameObject theLastCamera)
    {
        if (WinnerEvent != null)
        {
            WinnerEvent(this, new EventArgs());
        }
        else
        {
            Debug.Log("Some One has complete the game, but 'WinnerEvent' is not trigger. ");
        }

        //停止Update自动刷新排名
        isEndRank = true;
        //玩家动画停止
        PlayerStopRunning(this, null);

        if (RankList == null && RankList.Count != 0)
        {
            Debug.LogWarning("Function  PlayEndAnimation():_userList  is  null");
            return null;
        }
        //最后一次排序
        _RankThePlayers();
        int iMax = RankList.Count;
        Cloth cloth = endPoint.transform.Find("ClothParent/cloth").GetComponent<Cloth>();


        cloth.capsuleColliders = new CapsuleCollider[inscreenUserNum + 1];
        CapsuleCollider[] capsuleColliderList = new CapsuleCollider[inscreenUserNum + 1];
        if (RankList.Count > inscreenUserNum + 1)
            iMax = inscreenUserNum + 1;


        for (int i = 0; i < iMax; i++)
        {
            capsuleColliderList[i] = RankList[i].transform.GetComponent<CapsuleCollider>();
        }
        cloth.capsuleColliders = capsuleColliderList;

#if ISCAR

#else
        playerTrackerMove.isStart = false;
        endPointTrackerMove.isStart = false;
#endif




        int count = RankList.Count;
        for (int i = 0; i < count; i++)
        {
            if (RankList[i].currentRankTag < inscreenUserNum + 1)
            {
                RankList[i].itCanMove = false;
#if ISCAR
                endPointTrackerMove.isStart = false;
                playerTrackerMove.isStart = false;
                StartCoroutine(waitAnimation(RankList[i]));
#else
                RankList[i].animator.applyRootMotion = true;
                StartCoroutine(waitAnimation(RankList[i].currentRankTag));
#endif



#if ISCAR
#else
                RankList[i].animator.speed *= 0.5f;
#endif
                RankList[i].TargetCamera = theLastCamera;
                Vector3 rotation = RankList[i].transform.rotation.eulerAngles;
                rotation.x = 0;
                rotation.z = 0;
                Quaternion quaternion = new Quaternion();
                quaternion.eulerAngles = rotation;
                RankList[i].transform.rotation = quaternion;
            }
            else
            {
                if (RankList[i].boxFbx.activeSelf)
                {
                    RankList[i].boxFbx.SetActive(false);
                }
            }
        }

        if (LatestEvent != null)
        {
            LatestEvent(this, new EventArgs());
        }
        else
        {
            Debug.Log("'LatestEvent' is null. ");
        }

       // UI_Animation.runTime.StopTheRunTime();//UI停止计时
        //UI_Animation.PlayeTheShoeAnima();//播放鞋子帧序列

        return RankList[0].transform;
    }
    IEnumerator waitAnimation(PlayerBody i)
    {
        if (i.currentRankTag == 0)
        {
            i.itCanMove = true;
            yield return new WaitForSeconds(0.2f);
            i.itCanMove = false;
        }
    }
    IEnumerator waitAnimation(int i)
    {
        if (i == 0)
        {
            yield return new WaitForSeconds(0.6f);
            RankList[i].num_Animator = 2;
        }
        else
        {
            yield return new WaitForSeconds(1.2f);
            RankList[i].num_Animator = 0;
        }
    }

    #region 查询与排序
    private List<PlayerBody> _CompareTheUserScore(List<PlayerBody> userList)
    {
        if (userList == null)
        {
            Debug.LogWarning("PlayerController._CompareTheUserScore userList is null");
            return null;
        }
        List<PlayerBody> playerList = new List<PlayerBody>();
        for (int i = 0; i < userList.Count; i++)
        {
            playerList.Add(userList[i]);
        }
        playerList.Sort(new PlayerBodyCompare());
        for (int i = 0; i < playerList.Count; i++)
        {
            int lastRank = playerList[i].currentRankTag;
            playerList[i].lastRankTag = lastRank;
            playerList[i].currentRankTag = i;
        }
        return playerList;
    }
    public PlayerBody GetThePlayer(string userId)
    {
        return _FindThePlayer(userId);
    }
    private PlayerBody _FindThePlayer(string userId)
    {
        PlayerBody tempPlayer = _userList.Find(delegate (PlayerBody t)
        {
            return t.userId == userId;
        });
        return tempPlayer;
    }
    #endregion


    #region 下载头像
    private static bool m_loadComplete = true;
    private static List<PlayerBody> m_loadTaskList = new List<PlayerBody>();
    public static void GetHeadPortriatFromHttp(PlayerBody pBody)
    {
        if (pBody != null)
            m_loadTaskList.Add(pBody);
        if (m_loadComplete == true && m_loadTaskList.Count > 0)
        {
            m_loadComplete = false;
            System.Threading.ThreadPool.QueueUserWorkItem((object state) =>
                {
                    var param = state as List<PlayerBody>;
                    param[0].isLoadHeadTexture = m_DownloadPicture(param[0], 10 * 1000);
                    param.RemoveAt(0);
                    m_loadComplete = true;
                    if (param.Count > 0)
                        GetHeadPortriatFromHttp(null);
                }, m_loadTaskList);
        }
    }
    private static bool m_DownloadPicture(PlayerBody pBody, int timeOut)
    {
        bool isSuccess = false;
        System.Net.WebResponse response = null;
        System.IO.Stream stream = null;
        try
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(pBody.headPortriat.headPortraitUrl);
            if (timeOut != -1) request.Timeout = timeOut;
            response = request.GetResponse();
            stream = response.GetResponseStream();
            //  Debug.LogWarning(pBody.userId + "  ----  " + response.ContentType.ToLower());
            if (response.ContentType.ToLower().StartsWith("image/"))
            { 
                var size = response.ContentLength;
                var buffer = new byte[size];
                int currentReadOff = 0;   
                do
                {
                    var read = stream.Read(buffer, currentReadOff, (int)size - currentReadOff);
                    currentReadOff += read;
                } while (currentReadOff < size);

                if (currentReadOff != size)
                {
                    Debug.LogWarning(pBody.userId + "  ××××× " + currentReadOff + " -- " + size);
                }
                else
                {
                    // Debug.LogWarning(pBody.userId + "  √√√ " + currentReadOff + " -- " + size);
                }
                MemoryStream imageStream = new MemoryStream(buffer);
                Bitmap origenBit = new Bitmap(imageStream);
                Bitmap targeBit = new Bitmap(128, 128);
                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(targeBit);
                gr.DrawImage(origenBit, new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, origenBit.Width, origenBit.Height), GraphicsUnit.Pixel);
                MemoryStream targetStream = new MemoryStream();
                targeBit.Save(targetStream, ImageFormat.Png);
                var targetBuffer = targetStream.GetBuffer();
                buffer = null;
                imageStream.Dispose();
                origenBit.Dispose();
                targeBit.Dispose();
                gr.Dispose();
                targetStream.Dispose();
                pBody.headBuffer = targetBuffer;
                isSuccess = true;
            }
        }
        catch (System.Exception ee)
        {
            Debug.LogWarning(ee.GetType());
            Debug.LogWarning(ee.Message);
        }
        finally
        {
            if (stream != null) stream.Close();
            if (response != null) response.Close();
        }

        return isSuccess;
    }
    private static void SaveStandardImage(string url)
    {
        try
        {

            var newW = 256;
            var newH = 256;
            Bitmap bitMap = new Bitmap(@"E:\example\WebSite48\无标题.JPG");

            Bitmap targeBitMap = new Bitmap(newW, newH);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(targeBitMap);
            // 插值算法的质量 
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(bitMap, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bitMap.Width, bitMap.Height), GraphicsUnit.Pixel);

            targeBitMap.Save(@"E:\example\WebSite48\无标题1.JPG");

            bitMap.Dispose();
            targeBitMap.Dispose();
            g.Dispose();
        }
        catch (Exception ee)
        {
            Debug.LogWarning("图片处理异常：" + ee.Message);
        }
    }
    #endregion
}



public enum userStatus
{
    defult = 0,
    isGame = 1,
    logout = 2
}

/// <summary>
/// 玩家头像类
/// </summary>
public class HeadPortriat
{
    /// <summary>
    ///共有属性：玩家头像-------------添加获取头像方法属性设置
    /// </summary>
    public Texture2D headPortraitTexture
    {
        get
        {
            return _headPortraitTexture;
        }
        set
        {
            _headPortraitTexture = value;
        }
    }
    private Texture2D _headPortraitTexture;

    /// <summary>
    /// 共有属性，Sprite格式图片
    /// </summary>
    public Sprite headSprite
    {
        get
        {
            return _headSprite;
        }
        set
        {
            _headSprite = value;
        }
    }
    private Sprite _headSprite;
    /// <summary>
    /// 玩家头像Url
    /// </summary>
    public string headPortraitUrl
    {
        get
        {
            return _headPortraitUrl;
        }
        set
        {
            _headPortraitUrl = value;
        }
    }

    private string _headPortraitUrl = string.Empty;

    public void Dispose()
    {
        UnityEngine.MonoBehaviour.Destroy(_headPortraitTexture);
        _headPortraitTexture = null;
        _headPortraitUrl = string.Empty;
    }
}
/// <summary>
/// 心跳包
/// </summary>
public class HeartBeatInfo
{
    public heartType hb
    {
        get { return _answer; }
        set { _answer = value; }
    }
    private heartType _answer = heartType.send_to_other;
    public enum heartType
    {
        send_to_other = 0,//发送0
        receive_from_other = 1,//接收到0，应该发送1
    }
}

public enum InitiaItIsNpc
{
    no = 0,
    yes = 1,
}