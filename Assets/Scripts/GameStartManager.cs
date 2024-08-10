using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject match3Canvas; 
    [SerializeField] private GameObject readyCanvas;
    [SerializeField] private GameObject goCanvas;
    [SerializeField] private float match3Time, readyTime, goTime;

    void Start()
    {
        //events
        EventManager.StartGame += StartGame;

        match3Canvas.SetActive(false);
        readyCanvas.SetActive(false);
        goCanvas.SetActive(false);
    }

    void OnDisable()
    {
        EventManager.StartGame -= StartGame;
    }


    private void StartGame(Model model){
        if(model.isMatchThreeMode()){
            StartCoroutine(Match3Animation(model));
        }else{
            StartCoroutine(ReadyGo(model));
        }
    }

    private IEnumerator Match3Animation(Model model){
        match3Canvas.SetActive(true);
        yield return new WaitForSeconds(match3Time);
        match3Canvas.SetActive(false);
        StartCoroutine(ReadyGo(model));
    }
    private IEnumerator ReadyGo(Model model){
        readyCanvas.SetActive(true);
        yield return new WaitForSeconds(readyTime);
        readyCanvas.SetActive(false);
        goCanvas.SetActive(true);
        yield return new WaitForSeconds(goTime);
        goCanvas.SetActive(false);
        EventManager.StartGameTimer(model.score.GetGameTime());
    }
}
