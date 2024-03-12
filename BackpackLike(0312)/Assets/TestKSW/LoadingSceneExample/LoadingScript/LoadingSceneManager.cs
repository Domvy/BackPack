using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Loading // 중간 로딩 화면
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public static string nextSceneName; // 다음 씬
        [SerializeField]
        Image progressBar; // 하단 진행 바

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Loading());
        }
        public static void LoadingScene(string sceneName) // 로딩 화면 실행
        {
            nextSceneName = sceneName;
            SceneManager.LoadScene("Loading");
        }
        IEnumerator Loading() // 다음 씬 불러오기
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
            operation.allowSceneActivation = false;
            progressBar.fillAmount = 0;
            while (!operation.isDone)
            {
                progressBar.fillAmount = operation.progress;
                if (0.9f <= operation.progress)
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }
                yield return null;
            }
        }
    }
}

