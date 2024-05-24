using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //public CardData cardDataController;
    //animation states? enum of staes??
    public Model model;
    public View view;
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
    // Update is called once per frame
    void Update()
    {
        
    }

    //called by cardmono on click
    public void flipCard(CardMono card){        
        //this is screaming to be refactored
        if(cardsFlipped==0){
            card.SetEnabled(false);
            firstCard=card;        
            card.imageComponent.sprite= card.cardData.cardImages[((Card)card.gridObjectMono.getCardBase()).getId()];        
            cardsFlipped++;
        }else if(cardsFlipped==1){
            card.SetEnabled(false);
            secondCard=card;
            card.imageComponent.sprite= card.cardData.cardImages[((Card)card.gridObjectMono.getCardBase()).getId()];
            cardsFlipped++;
            ReenableFlippedCards(); //this doesnt work 
        }else{
            if(model.isMatchThreeMode()){
                if(cardsFlipped==3){
                    cardsFlipped=0;
                    if(!checkThreeFlippedCards()){
                        thirdCard.SetEnabled(true);
                        thirdCard.imageComponent.sprite= secondCard.cardData.cardBack;
                        ResetFlippedCards(card);
                    }                        
                    return;
                } 
                cardsFlipped++;               
                card.SetEnabled(false);
                card.imageComponent.sprite= card.cardData.cardImages[((Card)card.gridObjectMono.getCardBase()).getId()];
                thirdCard=card;
                ReenableFlippedCards();
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
    secondCard.SetEnabled(true);            
    firstCard.imageComponent.sprite= firstCard.cardData.cardBack;
    secondCard.imageComponent.sprite= secondCard.cardData.cardBack;
    firstCard=card;
    firstCard.SetEnabled(false);
    card.imageComponent.sprite= card.cardData.cardImages[((Card)card.gridObjectMono.getCardBase()).getId()];
    cardsFlipped++;
}

private void ReenableFlippedCards(){
    firstCard.SetEnabled(true);
    secondCard.SetEnabled(true);
    if(thirdCard!=null){thirdCard.SetEnabled(true);}
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

            List<int[]> list =((WallModel)model).CalculateWallsToDestroy(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos(), secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos());
            for(int i=0; i<list.Count; i++){
                view.RemoveCard(list[i][0], list[i][1]);
                print("DELTED");
            }

            if(model is WallModel){
                ((WallModel)model).RemoveWalls();
            }
            //update the model and assign base card amounts to fall, note this changes the base cards row
            model.RemoveGridObject(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos());
            model.RemoveGridObject(secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos());    
            
            //view.UpdateColumn( firstCard.gridObjectMono.getCardBase().getColPos(),model);       
            //view.UpdateColumn( secondCard.gridObjectMono.getCardBase().getColPos(), model);    
            

            for(int i=0; i<model.getCol(); i++){
                view.UpdateColumn(i, model);
            }
    }

        private void MatchFound(CardMono firstCard, CardMono secondCard, CardMono thirdCard){
            //must first be removed as the row, col positions are known, and updating the model will change them
            view.RemoveCard(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos());            
            view.RemoveCard(secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos());
            view.RemoveCard(thirdCard.gridObjectMono.getCardBase().getRowPos(), thirdCard.gridObjectMono.getCardBase().getColPos());

            //update the model and assign base card amounts to fall, note this changes the base cards row
            model.RemoveGridObject(firstCard.gridObjectMono.getCardBase().getRowPos(), firstCard.gridObjectMono.getCardBase().getColPos());
            model.RemoveGridObject(secondCard.gridObjectMono.getCardBase().getRowPos(), secondCard.gridObjectMono.getCardBase().getColPos());    
            model.RemoveGridObject(thirdCard.gridObjectMono.getCardBase().getRowPos(), thirdCard.gridObjectMono.getCardBase().getColPos());    

            view.UpdateColumn( firstCard.gridObjectMono.getCardBase().getColPos(),model);       
            view.UpdateColumn( secondCard.gridObjectMono.getCardBase().getColPos(), model);    
            view.UpdateColumn( thirdCard.gridObjectMono.getCardBase().getColPos(), model);
    }
}
