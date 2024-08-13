using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartButton : MonoBehaviour
{
    public void Reset(){
    // Get the name of the current active scene
    string sceneName = SceneManager.GetActiveScene().name;

    // Reload the scene with the given name
    SceneManager.LoadScene(sceneName);
    }
}
