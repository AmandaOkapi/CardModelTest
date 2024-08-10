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

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject textRequirementPrefab, timerPrefab, iconRequirementPrefab, scorePrefab, movesPrefab;
    [SerializeField] private GameObject textGoalGrid, iconGoalGrid;

    [Header ("Canvas elements")]
    [SerializeField] private GameObject timerOver;
    [SerializeField] private GameObject earlyWin;


    private int matchesFound, wallsDestoyed, currentCombo, moves;

    private int comboHighScore = 0;
    private int truePoints=0;
    private int displayedPoints=0;
    private float gameTime;

    private float timePassed;


    [Header ("Adjustabe points amounts")]
    [SerializeField] private Vector2 scoreIncreaseAmountRange;
    private int scoreIncreaseAmount{
        get
        {
            return (int)Random.Range(scoreIncreaseAmountRange.x, scoreIncreaseAmountRange.y);
        }
    }
    [SerializeField] private int wallScoreIncreaseAmount;

    public Dictionary<int, int> cardsSeen = new Dictionary<int, int>();
    

    //private Score myScore;

    private ScoreRequirements[] myScoreRequirments;  
    private ScoreRequirements[] textGoals;  
    private ScoreRequirements[] iconGoals;  

    //fun text increase curve shennanigans
    private float myBase =1.1f;
    private bool scoreCurveFlag= true;
    private float index =0;


    // Start is called before the first frame update
    void Start()
    {
        EventManager.MatchFoundEvent +=MatchFound;
        EventManager.WallDestroyed +=WallsDestoryed;
        EventManager.MatchFailed +=MatchFailed;   
        EventManager.GameTimer +=GameTimer;   
        EventManager.InitializeView+=SetupScoreView;
        movesPrefab.SetActive(false);
        iconGoalGrid.SetActive(false);
        textGoalGrid.SetActive(false);
        UpdateMainScoreText();
    }

    void OnDisable()
    {
        EventManager.MatchFoundEvent -= MatchFound;
        EventManager.WallDestroyed -= WallsDestoryed;
        EventManager.MatchFailed -= MatchFailed;   
        EventManager.GameTimer -= GameTimer;   
        EventManager.InitializeView -= SetupScoreView;
    }

    private void Update(){
        if(displayedPoints < truePoints){
            if(scoreCurveFlag){
                index=0;
                scoreCurveFlag=false;
            }
            //LMAO exponential curve 
            displayedPoints+=(int) Mathf.Pow(myBase, index);
            index++;
            UpdateMainScoreText(Mathf.Clamp(displayedPoints, 0, truePoints));
        }else{
            scoreCurveFlag =true;
        }
        
    }
    private void GameTimer(float time){
        gameTime=time;
        Debug.Log("Time Start!!");
        if(time>0){
            GameObject textPart = timerPrefab.transform.GetChild(0).gameObject;
            textPart.SetActive(true);
            GameObject infinity  = timerPrefab.transform.GetChild(1).gameObject;
            infinity.SetActive(false);
            StartCoroutine(timer());   
        }else{
            GameObject textPart = timerPrefab.transform.GetChild(0).gameObject;
            textPart.SetActive(false);
            GameObject infinity  = timerPrefab.transform.GetChild(1).gameObject;
            infinity.SetActive(true);
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
        moves++;
        matchesFound++;
        currentCombo++;
        comboHighScore = Mathf.Max(comboHighScore, currentCombo);
        truePoints += scoreIncreaseAmount;
        //UpdateMainScoreText();

        foreach(ScoreRequirements sr in myScoreRequirments){
            if(sr is GetXMatches getXMatches){
                getXMatches.UpdateDisplayString(matchesFound);
            }else if (sr is GetXCombo getXCombo){
                getXCombo.UpdateDisplayString(comboHighScore);
            }else if (sr is GetLessThanXMoves getLessThanXMoves){
                getLessThanXMoves.UpdateDisplayString(moves);
            }
        }
        UpdateScoreView();
        if(CheckWinConditions()){
            earlyWin.SetActive(true);
        }

    }
    private void MatchFailed(int id1, int id2){
        moves++;
        comboHighScore = Mathf.Max(comboHighScore, currentCombo);
        currentCombo = 0;  
        if(myScoreRequirments==null){
            Debug.Log("Null score from match failed");
            return;
        }  else{
            Debug.Log("my score");
        } 
        foreach(ScoreRequirements sr in myScoreRequirments){
            if(sr is GetXCombo getXCombo){
                getXCombo.UpdateDisplayString(matchesFound);
            }else if (sr is GetLessThanXMoves getLessThanXMoves){
                getLessThanXMoves.UpdateDisplayString(moves);
            }
            UpdateScoreView();
        }

        UpdateScoreView();
        Debug.Log("updated2");

    }
    private void WallsDestoryed(){
        wallsDestoyed++;
        truePoints+=wallScoreIncreaseAmount;
        //UpdateMainScoreText();
        foreach(ScoreRequirements sr in myScoreRequirments){
            if(sr is DestroyAllXWalls destroyAllXWalls){
                destroyAllXWalls.UpdateDisplayString(wallsDestoyed);
            }else if(sr is DestroyXWalls destroyXWalls){
                destroyXWalls.UpdateDisplayString(wallsDestoyed);
            }
        }
    }



    public void SetupScoreView(Score score){
        myScoreRequirments =score.GetScoreRequirments();

        foreach(ScoreRequirements sr in  myScoreRequirments){
            switch(sr.iconType){
                case IconType.Moves:
                    movesPrefab.SetActive(true);
                    break;
                case IconType.TextRequirement:
                    textGoalGrid.SetActive(true);
                    Instantiate(textRequirementPrefab, textGoalGrid.transform);
                    break; 
                case IconType.IconRequirement:
                    iconGoalGrid.SetActive(true);
                    Instantiate(iconRequirementPrefab, iconGoalGrid.transform);
                    break;
            }
        }
        //redo me later
        //UpdateScoreView();
    }

    public void UpdateScoreView(){
        int goalIndex=0; 
        int textIndex=0;
        foreach(ScoreRequirements sr in  myScoreRequirments){
            switch(sr.iconType){
                case IconType.Moves:
                    movesPrefab.GetComponent<ITextPrefab>().GetText().text  = sr.getDisplayString();                    
                    break;
                case IconType.TextRequirement:
                    Debug.Log(textGoalGrid.transform);
                    Debug.Log(textGoalGrid.transform.GetChild(textIndex));
                    textGoalGrid.transform.GetChild(textIndex).GetComponent<TextMeshProUGUI>().text  = sr.getDisplayString();        
                    textIndex++;
                    break; 
                case IconType.IconRequirement:
                    iconGoalGrid.transform.GetChild(goalIndex).GetComponent<ITextPrefab>().GetText().text  = sr.getDisplayString();
                    goalIndex++;
                    break;
            }
        }
    }

    private void UpdateMainScoreText(){
        UpdateMainScoreText(truePoints);
    }
    private void UpdateMainScoreText(int points){
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
                    win= getXPoints.CheckConditional(truePoints);
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
                    win= getXCombo.CheckConditional(currentCombo);
                    break;
                case(GetLessThanXMoves getLessThanXMoves):
                    win= getLessThanXMoves.CheckConditional(moves);
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


