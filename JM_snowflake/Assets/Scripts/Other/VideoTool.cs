using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Threading;

public class VideoTool : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public Image imagebg;
    public bool Ready()
    {
        Debug.Log("VideoTool Ready");
        string picPath;
        bool ismovie = false;
        picPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Movie");
        if (System.IO.Directory.Exists(picPath))
        {
            System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(picPath);
            foreach (System.IO.FileInfo info in root.GetFiles())
            {
                Debug.Log(info.Extension);
                if (info.Extension == ".png" || info.Extension == ".jpg")
                {
                    System.IO.FileStream fs = new System.IO.FileStream(info.FullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    fs.Dispose();
                    Texture2D texture2D = new Texture2D(0, 0);
                    texture2D.LoadImage(data);
                    imagebg.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);
                    data = null;
                    break;
                }
                else if (info.Extension == ".avi" || info.Extension == ".mp4")
                {
                    Log.LogColor(info.FullName);
                    Log.LogColor(info.Length.ToString());
                    Log.LogColor(Qy_CSharp_NetWork.Tools.Json.JsonTools.ToJson(info));


                    ismovie = true;
                    videoPlayer.url = info.FullName;

                    videoPlayer.Prepare();

                    //videoPlayer.SetTargetAudioSource(0,videoPlayer.GetComponent<AudioSource>());


                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("___not Exist");
        }
        Debug.Log("IsMovie:"+ismovie);
        return ismovie;
    }

    public float GetVideoTime()
    {
        Log.LogColor("帧数" + videoPlayer.frameCount.ToString());
        Log.LogColor("帧速率" + videoPlayer.frameRate.ToString());
        float time = (videoPlayer.frameCount / videoPlayer.frameRate);
        return time;
    }


    private void VideoDone(VideoPlayer videoPlayer)
    {
        Log.LogColor("video done!");
    }
    private void FirstReady(VideoPlayer videoPlayer, long i)
    {
        Log.LogColor("video FirstReady!" + i);
    }
    private void ReadyCompelete(VideoPlayer videoPlayer)
    {
        Log.LogColor("video ReadyCompelete!");
    }
    private void Started(VideoPlayer videoPlayer)
    {
        Log.LogColor("video Started!");
    }



}


