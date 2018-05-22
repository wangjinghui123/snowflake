using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public int TotalTime = 30;//总时间
    private Text timeText;
    private int munite;//分
    private int second;//秒

    void Start()
    {
        timeText = this.GetComponentInChildren <Text>();
        StartCoroutine(StartTimer());

    }
   public  IEnumerator StartTimer()
    {
        while (TotalTime >= 0)
        {
            yield return new WaitForSeconds(1);
            TotalTime--;
            timeText.text = TotalTime.ToString();
            if (TotalTime <= 0)
            {
                timeText.text = "00" + ":00";
                break;
            }
            munite = TotalTime / 60;
            second = TotalTime % 60;
            string length = munite.ToString();
            if (second >= 10)
            {
                timeText.text = "" + munite + ":" + second;
            }
            else
            {
                timeText.text = "0" + munite + ":0" + second;
            }
        }
    }

}
