using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace WJH
{


    public class SceneController : MonoBehaviour
    {
        static SceneController instance;
        AsyncOperation async;
        public static SceneController Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject g = new GameObject("SceneController");
                    instance = g.AddComponent<SceneController>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            instance = this;

        }
        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName">切换到目标场景</param>
        /// <param name="action">切换到目标场景后要做的事情</param>
        public void ChangeScene(string sceneName, Action action = null)
        {
            SceneManager.LoadScene("loadingScene", LoadSceneMode.Additive);

            StartCoroutine(LoadScene(sceneName, action));
        }

        IEnumerator LoadScene(string sceneName, Action action)
        {
            async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            async.allowSceneActivation = false;
            print(async.progress);
            float value = async.progress;
            //场景加载好后不自动切换



            while (async.progress < 0.9f)
            {
                yield return null;
                print("加载进度:      " + async.progress);
            }
            value = 0.9f;
            while (value < 1f)
            {
                value += 0.01f;
                yield return new WaitForSeconds(0.1f);
            }
            yield return async;
            yield return new WaitForSeconds(2f);
            SceneManager.UnloadSceneAsync("loadingScene");
            ; async.allowSceneActivation = true;
            if (action != null)
            {
                action();
            }
        }





    }
}