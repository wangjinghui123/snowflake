using UnityEngine;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class OnlyOne : MonoBehaviour
{
    //void Awake()
    //{
    //    if (!OnlyOne.GetInstance())
    //    {
    //        Application.Quit();
    //    }
    //}

    public static bool GetInstance()
    {
        Process pro = GetRunningInstance();
        UnityEngine.Debug.Log(" GetInstance process ! continue");
        if (pro != null)
            UnityEngine.Debug.Log("存在一个开启的程序" + pro.ProcessName);
        if (pro == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private static Process GetRunningInstance()
    {
        Process currentProcess = Process.GetCurrentProcess(); //获取当前进程 
        //获取当前运行程序完全限定名 
        string currentFileName = currentProcess.MainModule.FileName;
        UnityEngine.Debug.Log("current exe is  :" + currentProcess.ProcessName);
        //获取进程名为ProcessName的Process数组。 
        Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
        //遍历有相同进程名称正在运行的进程 
        foreach (Process process in processes)
        {
            if (process.MainModule.FileName == currentFileName)
            {
                if (process.Id != currentProcess.Id) //根据进程ID排除当前进程 
                    return process;//返回已运行的进程实例 
            }
        }
        return null;
    }

    /// <summary>
    /// 查找一个窗口,返回此窗口的句柄
    /// </summary>
    /// <param name="lpClassName">要查找的窗口的类名,如果设为null,表示适配所有类</param>
    /// <param name="lpWindowName">要查找的窗口的标题文本</param>
    /// <returns>返回窗口的句柄,如果没找到,返回0</returns>
    [DllImport("user32.DLL", EntryPoint = "FindWindowW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long FindWindow(string lpClassName, string lpWindowName);

    /// <summary>
    /// 在窗口列表中寻找与指定条件相符的第一个子窗口
    /// </summary>
    /// <param name="hwndParent">在其中查找子的父窗口。如设为零，表示使用桌面窗口（通常说的顶级窗口都被认为是桌面的子窗口，所以也会对它们进行查找）</param>
    /// <param name="hwndChildAfter">子窗口句柄。查找从在Z序中的下一个子窗口开始。子窗口必须为hwndPareRt窗口的直接子窗口而非后代窗口。如果HwndChildAfter为NULL，查找从hwndParent的第一个子窗口开始。如果hwndParent 和 hwndChildAfter同时为NULL，则函数查找所有的顶层窗口及消息窗口。</param>
    /// <param name="lpszClass">要查找的窗口的类名,如果设为null,表示适配所有类</param>
    /// <param name="lpszWindow">要查找的窗口的标题文本</param>
    /// <returns>找到的窗口的句柄。如未找到相符窗口，则返回零</returns>
    [DllImport("user32.DLL", EntryPoint = "FindWindowExW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long FindWindowEx(long hwndParent, long hwndChildAfter, string lpszClass, string lpszWindow);

    /// <summary>
    /// 将指定的窗口带至窗口列表顶部。倘若它部分或全部隐藏于其他窗口下面，则将隐藏的部分完全显示出来。该函数也对弹出式窗口、顶级窗口以及MDI子窗口产生作用
    /// </summary>
    /// <param name="hwnd">欲带至顶部的那个窗口的句柄</param>
    /// <returns>非零表示成功，零表示失败</returns>
    [DllImport("user32.DLL", EntryPoint = "BringWindowToTopW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long BringWindowToTop(long hwnd);

    /// <summary>
    /// 获得拥有输入焦点的窗口的句柄
    /// </summary>
    /// <returns>拥有焦点的那个窗口的句柄。如没有窗口拥有输入焦点，则返回零</returns>
    [DllImport("user32.DLL", EntryPoint = "GetFocus", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long GetFocus();

    /// <summary>
    /// 搜索内部窗口列表，寻找隶属于指定窗口的头一个窗口的句柄
    /// </summary>
    /// <param name="hwnd">想在其中搜寻顶级子的窗口。零表示寻找位于桌面的顶级窗口</param>
    /// <returns>Z序列中的顶级窗口也是内部窗口列表的第一个窗口</returns>
    [DllImport("user32.DLL", EntryPoint = "GetTopWindow", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long GetTopWindow(long hwnd);

    /// <summary>
    /// 返回包含了指定点的窗口的句柄。忽略屏蔽、隐藏以及透明窗口
    /// </summary>
    /// <param name="xPoint">x点值</param>
    /// <param name="yPoint">y点值</param>
    /// <returns>包含了指定点的窗口的句柄。如指定的点处没有窗口存在，则返回零</returns>
    [DllImport("user32.DLL", EntryPoint = "WindowFromPoint", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long WindowFromPoint(long xPoint, long yPoint);
}
