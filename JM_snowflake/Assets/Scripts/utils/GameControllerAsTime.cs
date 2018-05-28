using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Video;

public class GameControllerAsTime : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public event EventHandler MovieTimeDoneEvent;
    public event EventHandler ReadyTimeDoneEvent;
    public event EventHandler GameTimeDoneEvent;

    public event EventHandler StartGameEvent;
    public static float allOfTheGameTime;
    // private float allOfTheGameTime =56;
    public float TimeOfGame { get { return gameTime; } }
    public int TimeOfReady { get { return readyTime; } }
    private float movieTime;


    private DateTime startatTime;
    private DateTime movieDateTimeDone;
    private DateTime readyDateTimeDone;
    private DateTime gameDateTimeDone;
    private DateTime endTime;

    private float _startatTimeTime;
    private float _movieDateTimeDoneTime;
    private float _readyDateTimeDoneTime;
    private float _gameDateTimeDoneTime;
    private float _endTimeTime;


    private int readyTime;
    private float gameTime;
    private float lastTime;
    private float minTime;
    private DateTime dateTimeNow;
    private int timeParam;
    private bool listenCurrentTime_Start;
    private bool listenCurrentTime_MovieDone;
    private bool listenCurrentTime_ReadyDone;
    private bool listenCurrentTime_GameDone;
    private bool listenCurrentTime_End;

    private string DEBUGFORMAT;
    //void OnEnable()
    //{
    //    _StartGameTimeCount();
    //}

    private System.Diagnostics.Stopwatch _st;

    public void Init(bool istest, float m_Time)
    {
        Debug.Log("movieTime:"+m_Time);
        //时间默认值设定
        allOfTheGameTime = 80;
        movieTime = m_Time > 3 ? m_Time : 3;
        readyTime = 5;
        gameTime = 20f;
        lastTime = 10f;
        minTime = movieTime + readyTime + gameTime + lastTime;
        startatTime = DateTime.MinValue;
        timeParam = 50;
        listenCurrentTime_Start = false;
        listenCurrentTime_MovieDone = false;
        listenCurrentTime_ReadyDone = false;
        listenCurrentTime_GameDone = false;
        listenCurrentTime_End = true;
        DEBUGFORMAT = "yyyy/MM/dd/HH:mm:ss.fff";
        //参数获取

        if (istest)
        {
            //----------------------------↓↓↓↓ 测试专用区域↓↓↓↓----------------------------------//
            //----------------------------         ↓↓↓↓↓↓
            allOfTheGameTime = 69;
            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = DEBUGFORMAT;
            startatTime = Convert.ToDateTime("2017/03/17/12:21:00.000", dtFormat);//时间测试

            ////王总专用口
            startatTime = Convert.ToDateTime(DateTime.Now + TimeSpan.FromMilliseconds(0d), dtFormat);
            //----------------------------         ↑↑↑↑↑↑
            //----------------------------↑↑↑↑ 测试专用区域↑↑↑↑----------------------------------//
        }
        else

            _GetImportantValue_Time();




        endTime = startatTime + new TimeSpan(0, 0, (int)allOfTheGameTime);
        dateTimeNow = DateTime.Now;
        Debug.LogFormat("real -----> now       :" + dateTimeNow.ToString(DEBUGFORMAT) + "\n");
        Debug.LogFormat("real -----> startat   :" + startatTime.ToString(DEBUGFORMAT) + "\n");
        Debug.LogFormat("real -----> endtime   :" + endTime.ToString(DEBUGFORMAT) + "\n");
        Debug.Log("time param : " + timeParam);

        if (dateTimeNow >= endTime)//当前时间大于结束时间
        {
            Debug.LogWarning("'current time' is out of 'end time'. end time is :" + endTime.ToString(DEBUGFORMAT) + "\n" +
                             "                                     but current time is :" + dateTimeNow.ToString(DEBUGFORMAT));
            Debug.Log("Application.Quit()");
            Application.Quit();
            return;
        }
        else if (dateTimeNow >= startatTime && dateTimeNow < endTime)//当前时间 > 启动时间
        {
            allOfTheGameTime = (float)(endTime - dateTimeNow).TotalSeconds;
            if (allOfTheGameTime >= minTime)
            {
                listenCurrentTime_Start = false;
                listenCurrentTime_MovieDone = true;
                listenCurrentTime_ReadyDone = false;
                listenCurrentTime_GameDone = false;
                //listenCurrentTime_End = false;
                Debug.LogError("'current time' > start time .and the residue time is enough.so play the game now.");
                gameTime = allOfTheGameTime - movieTime - readyTime - lastTime;

                startatTime = dateTimeNow;

                movieDateTimeDone = startatTime + new TimeSpan(0, 0, 0, 0, (int)(movieTime * 1000));
                readyDateTimeDone = movieDateTimeDone + new TimeSpan(0, 0, 0, 0, (int)(readyTime * 1000));
                gameDateTimeDone = readyDateTimeDone + new TimeSpan(0, 0, 0, 0, (int)(gameTime * 1000));

                _startatTimeTime = 0;
                _movieDateTimeDoneTime = movieTime * 1000;
                _readyDateTimeDoneTime = (movieTime + readyTime) * 1000;
                _gameDateTimeDoneTime = (movieTime + readyTime + gameTime) * 1000;
                _endTimeTime = (movieTime + readyTime + gameTime + lastTime) * 1000;




                _Init_DebugShow();
                _st = new System.Diagnostics.Stopwatch();
                _st.Start();
                if (StartGameEvent != null)
                    StartGameEvent(this, new EventArgs());

                Debug.Log("movieTime:" + movieTime);

                return;
            }
            else if (allOfTheGameTime < minTime)
            {
                MyProcess curentPorcess = new MyProcess();
                MyProcess.ShowWindow(curentPorcess.GetMainWindowHandle(System.Diagnostics.Process.GetCurrentProcess().Id), 3);
                Debug.LogWarning("'current time' > start time .and the residue time is not enough.so wait time done and quit .");
                movieTime = 0;
                readyTime = 0;
                gameTime = 0;
                lastTime = 0;
                listenCurrentTime_Start = false;
                listenCurrentTime_MovieDone = false;
                listenCurrentTime_ReadyDone = false;
                listenCurrentTime_GameDone = false;
                //listenCurrentTime_End = true;
                _startatTimeTime = 0;
                _movieDateTimeDoneTime = 0;
                _readyDateTimeDoneTime = 0;
                _gameDateTimeDoneTime = 0;
                _endTimeTime = 0;
                _st = new System.Diagnostics.Stopwatch();
                _st.Start();

                movieDateTimeDone = startatTime;
                readyDateTimeDone = startatTime;
                gameDateTimeDone = startatTime;

                _Init_DebugShow();
                return;
            }

        }
        else if (dateTimeNow < startatTime) //正常情况
        {
            Debug.Log("正常" + movieTime);
            gameTime = allOfTheGameTime - movieTime - readyTime - lastTime;

            if (gameTime < 10)
            {
                Application.Quit();
                return;
            }

            movieDateTimeDone = startatTime + new TimeSpan(0, 0, 0, 0, (int)(movieTime * 1000));
            readyDateTimeDone = movieDateTimeDone + new TimeSpan(0, 0, 0, 0, (int)(readyTime * 1000));
            gameDateTimeDone = readyDateTimeDone + new TimeSpan(0, 0, 0, 0, (int)(gameTime * 1000));

            _Init_DebugShow();

            listenCurrentTime_Start = true;
            listenCurrentTime_MovieDone = false;
            listenCurrentTime_ReadyDone = false;
            listenCurrentTime_GameDone = false;

            //listenCurrentTime_End = false;
            _startatTimeTime = (float)(startatTime - dateTimeNow).TotalMilliseconds;
            _movieDateTimeDoneTime = _startatTimeTime + movieTime * 1000;
            _readyDateTimeDoneTime = _movieDateTimeDoneTime + readyTime * 1000;
            _gameDateTimeDoneTime = _readyDateTimeDoneTime + gameTime * 1000;
            _endTimeTime = _gameDateTimeDoneTime + lastTime * 1000;
            _st = new System.Diagnostics.Stopwatch();
            _st.Start();
        }
        //_StartKillExe(_endTimeTime);
    }
    void FixedUpdate()
    {
        dateTimeNow = DateTime.Now;
        if (listenCurrentTime_Start)
        {
            //if ((dateTimeNow - startatTime).TotalMilliseconds > -timeParam)
            if ((_st.ElapsedMilliseconds) > -timeParam + _startatTimeTime)
            {
                listenCurrentTime_Start = false;
                listenCurrentTime_MovieDone = true;
                listenCurrentTime_ReadyDone = false;
                listenCurrentTime_GameDone = false;
                listenCurrentTime_End = false;
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!--------- Start Game Time---------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.LogFormat("start time is :" + startatTime.ToString(DEBUGFORMAT));
                Debug.LogFormat("now time is :" + dateTimeNow.ToString(DEBUGFORMAT) + "    误差：" + (dateTimeNow - startatTime).TotalMilliseconds);
                if (StartGameEvent != null)
                    StartGameEvent(this, new EventArgs());
            }
        }
        if (listenCurrentTime_MovieDone)
        {
            //if ((dateTimeNow - movieDateTimeDone).TotalMilliseconds > -timeParam)
            if ((_st.ElapsedMilliseconds) > -timeParam + _movieDateTimeDoneTime)

            {
                listenCurrentTime_Start = false;
                listenCurrentTime_MovieDone = false;
                listenCurrentTime_ReadyDone = true;
                listenCurrentTime_GameDone = false;
                listenCurrentTime_End = false;
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!--------- Movie Time Done ---------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.LogFormat("movie time done :" + movieDateTimeDone.ToString(DEBUGFORMAT));
                Debug.LogFormat("now time is :" + dateTimeNow.ToString(DEBUGFORMAT) + "    误差：" + (dateTimeNow - movieDateTimeDone).TotalMilliseconds);
                if (MovieTimeDoneEvent != null)
                    MovieTimeDoneEvent(this, new EventArgs());
            }
        }
        if (listenCurrentTime_ReadyDone)
        {
            //if ((dateTimeNow - readyDateTimeDone).TotalMilliseconds > (-timeParam + 1 * 1000))//此处的+1*1000是相对于UI层reandy时间的差值转毫秒
            if ((_st.ElapsedMilliseconds) > -timeParam + _readyDateTimeDoneTime + 500)
            {
                listenCurrentTime_Start = false;
                listenCurrentTime_MovieDone = false;
                listenCurrentTime_ReadyDone = false;
                listenCurrentTime_GameDone = true;
                listenCurrentTime_End = false;
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ---------   Ready Time Done  ---------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.LogFormat("ready time done :" + readyDateTimeDone.ToString(DEBUGFORMAT));
                Debug.LogFormat("now time is :" + dateTimeNow.ToString(DEBUGFORMAT) + "    误差：" + (dateTimeNow - readyDateTimeDone).TotalMilliseconds);
                if (ReadyTimeDoneEvent != null)
                    ReadyTimeDoneEvent(this, new EventArgs());
            }
        }
        if (listenCurrentTime_GameDone)
        {
            //if ((dateTimeNow - gameDateTimeDone).TotalMilliseconds > (-timeParam - 5 * 1000))//此处是对于生成终点的差值处理
            if ((_st.ElapsedMilliseconds) > -timeParam + _gameDateTimeDoneTime - 5 * 1000)

            {
                listenCurrentTime_Start = false;
                listenCurrentTime_MovieDone = false;
                listenCurrentTime_ReadyDone = false;
                listenCurrentTime_GameDone = false;
                listenCurrentTime_End = true;
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!--------- Game time Done ---------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.LogFormat("game time done:" + gameDateTimeDone.ToString(DEBUGFORMAT));
                Debug.LogFormat("now time is :" + dateTimeNow.ToString(DEBUGFORMAT) + "    误差：" + (dateTimeNow - gameDateTimeDone).TotalMilliseconds);
                if (GameTimeDoneEvent != null)
                    GameTimeDoneEvent(this, new EventArgs());
            }
        }
        if (listenCurrentTime_End)
        {
            //if ((dateTimeNow - endTime).TotalMilliseconds > -timeParam)
            if ((_st.ElapsedMilliseconds) > -timeParam + _endTimeTime)
            {
                listenCurrentTime_Start = false;
                listenCurrentTime_MovieDone = false;
                listenCurrentTime_ReadyDone = false;
                listenCurrentTime_GameDone = false;
                listenCurrentTime_End = false;
                Debug.Log((dateTimeNow - endTime).TotalMilliseconds);
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!--------- All time done. ---------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.LogFormat("end time is :" + endTime.ToString(DEBUGFORMAT));
                Debug.LogFormat("quit time is :" + dateTimeNow.ToString(DEBUGFORMAT) + "    误差：" + (dateTimeNow - endTime).TotalMilliseconds);
                Application.Quit();
            }
        }
    }
    private void _Init_DebugShow()
    {
        Debug.Log("all of the game time :" + allOfTheGameTime);
        Debug.Log("start time  at : " + startatTime.ToString(DEBUGFORMAT));
        Debug.Log("Movie time :" + movieTime + "\nMovie time done at : " + movieDateTimeDone.ToString(DEBUGFORMAT));
        Debug.Log("Ready time :" + readyTime + "\nReady time done at : " + readyDateTimeDone.ToString(DEBUGFORMAT));
        Debug.Log("Game time :" + gameTime + "\nGame time done at : " + gameDateTimeDone.ToString(DEBUGFORMAT));
        Debug.Log("End time :" + lastTime + "\nEnd time done at : " + endTime.ToString(DEBUGFORMAT));
    }


    /// <summary>
    /// 获取外部传参，TODO：考虑监听外部控制事件
    /// </summary>
    private void _GetImportantValue_Time()
    {
        string time = "time";
        string startat = "startat";
        string tempParam = "timeparam";
        List<string> keys = new List<string>();
        keys.Add(time);
        keys.Add(startat);
        keys.Add(tempParam);

        Dictionary<string, string> argsDictionary = new Dictionary<string, string>();
        argsDictionary.Add(time, string.Empty);
        argsDictionary.Add(startat, string.Empty);
        argsDictionary.Add(tempParam, string.Empty);

        string[] arguments = Environment.GetCommandLineArgs();
        foreach (string arg in arguments)
        {
            //Debug.Log(arg);
            if (arg.Contains("="))
            {
                string[] str = arg.Split('=');
                if (str.Length == 2 && str[0] != null && str[1] != null)
                    if (str[0].Equals(time))
                    {
                        argsDictionary[time] = str[1];
                    }
                    else if (str[0].Equals(startat))
                    {
                        argsDictionary[startat] = str[1];
                        DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
                        dtFormat.ShortDatePattern = DEBUGFORMAT;
                        startatTime = Convert.ToDateTime(argsDictionary[startat], dtFormat);
                    }
                    else if (str[0].Equals(tempParam))
                    {
                        try
                        {
                            argsDictionary[tempParam] = str[1];
                            timeParam = int.Parse(argsDictionary[tempParam]);
                        }
                        catch (System.Exception ee)
                        {
                            Debug.LogWarning(ee.Message);
                        }
                    }
                //Debug.Log(str.Length + "     " + str[0] + "   " + str[1]);
            }
        }
        for (int i = 0; i < argsDictionary.Count; i++)
        {
            Debug.Log("Get the param from exe :" + keys[i] + "   " + argsDictionary[keys[i]]);
        }
        try
        {
            allOfTheGameTime = float.Parse(argsDictionary[time]);
        }
        catch (Exception ee)
        {
            Debug.LogWarning("gameTimeParam exception:" + ee.Message);
        }
    }


    private void _StartKillExe(float time)
    {
        Debug.LogWarning(System.Diagnostics.Process.GetCurrentProcess().ProcessName + "___" + time);

        string exeName = System.IO.Path.Combine(Application.streamingAssetsPath, "KillWF.exe");
        if (!System.IO.File.Exists(exeName))
            return;
        System.Diagnostics.Process.Start(exeName, "JM_WM.exe" + " " + ((time + 1.5f) * 1000).ToString());
        //System.Diagnostics.Process.Start(exeName, "JM_WM.exe" + " " + ((4) * 1000).ToString());
    }


}
