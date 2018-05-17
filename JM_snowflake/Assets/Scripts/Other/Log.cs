using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log
{

    public static void LogColor(string str)
    {
        Debug.Log("<color=green>" + str + "</color>");
    }


    public static string GetNowTime()
    {
        return DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss.fff");
    }
}
