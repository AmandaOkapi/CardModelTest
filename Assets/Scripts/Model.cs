using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Model{
    protected GridObject[,] cardGrid;
    protected Card[] possibleCards;

    [SerializeField] private int row=8;
    [SerializeField] private int col=6;

    public int getRow(){return row;}
    public int getCol(){return col;}
    public Model(){
        //row=30;
        //col=20;
        cardGrid= new GridObject[row,col];
        possibleCards= new Card[] {new Card(0), new Card(1), new Card(2), new Card(3),new Card(4), new Card(5), new Card(6), new Card(7), new Card(8), new Card(9), new Card(10), new Card(11), new Card(12)};
    }

    public virtual void RemoveGridObject(int row, int col){
        cardGrid[row,col]=null;
        TranslateDown(row, col);
    }

    protected virtual Card CreateNewCard(int row, int col){
        cardGrid[row,col] = new Card(UnityEngine.Random.Range(0, possibleCards.Length), row, col);
        return (Card)cardGrid[row,col];
    }

    public GridObject getCardAtIndex(int row, int col){
        return cardGrid[row,col];
    }    
    public void PopulateGrid(){
        for(int i=0; i<cardGrid.GetLength(0); i++){
            for(int j=0; j<cardGrid.GetLength(1); j++){
                cardGrid[i,j]=CreateNewCard(i,j);
            }
        }
    }    
    
    protected void TranslateDown(int row, int col){
        //recursion go brrrrrrrrrr
        if(row==0 ){
            cardGrid[row,col]=null;
            return;
        }else{
            cardGrid[row,col]=cardGrid[row-1, col];
            if(cardGrid[row,col]==null ){
                return;
            }
            cardGrid[row,col].setRowPos(row);
            if(row<this.getRow()){
                ((Card)cardGrid[row,col]).IncreaseCellsToFall();
            }
            TranslateDown(row-1, col);
        }
    }
    public void PrintArray()
    {
        String myString="";
        for (int i = 0; i < cardGrid.GetLength(0); i++)
        {
            for (int j = 0; j < cardGrid.GetLength(1); j++)
            {
                myString+="myArray[" + i + "," + j + "] = " + ((Card)cardGrid[i, j]).getId();
            }
            myString+="\n";
        } 
        Debug.Log(myString);
    }
}

[System.Serializable]
public class OriginalModel :Model {
    public OriginalModel() : base(){
        possibleCards= new Card[] {new Card(0), new Card(1), new Card(2), new Card(3),new Card(4), new Card(5), new Card(6), new Card(7), new Card(8), new Card(9), new Card(10), new Card(11), new Card(12)};
    }

    public override void RemoveGridObject(int row, int col){
        base.RemoveGridObject(row, col);
        Card newCard= CreateNewCard(0,col);
        newCard.IncreaseCellsToFall();
    }

}


public class EliminationModel : Model{
    
    private Dictionary<Card, int> cardUsgae;
    public EliminationModel() : base(){
        possibleCards= new Card[] {new Card(0), new Card(1), new Card(2), new Card(3),new Card(4), new Card(5), new Card(6), new Card(7), new Card(8), new Card(9), new Card(10), new Card(11), new Card(12)};
        cardUsgae = new Dictionary<Card, int>();
        int totalCards = getCol()* getRow();
        int max = this.possibleCards.Length;
        if(possibleCards.Length*2 > totalCards){
            Debug.Log("special max");
            max = totalCards/2;
        } 
        Debug.Log("got here1");
        for(int i=0; i<max; i++){
               if (!cardUsgae.ContainsKey(possibleCards[i])) // Check if the key already exists in the dictionary
                {
                cardUsgae.Add(possibleCards[i], 2);
                }
                else
                {
                Debug.LogWarning("Key " + possibleCards[i].getId() + " already exists in cardUsage dictionary.");
                }
        }
        Debug.Log("got here2");
        int x = totalCards - max*2; 
        
        for(int i=0; x>0; i++){
            if(i>=possibleCards.Length){
                i=0;
            }
            cardUsgae[possibleCards[i]] +=2;
            x-=2;
        }
    }


    protected override Card CreateNewCard(int row, int col){
        int randomIndex = UnityEngine.Random.Range(0, cardUsgae.Count);
        List<Card> keys = new List<Card>(cardUsgae.Keys);
        Card selectedCard = keys[randomIndex];

        cardGrid[row,col] = new Card(selectedCard.getId(), row, col);
        cardUsgae[selectedCard]--;
        if(cardUsgae[selectedCard]<=0){
            cardUsgae.Remove(selectedCard);
        }
        return (Card)cardGrid[row,col];
    }
}