using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MySceneManager : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;

    private void Start(){
        // Get the active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Get the index of the active scene
        int currentSceneIndex = currentScene.buildIndex;

        if(gameObject.tag=="Start"){
            if (currentSceneIndex == 0)
            {
                transition.SetTrigger("Idle");
            }
            else
            {
                transition.SetTrigger("StartOpen");
            }
        }
    }
    public void ChangeScene(string sceneName){
        PlayerPrefs.SetInt("levelNumber", -1);
        PlayerPrefs.Save();
        LoadNextLevel(sceneName);
    }

    public void GenerateScene(int levelNumber){
        PlayerPrefs.SetInt("levelNumber", levelNumber);
        PlayerPrefs.Save();
        LoadNextLevel("GenerativeScene");
    }

    private void LoadNextLevel(string sceneName){
        if(transition!=null){
            StartCoroutine(LoadLevel(sceneName));
        }else{
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }

    IEnumerator LoadLevel(string sceneName){
        //AsyncOperation loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        //loadingOperation.allowSceneActivation = false;
        
        transition.SetTrigger("StartClose");


        yield return new WaitForSeconds(transitionTime);        
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

/*
        while (!loadingOperation.isDone)
        {
            // get the current progress
            float loadProgress = loadingOperation.progress; 

            // if almost loaded, allow space bar to load the scene
            if (loadingOperation.progress >= 0.9f)
            {
                loadingOperation.allowSceneActivation = true;
                
            }

            // wait one frame
            yield return null;
        }*/
    }


    //dagan code
    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        // set up asynch loading
        AsyncOperation loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);

        // stop automatic scene loading if needed
        loadingOperation.allowSceneActivation = false;

        // loop till done
        while (!loadingOperation.isDone)
        {
            // get the current progress
            float loadProgress = loadingOperation.progress; 

            // if almost loaded, allow space bar to load the scene
            if (loadingOperation.progress >= 0.9f)
            {
                loadingOperation.allowSceneActivation = true;
                
            }

            // wait one frame
            yield return null;
        }
    }

}
