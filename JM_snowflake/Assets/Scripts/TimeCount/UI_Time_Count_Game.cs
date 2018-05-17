using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;


/// <summary> 
///这是UI层计时器脚本:
/// 操作说明：一张父物体的Image下拖入三张Image,
///           计时数字图片保存在streamingAssetsPath/Texture/TimeTextrue/*.png
///           注意：计时数字图片名称：0.png，1.png 。。。。。。9.png
/// </summary> 

public class UI_Time_Count_Game : MonoBehaviour
{
    //游戏准备计时设置
    #region
    public int AllTime = 10;
    public int currentGameTime;
    //百位
    public Image Hundreds;
    //十位
    public Image Decade;
    //个位
    public Image Unit;
    //初始化动画大小
    Vector2 standardScale;
    //数字保存路径
    private string pathOfNumber;
    //数字图片列表
    List<Sprite> spritelists;// = new List<Sprite>();


    void OnEnable()
    {
        currentGameTime = AllTime;
        pathOfNumber = "Texture/TimeTexture";
        TextureInitialize();
        ReadyTimecount(currentGameTime);
    }

    void OnDisable()
    {
        StopTimeCount();
        for (int i = 0; i < spritelists.Count; i++)
        {
            Destroy(spritelists[i]);
        }
        spritelists.Clear();
    }
    public void FalseTheActive()
    {
        this.gameObject.SetActive(false);
    }
    public void StopTimeCount()
    {
        StopCoroutine("ChangNumber");
    }

    private void ReadyTimecount(int allTimeOfTheCount)
    {
        StartCoroutine("ChangNumber", allTimeOfTheCount);
    }

    private void TextureInitialize()
    {
        spritelists = new List<Sprite>();

        //标准数字大小设置
        standardScale = new Vector2(
           Mathf.Abs((float)(Unit.rectTransform.transform.parent.GetComponent<RectTransform>().rect.x / 3)),
           Mathf.Abs((float)Unit.rectTransform.transform.parent.GetComponent<RectTransform>().rect.y)
            );
        //Debug.Log(standardScale);
        //数字获取图片
        int _index = 10;
        for (int i = 0; i < _index; i++)
        {
            string path = Application.streamingAssetsPath + @"/" + pathOfNumber + @"/" + i.ToString() + ".png";
            //创建文件读取流
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];
            //读取文件
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            //释放文件读取流
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;

            //创建Texture
            int width = (int)standardScale.x;
            int height = (int)standardScale.y;
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            //创建Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            spritelists.Add(sprite);
        }
        int x = 0,
            y = 0,
            z = 0;
        if (AllTime >= 0 && AllTime <= 10)
        {
            x = AllTime % 10;
            Unit.sprite = spritelists[x];
        }
        else if (AllTime >= 10 && AllTime < 100)
        {
            x = AllTime % 10;
            y = ((AllTime - x) / 10) % 10;
            Decade.sprite = spritelists[y];
            Unit.sprite = spritelists[x];
        }
        else if (AllTime >= 100 && AllTime < 1000)
        {
            x = AllTime % 10;
            y = ((AllTime - x) / 10) % 10;
            z = (AllTime - AllTime % 100) / 100;
            Hundreds.sprite = spritelists[z];
            Decade.sprite = spritelists[y];
            Unit.sprite = spritelists[x];
        }
    }

    IEnumerator ChangNumber(int allTimeOfTheCount)
    {
        //  Debug.Log(AllTime);
        int x = 0, y = 0, z = 0;
        Vector2 offsetMin = new Vector2(0, 0);
        Vector2 offsetMax = new Vector2(0, 0);
        Vector2 povitPosition = new Vector2(0.5f, 0.5f);

        if (allTimeOfTheCount < 0 || allTimeOfTheCount > 999)
        {
            Debug.LogWarning("应输出准确的游戏准备时间（按秒计算的整数），时间不能是负数,且小于999秒");
        }
        while ((allTimeOfTheCount >= 0) && (allTimeOfTheCount <= 999))
        {
            if (allTimeOfTheCount >= 100 && allTimeOfTheCount <= 999)
            {
                Hundreds.enabled = true;
                Decade.enabled = true;
                Unit.enabled = true;
                //个位锚点
                Unit.rectTransform.anchorMin = new Vector2((float)2 / 3, 0f);
                Unit.rectTransform.anchorMax = new Vector2(1f, 1f);
                Unit.rectTransform.pivot = povitPosition;
                Unit.rectTransform.offsetMin = offsetMin;
                Unit.rectTransform.offsetMax = offsetMax;
                //十位锚点
                Decade.rectTransform.anchorMin = new Vector2((float)1 / 3, 0f);
                Decade.rectTransform.anchorMax = new Vector2((float)2 / 3, 1f);
                Decade.rectTransform.pivot = povitPosition;
                Decade.rectTransform.offsetMin = offsetMin;
                Decade.rectTransform.offsetMax = offsetMax;
                //百位锚点
                Hundreds.rectTransform.anchorMin = new Vector2(0, 0);
                Hundreds.rectTransform.anchorMax = new Vector2((float)1 / 3, 1f);
                Hundreds.rectTransform.pivot = povitPosition;
                Hundreds.rectTransform.offsetMin = offsetMin;
                Hundreds.rectTransform.offsetMax = offsetMax;
                //数字设置
                x = allTimeOfTheCount % 10;
                y = ((allTimeOfTheCount - x) / 10) % 10;
                z = (allTimeOfTheCount - allTimeOfTheCount % 100) / 100;
                Hundreds.sprite = spritelists[z];
                Decade.sprite = spritelists[y];
                Unit.sprite = spritelists[x];
                yield return new WaitForSeconds(1);
            }
            else if (allTimeOfTheCount >= 0 && allTimeOfTheCount < 10)
            {
                x = allTimeOfTheCount;
                Hundreds.enabled = false;
                Decade.enabled = false;
                Unit.enabled = true;
                Unit.rectTransform.anchorMin = new Vector2(0.3f, 0f);
                Unit.rectTransform.anchorMax = new Vector2(0.7f, 1f);
                Unit.rectTransform.offsetMin = new Vector2(0, 0);
                Unit.rectTransform.offsetMax = new Vector2(0, 0);
                Unit.overrideSprite = spritelists[x];
                //standardScale = Unit.rectTransform.sizeDelta;
                Unit.rectTransform.sizeDelta = standardScale * 1.5f;//new Vector2(300, 400);
                Unit.rectTransform.DOSizeDelta(new Vector2(0, 0), 0.2f);
                yield return new WaitForSeconds(1);
                if (allTimeOfTheCount == 0)
                {
                    Unit.enabled = false;
                }
            }
            else if (allTimeOfTheCount >= 10 && allTimeOfTheCount < 100)
            {
                Hundreds.enabled = false;
                Decade.enabled = true;
                Unit.enabled = true;
                //个位锚点
                Unit.rectTransform.anchorMin = new Vector2(0.5f, 0f);
                Unit.rectTransform.anchorMax = new Vector2(0.5f + (float)1 / 3, 1f);
                Unit.rectTransform.pivot = povitPosition;
                Unit.rectTransform.offsetMin = offsetMin;
                Unit.rectTransform.offsetMax = offsetMax;
                //十位锚点
                Decade.rectTransform.anchorMin = new Vector2(0.5f - (float)1 / 3, 0f);
                Decade.rectTransform.anchorMax = new Vector2(0.5f, 1f);
                Decade.rectTransform.pivot = povitPosition;
                Decade.rectTransform.offsetMin = offsetMin;
                Decade.rectTransform.offsetMax = offsetMax;
                //百位锚点
                Hundreds.rectTransform.anchorMin = new Vector2(0, 0);
                Hundreds.rectTransform.anchorMax = new Vector2((float)1 / 3, 1f);
                Hundreds.rectTransform.pivot = povitPosition;
                Hundreds.rectTransform.offsetMin = offsetMin;
                Hundreds.rectTransform.offsetMax = offsetMax;


                x = allTimeOfTheCount % 10;
                y = (allTimeOfTheCount - x) / 10;
                Unit.overrideSprite = spritelists[x];
                Decade.overrideSprite = spritelists[y];
                yield return new WaitForSeconds(1);
            }
            allTimeOfTheCount--;

            //Debug.Log("剩余时间：" + allTimeOfTheCount);
        }
    }


    #endregion
}
