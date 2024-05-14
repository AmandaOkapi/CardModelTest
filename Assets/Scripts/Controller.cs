using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //public CardData cardDataController;
    //animation states? enum of staes??
    public Model model;
    public View view;
    public static int cardsFlipped;
    public static CardMono firtCard;
    public static CardMono secondCard;
    // Start is called before the first frame update
    void Awake()
    {
        model= new Model(); 
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
            firtCard=card;        
            card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];        
            cardsFlipped++;
        }else if(cardsFlipped==1){
            card.SetEnabled(false);
            secondCard=card;
            card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];
            cardsFlipped++;
        }else{
            cardsFlipped=0;
            if(!checkFlippedCards()){            
                firtCard.SetEnabled(true);
                secondCard.SetEnabled(true);            
                firtCard.imageComponent.sprite= firtCard.cardData.cardBack;
                secondCard.imageComponent.sprite= secondCard.cardData.cardBack;
                firtCard=card;
                firtCard.SetEnabled(false);
                card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];
                cardsFlipped++;
            }
        }            
    }


    public bool checkFlippedCards(){
        if((firtCard.getCardBase().getId()== secondCard.getCardBase().getId()) && firtCard!=secondCard){
            MatchFound(firtCard, secondCard);
            return true;
        }
        return false;
    }

    private void MatchFound(CardMono firstCard, CardMono secondCard){
            //code straight from hell            
            //must first be removed as the row, col positions are known, and updating the model will change them
            view.RemoveCard(firtCard.getCardBase().getRowPos(), firtCard.getCardBase().getColPos());            
            view.RemoveCard(secondCard.getCardBase().getRowPos(), secondCard.getCardBase().getColPos());
            //update the model and assign base card amounts to fall, note this changes the base cards row
            model.RemoveGridObject(firtCard.getCardBase().getRowPos(), firtCard.getCardBase().getColPos());
            model.RemoveGridObject(secondCard.getCardBase().getRowPos(), secondCard.getCardBase().getColPos());    
            
            view.UpdateCollumn( firtCard.getCardBase().getColPos(),model);       
            view.UpdateCollumn( secondCard.getCardBase().getColPos(), model);    
    }
}
