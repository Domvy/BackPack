using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Loading // �߰� �ε� ȭ��
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public static string nextSceneName; // ���� ��
        [SerializeField]
        Image progressBar; // �ϴ� ���� ��

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Loading());
        }
        public static void LoadingScene(string sceneName) // �ε� ȭ�� ����
        {
            nextSceneName = sceneName;
            SceneManager.LoadScene("Loading");
        }
        IEnumerator Loading() // ���� �� �ҷ�����
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

