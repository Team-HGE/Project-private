using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hotel_A_Loading : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadSceneA());
    }

    IEnumerator LoadSceneA()
    {
        yield return new WaitForSeconds(2);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)SceneEnum.AScene);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
