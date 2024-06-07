using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void ChangeScene(string sceneName){
        PlayerPrefs.SetInt("levelNumber", -1);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void GenerateScene(int levelNumber){
        PlayerPrefs.SetInt("levelNumber", levelNumber);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GenerativeScene");
    }
}
