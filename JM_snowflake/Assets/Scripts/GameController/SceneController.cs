using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine.Video;

public class SceneController : MonoBehaviour
{
    public const bool UNITYE_DEBUG = true    ;
    public const bool NET_WORK_DEBUG = false  ;
    [SerializeField]
    private GameObject _movie;
    [SerializeField]
    private GameControllerAsTime _gameControllerAsTime;
    [SerializeField]
    private VideoTool videotool;
    [SerializeField]
    private GameObject _canvas;
    private bool _isMovie = true;
    private bool _istest = true   ;
    void Awake()
    {
        //if (!OnlyOne.GetInstance())
        //{
        //    Application.Quit();
        //}
        Application.targetFrameRate = 60;

        Application.runInBackground = true;
        UnityEngine.Debug.unityLogger.logEnabled = UNITYE_DEBUG;
        UnityEngine.Debug.unityLogger.filterLogType = LogType.Warning | LogType.Error | LogType.Log;

       // Cursor.visible = true ;
        _isMovie = videotool.Ready();
        InitializedTheGame();
    }
    void Start()
    {
        _gameControllerAsTime.StartGameEvent += StartGame;


        if (_isMovie)
        {
            Log.LogColor("开始预加载视频" + Log.GetNowTime());
            videotool.videoPlayer.prepareCompleted += OnPreCom;
        }
        else
        {
            _gameControllerAsTime.Init(_istest, 15);
        }
    }

    void OnPreCom(VideoPlayer p)
    {
        Log.LogColor("加载到可播放状态："+Log.GetNowTime());
        float m_time = videotool.GetVideoTime();
        _gameControllerAsTime.Init(_istest, m_time);
    }

    private void InitializedTheGame()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(_canvas);
        //加载视频
        if (_isMovie)
            _movie.SetActive(true);
    }
    private void StartGame(object sender, EventArgs ee)
    {
        Screen.SetResolution(1920, 1080, true);
        MyProcess curentPorcess = new MyProcess();
        MyProcess.ShowWindow(curentPorcess.GetMainWindowHandle(Process.GetCurrentProcess().Id), 3);

        UnityEngine.Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!  movie play now  !!!!!!!!!!!!!!!!!!!!!!!!!");
        //异步场景加载
        OnLoadScene(this, null);
        //跳场景事件注册
        _gameControllerAsTime.MovieTimeDoneEvent += OnMovieTimeDone;
        if (_isMovie)
            _movie.GetComponent<VideoPlayer>().Play();
        else
        {
            videotool.imagebg.gameObject.SetActive(true);
        }
        //发起网络连接
        GetComponent<NetWorkController>().StartNetWork(NET_WORK_DEBUG, _istest);
    }

    private AsyncOperation ansynLoad;
    private void OnLoadScene(object sender, EventArgs e)
    {
        StartCoroutine(LoadScene());
    }
    private IEnumerator LoadScene()
    {
        ansynLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        ansynLoad.allowSceneActivation = false;
        //TODO ansynLoad isdone? allowSceneActivation
        UnityEngine.Debug.Log("before yield return ansynLoad isdone=" + ansynLoad.isDone);
        yield return ansynLoad;
        UnityEngine.Debug.Log("******************************************* Loading has complete *********************************************************" + ansynLoad.progress + "___" + ansynLoad.isDone);
        yield return new WaitForEndOfFrame();
        if (!_isMovie)
            videotool.imagebg.gameObject.SetActive(false);
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.InitiaTheGameAndStart(gameObject, _gameControllerAsTime);
    }
    private void OnMovieTimeDone(object sender, EventArgs e)
    {
        ActiveScene();
    }
    private void ActiveScene()
    {
        UnityEngine.Debug.Log("Scene.count = " + SceneManager.sceneCount + "  " + ansynLoad.progress + "  " + ansynLoad.isDone);
        ansynLoad.allowSceneActivation = true;
    }



    public Text fps;

    //private float pastTime;
    //private int frameCount;
    //private void Update()
    //{
        
    //    frameCount++;
    //    fps.text = (frameCount / Time.time).ToString()+"__________"+(1/Time.deltaTime).ToString();
    //}

}
