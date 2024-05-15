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

    // Start is called before the first frame update
    void Awake()
    {
        model= new EliminationModel(serializedRow, serializedCol, true); 
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
        
        if(cardsFlipped==0){
            card.SetEnabled(false);
            firstCard=card;        
            card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];        
            cardsFlipped++;
        }else if(cardsFlipped==1){
            card.SetEnabled(false);
            secondCard=card;
            card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];
            cardsFlipped++;
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
                card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];
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
    secondCard.SetEnabled(true);            
    firstCard.imageComponent.sprite= firstCard.cardData.cardBack;
    secondCard.imageComponent.sprite= secondCard.cardData.cardBack;
    firstCard=card;
    firstCard.SetEnabled(false);
    card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];
    cardsFlipped++;
}
    public bool checkFlippedCards(){
        if((firstCard.getCardBase().getId()== secondCard.getCardBase().getId()) && firstCard!=secondCard){
            MatchFound(firstCard, secondCard);
            return true;
        }
        return false;
    }

    public bool checkThreeFlippedCards(){
        if((firstCard.getCardBase().getId()== secondCard.getCardBase().getId()) && (firstCard.getCardBase().getId()== thirdCard.getCardBase().getId()) && firstCard!=secondCard && firstCard!=thirdCard && secondCard!=thirdCard){
            MatchFound(firstCard, secondCard, thirdCard);
            return true;
        }
        return false;
    }

    private void MatchFound(CardMono firstCard, CardMono secondCard){
            //code straight from hell            
            //must first be removed as the row, col positions are known, and updating the model will change them
            view.RemoveCard(firstCard.getCardBase().getRowPos(), firstCard.getCardBase().getColPos());            
            view.RemoveCard(secondCard.getCardBase().getRowPos(), secondCard.getCardBase().getColPos());
            //update the model and assign base card amounts to fall, note this changes the base cards row
            model.RemoveGridObject(firstCard.getCardBase().getRowPos(), firstCard.getCardBase().getColPos());
            model.RemoveGridObject(secondCard.getCardBase().getRowPos(), secondCard.getCardBase().getColPos());    
            
            view.UpdateCollumn( firstCard.getCardBase().getColPos(),model);       
            view.UpdateCollumn( secondCard.getCardBase().getColPos(), model);    
    }

        private void MatchFound(CardMono firstCard, CardMono secondCard, CardMono thirdCard){
            //must first be removed as the row, col positions are known, and updating the model will change them
            view.RemoveCard(firstCard.getCardBase().getRowPos(), firstCard.getCardBase().getColPos());            
            view.RemoveCard(secondCard.getCardBase().getRowPos(), secondCard.getCardBase().getColPos());
            view.RemoveCard(thirdCard.getCardBase().getRowPos(), thirdCard.getCardBase().getColPos());

            //update the model and assign base card amounts to fall, note this changes the base cards row
            model.RemoveGridObject(firstCard.getCardBase().getRowPos(), firstCard.getCardBase().getColPos());
            model.RemoveGridObject(secondCard.getCardBase().getRowPos(), secondCard.getCardBase().getColPos());    
            model.RemoveGridObject(thirdCard.getCardBase().getRowPos(), thirdCard.getCardBase().getColPos());    

            view.UpdateCollumn( firstCard.getCardBase().getColPos(),model);       
            view.UpdateCollumn( secondCard.getCardBase().getColPos(), model);    
            view.UpdateCollumn( thirdCard.getCardBase().getColPos(), model);
    }
}
