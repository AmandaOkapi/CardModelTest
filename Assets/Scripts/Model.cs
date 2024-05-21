using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class Model{
    private bool matchThreeMode;
    protected bool hideTopRows;

    protected GridObject[,] cardGrid;
    protected Card[] possibleCards;

    protected int row;
    protected int col;

    public int getRow(){return row;}
    public int getCol(){return col;}

    public bool isMatchThreeMode(){return matchThreeMode;}
    public bool isHideTopRows(){return hideTopRows;}

    public Model(int row, int col){
        this.row=row;
        this.col=col;
        cardGrid= new GridObject[row,col];
        possibleCards= new Card[] {new Card(0), new Card(1), new Card(2), new Card(3),new Card(4), new Card(5), new Card(6), new Card(7), new Card(8), new Card(9), new Card(10), new Card(11), new Card(12)};
        matchThreeMode=false;
        hideTopRows=true;
    }

    public Model(int row, int col, bool matchThreeMode) : this(row, col){
        this.matchThreeMode=matchThreeMode;
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
        for(int i=cardGrid.GetLength(0)-1; i>=0; i--){
            for(int j=cardGrid.GetLength(1)-1; j>=0; j--){
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

public class OriginalModel :Model {
    public OriginalModel(int row, int col) : base(Mathf.Max(4, row), col){
        InitializeOriginalModel();
    }
    public OriginalModel(int row, int col,bool matchThreeMode) : base(Mathf.Max(4, row), col, matchThreeMode){
        InitializeOriginalModel();
    }

    private void InitializeOriginalModel(){
        possibleCards= new Card[] {new Card(0), new Card(1), new Card(2), new Card(3),new Card(4), new Card(5)};
        hideTopRows=true;
    }
    public override void RemoveGridObject(int row, int col){
        base.RemoveGridObject(row, col);
        Card newCard= CreateNewCard(0,col);
        newCard.IncreaseCellsToFall();
    }

}


public class EliminationModel : Model{
    
    private Dictionary<Card, int> cardUsgae;
    public EliminationModel(int row, int col) : base(row, col){
        InitializeEliminationModel();
    }

    public EliminationModel(int row, int col, bool matchThreeMode) : base(row, col, matchThreeMode){ 
        InitializeEliminationModel();
        }

    private void InitializeEliminationModel(){
        possibleCards= new Card[] {new Card(0), new Card(1), new Card(2), new Card(3),new Card(4), new Card(5), new Card(6), new Card(7), new Card(8), new Card(9), new Card(10), new Card(11), new Card(12)};
        cardUsgae = new Dictionary<Card, int>();
        ContructDictionary();
        hideTopRows=false;
    }
private void ContructDictionary(){
    //this code can be better
        int totalCards = getCol()* getRow();
        int setsSize = this.isMatchThreeMode()==true ? 3 : 2;

        //ensure an even numbert of cards
        totalCards -= totalCards%setsSize;
        //determine how many unique cards to use, this may be changed later for balancing
        int max = this.possibleCards.Length;
        if(possibleCards.Length*setsSize > totalCards){
            Debug.Log("special max");
            max = totalCards/setsSize;
        } 
        Debug.Log("got here1");
        //add the possible cards to the dictionary
        for(int i=0; i<max; i++){
            cardUsgae.Add(possibleCards[i], setsSize);
        }
        Debug.Log("got here2");
        //every card was assinged a pair of setsSize, assign the remaining
        int remainder = totalCards - max*setsSize; 
        for(int i=0; remainder>0; i++){
            if(i>=possibleCards.Length){
                i=0;
            }
            cardUsgae[possibleCards[i]] +=setsSize;
            remainder-=setsSize;
        }
        Debug.Log("got here3");
}
    protected override Card CreateNewCard(int row, int col){
        int randomIndex = UnityEngine.Random.Range(0, cardUsgae.Count);
        List<Card> keys = new List<Card>(cardUsgae.Keys);
        if(keys.Count ==0){
            return null;
        }
        Card selectedCard = keys[randomIndex];

        cardGrid[row,col] = new Card(selectedCard.getId(), row, col);
        cardUsgae[selectedCard]--;
        if(cardUsgae[selectedCard]<=0){
            cardUsgae.Remove(selectedCard);
        }
        Debug.Log("made : "+row +"," +col);

        return (Card)cardGrid[row,col];
    }
}


public class WallModel : Model{
    public WallModel() : base(5,9){

    }


}