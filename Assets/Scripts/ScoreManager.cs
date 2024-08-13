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
    [SerializeField] private CardData cardData;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private GameObject textRequirementPrefab, timerPrefab, iconRequirementPrefab, scorePrefab, movesPrefab;
    [SerializeField] private GameObject textGoalGrid, iconGoalGrid;

    [Header ("Canvas elements")]
    [SerializeField] private GameObject timerOver;
    [SerializeField] private GameObject earlyWin;


    private int matchesFound, wallsDestoyed, currentCombo, movesLeft, glassDestoryed, moves;
    private int comboHighScore;

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
    [SerializeField] private Vector2 wallScoreIncreaseAmountRange;

    private int wallScoreIncreaseAmount{
        get
        {
            return (int)Random.Range(wallScoreIncreaseAmountRange.x, wallScoreIncreaseAmountRange.y);
        }
    }
    [SerializeField] private Vector2 glassScoreIncreaseAmountRange;
    private int glassScoreIncreaseAmount{
        get
        {
            return (int)Random.Range(glassScoreIncreaseAmountRange.x, glassScoreIncreaseAmountRange.y);
        }
    }
    [SerializeField] private int luckyMatchTncreaseAmount;
    public Dictionary<int, int> cardsSeen = new Dictionary<int, int>();
    

    //private Score myScore;

    private ScoreRequirements[] myScoreRequirments;  
    private ScoreRequirements[] textGoals;  
    private ScoreRequirements[] iconGoals;  
    
    //fun text increase curve shennanigans
    private int truePoints=0;
    private int displayedPoints=0;
    private float myBase =1.1f;
    private bool scoreCurveFlag= true;
    private float index =0;

    private float spd =0.1f;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.MatchFoundEvent +=MatchFound;
        EventManager.WallDestroyed +=WallsDestoryed;
        EventManager.GlassDestroyed +=GlassDestoryed;
        EventManager.MatchFailed +=MatchFailed;   
        EventManager.GameTimer +=GameTimer;   
        EventManager.InitializeView+=SetupScoreView;
        EventManager.StartGame += StartGame;
        EventManager.LuckyMatchFound += LuckyMatch;
        movesPrefab.SetActive(false);
        iconGoalGrid.SetActive(false);
        textGoalGrid.SetActive(false);
        UpdateMainScoreText();
    }

    void OnDisable()
    {
        EventManager.MatchFoundEvent -= MatchFound;
        EventManager.WallDestroyed -= WallsDestoryed;
        EventManager.GlassDestroyed -=GlassDestoryed;
        EventManager.MatchFailed -= MatchFailed;   
        EventManager.GameTimer -= GameTimer;   
        EventManager.InitializeView -= SetupScoreView;
        EventManager.StartGame -= StartGame;
        EventManager.LuckyMatchFound -= LuckyMatch;
    }

    private void StartGame(Model model){
        if(model.isMatchThreeMode()){
            scoreIncreaseAmountRange *=2;
        }
    }
    private void LuckyMatch(){
        truePoints += luckyMatchTncreaseAmount;
    }
    private void Update(){
        if(displayedPoints < truePoints){
            if(scoreCurveFlag){
                index=0;
                scoreCurveFlag=false;
            }
            //LMAO exponential curve 
            displayedPoints+=(int)( Mathf.Pow(myBase, index) *spd);
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
        movesLeft--;
        matchesFound++;
        currentCombo++;
        comboHighScore = Mathf.Max(comboHighScore, currentCombo);
        truePoints += scoreIncreaseAmount *currentCombo;

        foreach(ScoreRequirements sr in myScoreRequirments){
            if(sr is GetXMatches getXMatches){
                getXMatches.UpdateDisplayString(matchesFound);
            }else if (sr is GetXCombo getXCombo){
                getXCombo.UpdateDisplayString(comboHighScore);
            }else if (sr is GetLessThanXMoves getLessThanXMoves){
                getLessThanXMoves.UpdateDisplayString(movesLeft);
            }
        }
        UpdateScoreView();

    }
    private void MatchFailed(int id1, int id2){
        moves++;
        movesLeft--;
        comboHighScore = Mathf.Max(comboHighScore, currentCombo);
        currentCombo = 0;  
        if(myScoreRequirments==null){
            Debug.Log("Null score from match failed");
            return;
        }  else{
            Debug.Log("my score");
        } 
        foreach(ScoreRequirements sr in myScoreRequirments){
            if (sr is GetLessThanXMoves getLessThanXMoves){
                getLessThanXMoves.UpdateDisplayString(movesLeft);
            }
        }

        UpdateScoreView();

    }
    private void WallsDestoryed(){
        wallsDestoyed++;
        truePoints+=wallScoreIncreaseAmount;
        foreach(ScoreRequirements sr in myScoreRequirments){
            if(sr is DestroyAllXWalls destroyAllXWalls){
                destroyAllXWalls.UpdateDisplayString(wallsDestoyed);
            }else if(sr is DestroyXWalls destroyXWalls){
                destroyXWalls.UpdateDisplayString(wallsDestoyed);
            }
        }
        //UpdateScoreView();
        
    }

    private void GlassDestoryed(){
        glassDestoryed++;
        truePoints += glassScoreIncreaseAmount;
        foreach(ScoreRequirements sr in myScoreRequirments){
            if(sr is DestroyXGlass destroyXGlass){
                destroyXGlass.UpdateDisplayString(glassDestoryed);
            }
        }
        //UpdateScoreView();
    }

    public void SetupScoreView(Score score){
        myScoreRequirments =score.GetScoreRequirments();

        foreach(ScoreRequirements sr in  myScoreRequirments){
            sr.Reset();
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
                    GameObject newIcon = Instantiate(iconRequirementPrefab, iconGoalGrid.transform);
                    newIcon.transform.GetChild(0).GetComponent<Image>().sprite = cardData.gridObjectIcons[sr.photoID];
                    break;
            }
        }
        UpdateScoreView();
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
                    Debug.Log("looking for text goal");
                    Debug.Log(textGoalGrid.transform);
                    Debug.Log(textGoalGrid.transform.GetChild(textIndex));
                    textGoalGrid.transform.GetChild(textIndex).GetComponent<ITextPrefab>().GetText().text = sr.getDisplayString();
                    if(sr.CheckConditional(sr.gameValue)){textGoalGrid.transform.GetChild(textIndex).GetComponent<ITextPrefab>().GoalMet();}
                    textIndex++;
                    break; 
                case IconType.IconRequirement:
                    Debug.Log("looking for icon goal");
                    Debug.Log(iconGoalGrid.transform);
                    Debug.Log(iconGoalGrid.transform.GetChild(goalIndex));
                    iconGoalGrid.transform.GetChild(goalIndex).GetComponent<ITextPrefab>().GetText().text  = sr.getDisplayString();
                    if(sr.CheckConditional(sr.gameValue)){iconGoalGrid.transform.GetChild(goalIndex).GetComponent<ITextPrefab>().GoalMet();}
                    goalIndex++;
                    break;
            }
        }
        if(CheckWinConditions()){
            earlyWin.SetActive(true);
        }
            
    }

    private void UpdateMainScoreText(){
        UpdateMainScoreText(truePoints);
    }
    private void UpdateMainScoreText(int points){
        if(scoreText!=null){
            Debug.Log("updating main score text");
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
        Debug.Log("checking for win");
        bool win =true;
        bool winFlag = true;
        foreach(ScoreRequirements sr in myScoreRequirments){
            win = sr.CheckConditional(sr.gameValue);
            if(!win && winFlag){
                Debug.Log("not a win");
                winFlag = false;
            }     
        }
        //should be true here
        return winFlag;
    }
}


