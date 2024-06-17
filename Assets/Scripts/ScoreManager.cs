using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ScoreManager : MonoBehaviour
{
    // [SerializeField] private TextMeshProUGUI MatchesFoundText;
    // [SerializeField] private TextMeshProUGUI WallsDestroyedText;

    // [SerializeField] private TextMeshProUGUI comboText;

    [SerializeField] private TextMeshProUGUI accuracyText;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private TextMeshProUGUI textPrefab;

    [Header ("Canvas elements")]
    [SerializeField] private GameObject timerOver;
    [SerializeField] private GameObject earlyWin;


    private int matchesFound, wallsDestoyed, combo;
    private float accuracy;

    private int points=0;
    private float gameTime;

    private float timePassed;


    [Header ("Adjustabe points amounts")]

    [SerializeField] private int scoreIncreaseAmount;
    [SerializeField] private int wallScoreIncreaseAmount;

    public Dictionary<int, int> cardsSeen = new Dictionary<int, int>();
    

    //private Score myScore;

    private ScoreRequirements[] myScoreRequirments;  

    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.MatchFoundEvent +=MatchFound;
        EventManager.WallDestroyed +=WallsDestoryed;
        EventManager.MatchFailed +=MatchFailed;   
        EventManager.GameTimer +=GameTimer;   
        EventManager.InitializeView+=SetupScoreView;
    }

    void OnDisable()
    {
        EventManager.MatchFoundEvent -= MatchFound;
        EventManager.WallDestroyed -= WallsDestoryed;
        EventManager.MatchFailed -= MatchFailed;   
        EventManager.GameTimer -= GameTimer;   
        EventManager.InitializeView -= SetupScoreView;
    }
    private void GameTimer(float time){
        gameTime=time;
        Debug.Log("Time Start!!");
        if(time>0){
            StartCoroutine(timer());   
        }
    }

    private IEnumerator timer(){
        timePassed=gameTime;
        while(timePassed>0){
            timePassed-=Time.deltaTime;
            timeText.text= string.Format("{0}:{1:00}", (int)timePassed/60, timePassed %60);
            yield return null;
        
        }
        Debug.Log("Times Up!!");
        timerOver.SetActive(true);
        Invoke("GameOver", 1f);
    } 
    private void MatchFound(int id1){
        matchesFound++;
        combo++;
        points += scoreIncreaseAmount;
        UpdateMainScoreText();

        foreach(ScoreRequirements sr in myScoreRequirments){
            if(sr is GetXMatches getXMatches){
                getXMatches.UpdateDisplayString(matchesFound);
            }else if (sr is GetXCombo getXCombo){
                getXCombo.UpdateDisplayString(combo);
            }
        }
        UpdateScoreView();
        if(CheckWinConditions()){
            earlyWin.SetActive(true);
        }
        //accurracy shit
        if (cardsSeen.ContainsKey(id1)){
            if(cardsSeen[id1]>1){
                cardsSeen[id1]-=2;
            }

        }
        int total=0;
        int missedCards=0;
        foreach (int cardSeen in cardsSeen.Values)
        {                
            total+=cardSeen;
            if(cardSeen>1){
                missedCards+=cardSeen;
            }
        }

        if(total>0){
            accuracy = (total-missedCards) / (float)total;

        }else{
            accuracy=1;
        }
        if(accuracyText!=null){
            accuracyText.text = (accuracy*100).ToString() + "%";
        }else{
        }

    }

    private void WallsDestoryed(){
        wallsDestoyed++;
        points+=wallScoreIncreaseAmount;
        UpdateMainScoreText();
        foreach(ScoreRequirements sr in myScoreRequirments){
            if(sr is DestroyAllXWalls destroyAllXWalls){
                destroyAllXWalls.UpdateDisplayString(wallsDestoyed);
            }else if(sr is DestroyXWalls destroyXWalls){
                destroyXWalls.UpdateDisplayString(wallsDestoyed);
            }
        }
    }

    private void MatchFailed(int id1, int id2){
        combo = 0;  
        if(myScoreRequirments==null){
            Debug.Log("Null score from match failed");
            return;
        }  else{
            Debug.Log("my score");
        } 
        foreach(ScoreRequirements sr in myScoreRequirments){
            if(sr is GetXCombo getXCombo){
                getXCombo.UpdateDisplayString(matchesFound);
            }
            UpdateScoreView();
        }

        UpdateScoreView();
        Debug.Log("updated2");

        //accuracy shit
        int total=0;
        int missedCards=0;
        if (!cardsSeen.ContainsKey(id1)){
            cardsSeen.Add(id1, 1);
        }
        else{
            cardsSeen[id1]++;
        }
        if (!cardsSeen.ContainsKey(id2)){
            cardsSeen.Add(id2, 1);
        }

        foreach (int cardSeen in cardsSeen.Values)
        {                
            total+=cardSeen;
            if(cardSeen>1){
                missedCards+=cardSeen;
            }
        }
        if(total>0){
            accuracy = (total-missedCards) / (float)total;

        }
        if(accuracyText!=null){
            accuracyText.text = (accuracy*100).ToString() + "%";
        }else{
            Debug.Log("acc text problem");
        }

    }

    public void SetupScoreView(Score score){
        myScoreRequirments =score.GetScoreRequirments();
        foreach(ScoreRequirements sr in  myScoreRequirments){
            TextMeshProUGUI newText = Instantiate(textPrefab, gameObject.transform);
        }
        UpdateScoreView();
    }

    public void UpdateScoreView(){
        int childCount = transform.childCount;
        // Populate the array with the children
        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).GetComponent<TextMeshProUGUI>().text  = myScoreRequirments[i].getDisplayString();
        }
    }

    private void UpdateMainScoreText(){
        if(scoreText!=null){
            scoreText.text = points.ToString();
        }
    }

    private void GameOver(){
        timerOver.SetActive(true);

        if(CheckWinConditions()){
            timerOver.GetComponentInChildren<TextMeshProUGUI>().text = "Time's Up! \n You Win!";    
            }else{
            timerOver.GetComponentInChildren<TextMeshProUGUI>().text = "Time's Up! \n You Lose!";
        }
    }

    private bool CheckWinConditions(){
        bool win =true;
        foreach(ScoreRequirements sr in myScoreRequirments){
            switch(sr){
                case(GetXPoints getXPoints):
                    win= getXPoints.CheckConditional(points);
                    break;
                case(DestroyXWalls destroyXWalls):
                    win= destroyXWalls.CheckConditional(wallsDestoyed);
                    break;
                case(DestroyAllXWalls destroyAllXWalls):
                    win= destroyAllXWalls.CheckConditional(wallsDestoyed);
                    break;
                case(GetXMatches getXMatches):
                    win= getXMatches.CheckConditional(matchesFound);
                    break;
                case(GetXCombo getXCombo):
                    win= getXCombo.CheckConditional(combo);
                    break;
            }       
            if(win== false){
                return win;
            }     
        }
        //should be true here
        return win;
    }
}


