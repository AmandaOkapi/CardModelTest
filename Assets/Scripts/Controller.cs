using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Array2DEditor;
using Unity.VisualScripting;
using UnityEngine.UI;
public class Controller : MonoBehaviour
{
    //public CardData cardDataController;
    //animation states? enum of staes??
    public Model model;
    public View view;

    public GameObject resetFlipGuard;
    [SerializeField] private float timeToFlip;
    public int cardsFlipped;
    public CardMono firstCard=null;
    public CardMono secondCard=null;

    public CardMono thirdCard=null;

    [Header ("Level Editor? Ain't no way")]
    [SerializeField] private Array2DEditor.Array2DBool serializedCustomWall = null;    
    [SerializeField] private List<int> customPossibleCards;
    [SerializeField] private int serializedRow=4;
    [SerializeField] private int serializedCol=4;
    [SerializeField] private bool isMatchThreeMode;
    
    [Header ("Score")]
    [SerializeField] private float gameTime;
    private Score score;




    public int levelNumber;

    // Start is called before the first frame update
    void Awake()
    {
        levelNumber = PlayerPrefs.GetInt("levelNumber");

        if(levelNumber<0){
            if(serializedCustomWall !=null && serializedCustomWall.GridSize.y>1 ){
                bool[,] customWall = new bool[serializedCustomWall.GridSize.y, serializedCustomWall.GridSize.x];
                Debug.Log("rows " + customWall.GetLength(0) + " cols " + customWall.GetLength(1));
                //convert to bool[,]
                
                for (var y = 0; y < serializedCustomWall.GridSize.y; y++)
                {
                    for (var x = 0; x < serializedCustomWall.GridSize.x; x++)
                    {
                        customWall[y,x] = serializedCustomWall.GetCell(x,y);
                    }
                }
                model= new WallModelDestroyWalls(customWall.GetLength(0), customWall.GetLength(1), isMatchThreeMode, customWall ); 
            }else{
                model= new WallModelDestroyWalls(serializedRow, serializedCol, isMatchThreeMode); 
            }

            if(customPossibleCards.Count>0){
                model.SetPossibleCards(customPossibleCards);
            }

            score = new Score(gameTime, new DestroyAllXWalls(((WallModel)model).GetWallCount()),
                new GetXMatches(20));

        }

    }

    void Start(){
        resetFlipGuard.SetActive(false);
        Debug.Log("Game sStart");
        if(levelNumber>=0){
            model = LevelDataBase.levels[levelNumber].model;
            score = LevelDataBase.levels[levelNumber].score;
        }
        model.PopulateGrid();        

        view.InitializeView(model);

        if(score!=null){
            model.score=score;        
            if (score.GetGameTime()>0){
                Debug.Log("Event Start!!");
            }       
            Invoke("RunAfterStart", 0f);
        }     

    }
    private void RunAfterStart(){
        EventManager.StartGameTimer(model.score.GetGameTime());
        EventManager.StartInitializeView(model.score);
    }

    //called by cardmono on click
    public void flipCard(CardMono card){        
        if(View.cardsFalling>0 || view.IsPowerUpPlaying()){
            return;
        }

        if(view.IsFrenzy()){
            if(cardsFlipped==0){
                card.SetEnabled(false);
                firstCard=card;
                ((Card)(card.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(true);
                card.ShowFrenzy(true);
                cardsFlipped++;
            }else if(cardsFlipped==1){
                card.SetEnabled(false);
                secondCard=card;
                ((Card)(card.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(true);
                card.ShowFrenzy(true);
                cardsFlipped++;
                if(!model.isMatchThreeMode()){
                    resetFlipGuard.SetActive(true);
                }
            }else{
                //questionable match 3 code
                        card.SetEnabled(false);
                        ((Card)(card.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(true);
                        thirdCard=card;
                        card.ShowFrenzy(true);
                        cardsFlipped++;                    
                        resetFlipGuard.SetActive(true);    

                // //classic two matches
                // cardsFlipped=0;
                // if(!checkFlippedCards()){   
                //     ResetFlippedCards();
                // }
            }
            return;
        }
        //this is screaming to be refactored
        if(cardsFlipped==0){
            card.SetEnabled(false);
            firstCard=card;
            ((Card)(card.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(true);
            card.ShowflipCard();
            cardsFlipped++;
        }else if(cardsFlipped==1){
            card.SetEnabled(false);
            secondCard=card;
            ((Card)(card.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(true);
            card.ShowflipCard();
            cardsFlipped++;
            if(!model.isMatchThreeMode()){
                resetFlipGuard.SetActive(true);
            }
        }else{
            //questionable match 3 code
                    card.SetEnabled(false);
                    ((Card)(card.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(true);
                    thirdCard=card;
                    card.ShowflipCard();
                    cardsFlipped++;                    
                    resetFlipGuard.SetActive(true);    
        }            
    }

    public void FlipGuardPressed(){
        resetFlipGuard.SetActive(false);
        cardsFlipped=0;

        if(model.isMatchThreeMode()){
            if(!checkThreeFlippedCards()){
                thirdCard.SetEnabled(true);
                thirdCard.ShowUnflipCard(true);
                ResetFlippedCards();
            }     
        }else{            
            if(!checkFlippedCards()){   
                ResetFlippedCards();
            }
        }

    }
    public void flipCard(int rowPos, int colPos){ 
        if(View.cardsFalling>0 || view.IsPowerUpPlaying()){
            return;
        }
        Card newCard = (Card)model.getObjectAtIndex(rowPos,colPos);

    }       

private void ResetFlippedCards(){
        if(view.IsFrenzy()){
            firstCard.SetEnabled(true);
            ((Card)(firstCard.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(false);
            firstCard.ShowFrenzy(false);
            secondCard.SetEnabled(true);        
            ((Card)(secondCard.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(false);    
            secondCard.ShowFrenzy(false);
            if(thirdCard!=null){
                thirdCard.SetEnabled(true);        
                ((Card)(thirdCard.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(false);    
                thirdCard.ShowFrenzy(false);
                thirdCard=null;
            }
            return;            
        }
    firstCard.SetEnabled(true);
    ((Card)(firstCard.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(false);
    firstCard.ShowUnflipCard(true);
    secondCard.SetEnabled(true);        
    ((Card)(secondCard.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(false);    
    secondCard.ShowUnflipCard(true);
    if(thirdCard!=null){
        thirdCard.SetEnabled(true);        
        ((Card)(thirdCard.GetComponent<GridObjectMono>().getCardBase())).flipModelCard(false);    
        thirdCard.ShowUnflipCard(true);
        thirdCard=null;
    }
}

    public bool checkFlippedCards(){
        Card fc = (Card)firstCard.gridObjectMono.getCardBase();
        Card sc = (Card)secondCard.gridObjectMono.getCardBase();
        if((fc.getId()== sc.getId()) && firstCard!=secondCard){            
            //check for lucky match
            if(fc.GetTimesSeen() == 1 && sc.GetTimesSeen() == 1){
                Debug.Log("starting lucky match");
                view.InstantiateLuckyMatch(secondCard.transform.position);
                EventManager.StartLuckyMatchFound();
            }
            MatchFound(firstCard, secondCard);
            //Debug.Log("hello from check flipped cards");
            EventManager.StartMatchFoundEvent(firstCard.gridObjectMono.getCardBase().getId());

            return true;
        }
        EventManager.StartMatchFailed(firstCard.gridObjectMono.getCardBase().getId(), secondCard.gridObjectMono.getCardBase().getId());
        return false;
    }

    public bool checkThreeFlippedCards(){
        Card fc = (Card)firstCard.gridObjectMono.getCardBase();
        Card sc = (Card)secondCard.gridObjectMono.getCardBase();
        Card tc = (Card)thirdCard.gridObjectMono.getCardBase();

        if((fc.getId()== (sc.getId()) && (fc.getId()== tc.getId()) && firstCard!=secondCard && firstCard!=thirdCard && secondCard!=thirdCard)){
            //check for lucky match
            if(fc.GetTimesSeen() == 1 && sc.GetTimesSeen() == 1 && tc.GetTimesSeen()==1){
                view.InstantiateLuckyMatch(thirdCard.transform.position);
                EventManager.StartLuckyMatchFound();
            }
            MatchFound(firstCard, secondCard, thirdCard);
            Debug.Log("hello from check 3 flipped cards");
            EventManager.StartMatchFoundEvent(firstCard.gridObjectMono.getCardBase().getId() );

            return true;
        }
        EventManager.StartMatchThreeFaileddEvent(firstCard.gridObjectMono.getCardBase().getId(), secondCard.gridObjectMono.getCardBase().getId(), thirdCard.gridObjectMono.getCardBase().getId());
        return false;
    }

    private void MatchFound(CardMono firstCard, CardMono secondCard){
            //code straight from hell            
            //must first be removed as the row, col positions are known, and updating the model will change them
            view.RemoveCard(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos());            
            view.RemoveCard(secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos());
            if(model is WallModel){
                List<int[]> list =((WallModel)model).CalculateWallsToDestroy(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos(), secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos());
                for(int i=0; i<list.Count; i++){
                    view.RemoveCard(list[i][0], list[i][1]);
                    EventManager.StartWallDestroyedEvent();
                    print("DELETED "+ list[i][0] + " "+ list[i][1]);
                }
                
                ((WallModel)model).RemoveWalls();
            }

            //update the model and assign base card amounts to fall, note this changes the base cards row
            model.RemoveGridObject(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos());
            model.RemoveGridObject(secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos());            

            if(model is WallModel){
                for(int i=0; i<model.getCol(); i++){
                    view.UpdateColumn(i, model);
                }
            }else{
                view.UpdateColumn( firstCard.gridObjectMono.getCardBase().getColPos(),model);       
                view.UpdateColumn( secondCard.gridObjectMono.getCardBase().getColPos(), model);    
            }
    }

        private void MatchFound(CardMono firstCard, CardMono secondCard, CardMono thirdCard){
            //must first be removed as the row, col positions are known, and updating the model will change them
            view.RemoveCard(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos());            
            view.RemoveCard(secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos());
            view.RemoveCard(thirdCard.gridObjectMono.getCardBase().getRowPos(), thirdCard.gridObjectMono.getCardBase().getColPos());
            
            if(model is WallModel){
                List<int[]> list =((WallModel)model).CalculateWallsToDestroy(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos(), 
                                                                                secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos(),
                                                                                thirdCard.gridObjectMono.getCardBase().getRowPos(), thirdCard.gridObjectMono.getCardBase().getColPos());
                for(int i=0; i<list.Count; i++){
                    view.RemoveCard(list[i][0], list[i][1]);
                    print("DELTED");
                }
                
                ((WallModel)model).RemoveWalls();
            }
            //update the model and assign base card amounts to fall, note this changes the base cards row
            model.RemoveGridObject(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos());
            model.RemoveGridObject(secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos());    
            model.RemoveGridObject(thirdCard.gridObjectMono.getCardBase().getRowPos(), thirdCard.gridObjectMono.getCardBase().getColPos());    
            if(model is WallModel){
                for(int i=0; i<model.getCol(); i++){
                    view.UpdateColumn(i, model);
                }
            }else{
                view.UpdateColumn( firstCard.gridObjectMono.getCardBase().getColPos(),model);       
                view.UpdateColumn( secondCard.gridObjectMono.getCardBase().getColPos(), model);    
                view.UpdateColumn( thirdCard.gridObjectMono.getCardBase().getColPos(), model);
            }
    }




    public void UnflipCurrentlyFlippedCards(){
        //fix it so unflipped cards dont flip again for powerups
        if (firstCard != null && firstCard.gameObject != null)
        {
            firstCard.ShowUnflipCard(false);
            firstCard.SetEnabled(true);
        }

        if (secondCard != null && secondCard.gameObject != null)
        {
            secondCard.ShowUnflipCard(false);
            secondCard.SetEnabled(true);
        }

        if (thirdCard != null && thirdCard.gameObject != null)
        {
            thirdCard.ShowUnflipCard(false);
            thirdCard.SetEnabled(true);
        }
        cardsFlipped=0;
    }


}
