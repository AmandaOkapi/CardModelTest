using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //public CardData cardDataController;
    //animation states? enum of staes??
    public Model model;
    public View view;

    [SerializeField] private float timeToFlip;
    public int cardsFlipped;
    public CardMono firstCard;
    public CardMono secondCard;

    public CardMono thirdCard;


    [SerializeField] private int serializedRow=4;
    [SerializeField] private int serializedCol=4;
    [SerializeField] private bool isMatchThreeMode;


    // Start is called before the first frame update
    void Awake()
    {
        model= new WallModel(serializedRow, serializedCol, isMatchThreeMode); 
        model.PopulateGrid();
    }

    void Start(){
        view.InitializeView(model);
        
    }

    private IEnumerator changeCardImage(CardMono card){
        float time = 0f;
        card.AnimFlipCard();
        card.PlayFlip();
        while (time < timeToFlip)
        {
            time += Time.deltaTime;
            yield return null;
        }
        card.imageComponent.sprite= card.cardData.cardImages[((Card)card.gridObjectMono.getCardBase()).getId()];        
    }
    //called by cardmono on click
    public void flipCard(CardMono card){        
        //this is screaming to be refactored
        if(cardsFlipped==0){
            card.SetEnabled(false);
            firstCard=card;
            StartCoroutine(changeCardImage(card));        
            cardsFlipped++;
        }else if(cardsFlipped==1){
            card.SetEnabled(false);
            secondCard=card;
            StartCoroutine(changeCardImage(card));        
            cardsFlipped++;
        }else{
            if(model.isMatchThreeMode()){
                if(cardsFlipped==3){
                    cardsFlipped=0;
                    if(!checkThreeFlippedCards()){
                        thirdCard.SetEnabled(true);
                        thirdCard.PlayUnflipCard();
                        thirdCard.imageComponent.sprite= secondCard.cardData.cardBack;
                        ResetFlippedCards(card);
                    }                        
                    return;
                } 
                cardsFlipped++;               
                card.SetEnabled(false);
                StartCoroutine(changeCardImage(card));        
                thirdCard=card;
                return;
            }

            //classic two matches
            cardsFlipped=0;
            if(!checkFlippedCards()){            
                ResetFlippedCards(card);
            }
        }            
    }

private void ResetFlippedCards(CardMono card){
    firstCard.SetEnabled(true);
    firstCard.PlayUnflipCard();
    secondCard.SetEnabled(true);            
    secondCard.PlayUnflipCard();
    firstCard.imageComponent.sprite= firstCard.cardData.cardBack;
    secondCard.imageComponent.sprite= secondCard.cardData.cardBack;
    firstCard=card;
    card.SetEnabled(false);
    StartCoroutine(changeCardImage(card));        
    cardsFlipped++;
}

    public bool checkFlippedCards(){
        if((((Card)firstCard.gridObjectMono.getCardBase()).getId()== ((Card)secondCard.gridObjectMono.getCardBase()).getId()) && firstCard!=secondCard){
            MatchFound(firstCard, secondCard);
            return true;
        }
        return false;
    }

    public bool checkThreeFlippedCards(){
        if((((Card)firstCard.gridObjectMono.getCardBase()).getId()== ((Card)secondCard.gridObjectMono.getCardBase()).getId()) && (((Card)firstCard.gridObjectMono.getCardBase()).getId()== ((Card)thirdCard.gridObjectMono.getCardBase()).getId()) && firstCard!=secondCard && firstCard!=thirdCard && secondCard!=thirdCard){
            MatchFound(firstCard, secondCard, thirdCard);
            return true;
        }
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
                    print("DELTED");
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
}
