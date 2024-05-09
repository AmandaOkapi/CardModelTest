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
    public void flipCard(CardMono card){        

        if(cardsFlipped==0){
            firtCard=card;        
            card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];        
            cardsFlipped++;
        }else if(cardsFlipped==1){
            secondCard=card;
            card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];
            cardsFlipped++;
        }else{
            cardsFlipped=0;
            if(!checkFlippedCards()){
                firtCard.imageComponent.sprite= firtCard.cardData.cardBack;
                secondCard.imageComponent.sprite= secondCard.cardData.cardBack;
                firtCard=card;
                card.imageComponent.sprite= card.cardData.cardImages[card.getCardBase().getId()];
                cardsFlipped++;
            }
        }            
        
        //view.UpdateView(model);

    }


    public bool checkFlippedCards(){
        if((firtCard.getCardBase().getId()== secondCard.getCardBase().getId()) && firtCard!=secondCard){
            //code straight from hell
            model.RemoveGridObject(firtCard.getCardBase().getRowPos(), firtCard.getCardBase().getColPos());
            model.RemoveGridObject(secondCard.getCardBase().getRowPos(), secondCard.getCardBase().getColPos());    
            
            //view.RemoveCard(firtCard.getCardBase().getRowPos(), firtCard.getCardBase().getColPos());            
            //view.RemoveCard(firtCard.getCardBase().getRowPos(), firtCard.getCardBase().getColPos());

            view.UpdateCollumnTest( firtCard.getCardBase().getColPos(),model);       
            view.UpdateCollumnTest( secondCard.getCardBase().getColPos(), model);    
                    
            return true;
        }
        return false;
    }
}
