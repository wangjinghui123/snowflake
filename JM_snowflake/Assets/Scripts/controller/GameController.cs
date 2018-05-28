using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJH
{


    public class GameController : MonoBehaviour
    {

        float time = 0;
        bool OnStart = false;
        float maxTime = 0;

        private static GameController instance;

        public static GameController Instance
        {
            get
            {
                return instance;
            }
        }

        public Action<float> OnReadyTime;
        public Action OnReadyEndTime;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            OnReadyTime += ((time) =>
            {
                Debug.LogError(time);
            });


        }
        /// <summary>
        /// 倒计时
        /// </summary>
        public void InitTime(float time = 3f)
        {
            maxTime = time;
            this.time = 0;
            OnStart = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (OnStart)
            {
                this.time += Time.deltaTime;
                if (time <= maxTime)
                {
                    if (OnReadyTime != null)
                    {
                        OnReadyTime(Mathf.Floor(time));
                    }
                    Debug.Log("正在倒计时:" + Mathf.Floor(time));
                }
                else
                {
                    if (OnReadyEndTime != null)
                    {
                        OnReadyEndTime();
                    }
                    OnStart = false;
                    Debug.Log("倒计时结束");
                }
            }
        }
        private void OnDestroy()
        {

        }
    }

}