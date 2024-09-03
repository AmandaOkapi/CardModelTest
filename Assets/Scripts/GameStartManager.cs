using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject match3Canvas; 
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private float match3Time, readyTime, goTime;

    private float gameTime;
    void Start()
    {
        //events
        EventManager.StartGame += StartGame;

        match3Canvas.SetActive(false);
        countdownPanel.SetActive(false);
    }

    void OnDisable()
    {
        EventManager.StartGame -= StartGame;
    }


    private void StartGame(Model model){
        gameTime = model.score.GetGameTime();
        if(model.isMatchThreeMode()){
            StartCoroutine(Match3Animation());
        }else{
            DisplayCountdown();
        }
    }

    private IEnumerator Match3Animation(){
        match3Canvas.SetActive(true);
        yield return new WaitForSeconds(match3Time);
        match3Canvas.SetActive(false);
        DisplayCountdown();
    }

    private void DisplayCountdown(){
            countdownPanel.SetActive(true);
            countdownPanel.GetComponent<Animator>().Play("Countdown");
            Invoke("HideCountdownPanel", 4.1f);
    }

    public void HideCountdownPanel(){
        countdownPanel.SetActive(false);
        EventManager.StartGameTimer(gameTime);
    }

}
