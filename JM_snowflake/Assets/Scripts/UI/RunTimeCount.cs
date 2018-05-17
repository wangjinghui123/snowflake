using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class RunTimeCount : MonoBehaviour
{
    public System.EventHandler runTimeOverEvent;
    public System.EventHandler runTimeRemaining_5s;




    public float allRunTime;
    public float currentTime;
    public float minDeltaTime = 0.02f;

    private int _hourDacade;
    private int _hourUnit;
    private int _minuteDacade;
    private int _minuteUnit;
    private int _secondDacaed;
    private int _seconUnit;

    [SerializeField]
    private Image _Hour_Dacade;
    [SerializeField]
    private Image _Hour_Unit;
    [SerializeField]
    private Image _Minute_Dacade;
    [SerializeField]
    private Image _Minute_Unit;
    [SerializeField]
    private Image _Second_Dacade;
    [SerializeField]
    private Image _Second_Unit;

    //初始化动画大小
    Vector2 standardScale=Vector2.zero;
    //数字保存路径
    private string pathOfNumber;
    //数字图片列表
    List<Sprite> spritelists;// = new List<Sprite>();
    //资源下载路径和类型
    TextureDataPath myTextureType = TextureDataPath.resource;


    void OnEnable()
    {
        pathOfNumber = "Number";
        TextureInitialize(myTextureType);//初始化图片数据，选择格式通过，确保路径  note：Asset

        InvokeRepeating("_ChangeNum", 0, Time.fixedDeltaTime);
    }

    void OnDisable()
    {
        CancelInvoke("_ChangeNum");//-----------------------------------------TODO
        switch (myTextureType)
        {
            case TextureDataPath.resource:
                //Resources.UnloadUnusedAssets();
                break;
            case TextureDataPath.streamingAssetsPath:
                for (int i = 0; i < spritelists.Count; i++)
                {
                    Destroy(spritelists[i]);
                }
                break;
        }
        spritelists.Clear();
    }


    private enum TextureDataPath
    {
        streamingAssetsPath = 0,
        resource = 2,
        asset = 3
    }
    /// <summary>
    /// 读取数字图片
    /// </summary>
    /// <param name="texturePathType">图片所在文件夹类型</param>
    private void TextureInitialize(TextureDataPath texturePathType)
    {
        if (spritelists == null || spritelists.Count == 0)
        {
            if (spritelists == null)
                spritelists = new List<Sprite>();

            //Debug.Log(standardScale);
            //数字获取图片
            int _index = 10;
            switch (texturePathType)
            {
                case TextureDataPath.streamingAssetsPath:
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
                    break;
                case TextureDataPath.resource:
                    for (int i = 0; i < _index; i++)
                    {
                        string path = pathOfNumber + @"/" + i.ToString();// + @"/" + i.ToString();

                        Sprite tempSprite = new Sprite();
                        tempSprite = Resources.Load(path, typeof(Sprite)) as Sprite;
                        if (tempSprite == null)
                            Debug.LogWarning("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  " + path);
                        spritelists.Add(tempSprite);
                    }
                    break;
                default:
                    Debug.Log("标记图片资源路径有误，设置类内结构体");
                    break;
            }
        }


        _hourDacade = 0;
        _hourUnit = 0;
        _minuteDacade = 0;
        _minuteUnit = 0;
        _secondDacaed = 0;
        _seconUnit = 0;

        _Hour_Dacade.sprite = spritelists[_hourDacade];
        _Hour_Unit.sprite = spritelists[_hourUnit];
        _Minute_Dacade.sprite = spritelists[_minuteDacade];
        _Minute_Unit.sprite = spritelists[_minuteUnit];
        _Second_Dacade.sprite = spritelists[_secondDacaed];
        _Second_Unit.sprite = spritelists[_seconUnit];

        currentTime = 0;
    }



    public void StopTheRunTime()
    {
        CancelInvoke("_ChangeNum");
    }


    private void _ChangeNum()
    {

        currentTime += minDeltaTime;
        if (Mathf.Abs((allRunTime - currentTime) - 7f) <= 0.01 && runTimeRemaining_5s != null)
        {
            Debug.Log("------------- runTimeRemaining_5s -----------------");
            runTimeRemaining_5s(this, null);
        }
        if (currentTime >= allRunTime)
        {
            if (runTimeOverEvent != null)
                runTimeOverEvent(this, null);
            CancelInvoke("_ChangeNum");
            return;
        }
        _seconUnit += int.Parse((minDeltaTime * 100).ToString());
        //Debug.Log(int.Parse((Time.fixedDeltaTime * 200).ToString()) + "   " + Time.fixedDeltaTime + "  " + Time.fixedDeltaTime * 100f + "  float   " + (int)2f);
        //lastSecond = _seconUnit;
        if (_seconUnit >= 10)
        {
            _seconUnit = 0;
            _secondDacaed += 1;
        }
        // Debug.Log("lastSecond:" + lastSecond + "  currentSecond:" + _seconUnit);
        if (_secondDacaed >= 10)
        {
            _secondDacaed = 0;
            _minuteUnit += 1;
        }
        if (_minuteUnit >= 10)
        {
            _minuteUnit = 0;
            _minuteDacade += 1;
        }
        if (_minuteDacade >= 6)
        {
            _minuteDacade = 0;
            _hourUnit += 1;
        }
        if (_hourUnit >= 10)
        {
            _hourUnit = 0;
            _hourDacade += 1;
        }
        if (_hourDacade >= 10)
        {
            _hourDacade = 0;
            _hourUnit = 0;
            _minuteDacade = 0;
            _minuteUnit = 0;
            _secondDacaed = 0;
            _seconUnit = 0;
        }


        //Debug.Log(currentTime);
        _ChangeTexture();

    }
    private void _ChangeTexture()
    {
        //Debug.Log(_hourDacade + "   " + _minuteUnit + "------------------------------");
        _Hour_Dacade.sprite = spritelists[_hourDacade];
        _Hour_Unit.sprite = spritelists[_hourUnit];
        _Minute_Dacade.sprite = spritelists[_minuteDacade];
        _Minute_Unit.sprite = spritelists[_minuteUnit];
        _Second_Dacade.sprite = spritelists[_secondDacaed];
        _Second_Unit.sprite = spritelists[_seconUnit];

    }


}
