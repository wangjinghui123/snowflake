using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;

public class UIAnimation : MonoBehaviour
{
    public List<RunkingUser> userRankList;
    public RunTimeCount runTime;
    public ShortTimeAnimation readyTime;
    public List<Image> onlyHeadPortriat;

    public Image headImage;
    public GameObject headImageParent;
    public GameObject shoe;



    #region  UI层游戏时间逻辑处理
    /// <summary>
    /// 初始化游戏时间
    /// </summary>
    /// <param name="runTimefloat">游戏时间</param>
    /// <param name="readyTimeInt">准备时间</param>
    public void SetReadyAndRunTime(float runTimefloat, int readyTimeInt)
    {
        runTime.gameObject.SetActive(false);
        readyTime.gameObject.SetActive(false);
        runTime.allRunTime = runTimefloat;
        readyTime.readyTime = readyTimeInt;

    }
    public void StartTimeCount()
    {
        //runTime.gameObject.SetActive(true);
        readyTime.gameObject.SetActive(true);
        readyTime.readyTimeDoneEvent += ReadyToRun;
    }
    private void ReadyToRun(object sender, System.EventArgs args)
    {
        readyTime.readyTimeDoneEvent -= ReadyToRun;
        runTime.gameObject.SetActive(true);
    }
    #endregion



    /// <summary>
    /// 初始化UI图片、名称等
    /// </summary>

    public void InitiaUI()
    {
        _InitiaUI();
    }
    private void _InitiaUI()
    {
        for (int i = 0; i < userRankList.Count; i++)
        {
            userRankList[i].headPortriat.color = new Color(0, 0, 0, 0);
            userRankList[i].rankName.text = string.Empty;
            userRankList[i].step.text = string.Empty;
        }
        //shoe.SetActive(false);
        //awadeImage.gameObject.SetActive(false);
        //awadeText.gameObject.SetActive(false);

        standardSize = headImage.rectTransform.sizeDelta;
        maxPosVlue = headImageParent.GetComponent<RectTransform>().rect.size;// * 0.98f;
        delta_x = maxPosVlue.x / 11;
        delta_y = standardSize.y * 1.05f;
    }




    #region  UI层排行榜逻辑处理
    float delta_x;
    float delta_y;
    int m;
    int j;
    Vector2 standardSize;//10名后的头像大小
    Vector2 maxPosVlue;

    /// <summary>
    /// UI排名显示
    /// </summary>
    /// <param name="playerList">用户排序列表</param>
    public void ShowUserInfoOnRunk(List<PlayerBody> playerList)
    {
        _GetUserInfo(playerList);

    }
    private void _GetUserInfo(List<PlayerBody> playerList)
    {
        if (userRankList == null)
            userRankList = new List<RunkingUser>();

        if (playerList == null || playerList.Count == 0)
        {
            Debug.LogWarning("Function _GetUserInfo:PlayerList is null ");
            return;
        }
        int count = playerList.Count;
        for (int i = 0; i < count; i++)
        {
            if (i < 10)
            {

                userRankList[i].headPortriat.sprite = playerList[i].headPortriat.headSprite;
                userRankList[i].rankName.text = playerList[i].nickName;
                userRankList[i].step.text = playerList[i].score.ToString();
                if (playerList[i].status == userStatus.defult)
                {
                    Debug.LogWarning("Funtion ShowUserInfoOnRunk():in UI_Rank Player's status is defult check it userId is: " + playerList[i].userId);
                }
                else if (playerList[i].status == userStatus.isGame)
                {
                    userRankList[i].headPortriat.color = new Color(1, 1, 1, 1);
                }
                else if (playerList[i].status == userStatus.logout)
                {
                    userRankList[i].headPortriat.color = new Color(0.29f, 0.29f, 0.29f, 1);
                }
            }
            else
            {
                if (onlyHeadPortriat == null)
                    onlyHeadPortriat = new List<Image>();
                int idex = i - userRankList.Count;
                if (idex + 1 > onlyHeadPortriat.Count)
                {
                    Image tempImge = Instantiate(headImage);
                    tempImge.gameObject.SetActive(true);
                    tempImge.transform.SetParent(headImageParent.transform);
                    tempImge.transform.localScale = Vector3.one;
                    tempImge.rectTransform.sizeDelta = standardSize;
                    m = idex % 10;
                    j = (idex - (idex % 10)) / 10;
                    tempImge.rectTransform.anchoredPosition3D = new Vector3(-maxPosVlue.x / 2 + (m + 1) * delta_x, maxPosVlue.y / 2 - (j + 1) * delta_y, 0);
                    onlyHeadPortriat.Add(tempImge);
                }
                onlyHeadPortriat[idex].sprite = playerList[i].headPortriat.headSprite;

                if (playerList[i].status == userStatus.defult)
                {
                    Debug.LogWarning("Funtion ShowUserInfoOnRunk():in UI_Rank Player's status is defult check it userId is: " + playerList[i].userId);
                }
                else if (playerList[i].status == userStatus.isGame)
                {
                    onlyHeadPortriat[idex].color = new Color(1, 1, 1, 1);
                }
                else if (playerList[i].status == userStatus.logout)
                {
                    onlyHeadPortriat[idex].color = new Color(0.29f, 0.29f, 0.29f, 1);
                }
                //userRankList[i].headPortriat.sprite = null;
                //userRankList[i].headPortriat.color = new Color(1, 1, 1, 0);
                //userRankList[i].rankName.text = string.Empty;
                //userRankList[i].step.text = string.Empty;
            }
        }


    }
    #endregion

    public Image awadeImage;
    public Text awadeText;
    private int resetTime = 0;
    private bool showed = false;
    public void AwadeAnimation(object sender, SingleAwarde awadeInfo)
    {
        if (!showed)
        {
            showed = true;
        }
        else
        {
            return;
        }
        Debug.Log("AwardAnimation ---- in ----");
        //if (awadeInfo != null && !string.IsNullOrEmpty(awadeInfo.awardName) && !string.IsNullOrEmpty(awadeInfo.awardImg))
        //{

        Debug.Log("最后的奖励图片 加载 ");
        awadeImage.rectTransform.sizeDelta = Vector2.zero;
        awadeImage.gameObject.SetActive(true);
        awadeImage.rectTransform.DOSizeDelta(new Vector2(750, 346), 1f);

        //PlayerBody tempPlayer = GameObject.Find("PlayerController").GetComponent<PlayerController>().GetThePlayer(awadeInfo.user_id);
        //awadeText.text = tempPlayer.nickName + " 获得奖励:" + awadeInfo.awardName;
        //Debug.Log("弹出一等奖图片:" + awadeInfo.awardImg);




        //StartCoroutine(_GetTheTexture(awadeInfo.awardImg));
        //awadeText.gameObject.SetActive(true);
        //}
        //else
        //{
        //    Debug.LogWarning("奖励为null 或者 奖励图片url为 空、或者 奖励名称是空。");
        //}
        Debug.Log("AwardAnimation ---- out ----");
    }
    IEnumerator _GetTheTexture(string url)
    {
        //WWW ww = new WWW("Http:" + url);
        WWW ww = new WWW(url);

        yield return ww;
        if (string.IsNullOrEmpty(ww.error))
        {
            resetTime = 0;
            Texture2D texure = ww.texture;
            Sprite spriteTemp = Sprite.Create(texure, new Rect(0, 0, texure.width, texure.height), new Vector2(0.5f, 0.5f));
            awadeImage.sprite = spriteTemp;
            awadeImage.rectTransform.sizeDelta = Vector2.zero;
            awadeImage.gameObject.SetActive(true);
            awadeImage.rectTransform.DOSizeDelta(new Vector2(750, 346), 0.5f);
        }
        else
        {
            resetTime++;
            if (resetTime <= 3)
                StartCoroutine("_GetTheTexture", url);
            Debug.LogWarning("www.error =" + ww.error + "\n"
                            + "url:" + url);
        }
        ww.Dispose();
    }


    public void PlayeTheShoeAnima()
    {
        shoe.SetActive(true);
    }
}