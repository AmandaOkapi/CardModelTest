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
                cardGrid[i,j]=new Card(UnityEngine.Random.Range(0, possibleCards.Length), i, j);
            }
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
        TranslateDown(row, col);
        Card newCard= CreateNewCard(0,col);
        newCard.IncreaseCellsToFall();
    }

    private void TranslateDown(int row, int col){
        //recursion go brrrrrrrrrr
        if(row==0){
            cardGrid[row,col]=null;
            return;
        }else{
            cardGrid[row,col]=cardGrid[row-1, col];
            cardGrid[row,col].setRowPos(row);
            if(row<this.getRow()){
                ((Card)cardGrid[row,col]).IncreaseCellsToFall();
            }
            TranslateDown(row-1, col);
        }
    }

}


public class EliminationModel : Model{
    // public bool override RemoveGridObject(int row, int col){
    //     cardGrid[row,col]=null;
    //     TranslateDown(row, col);
    //     Card newCard= CreateNewCard(0,col);
    //     newCard.IncreaseCellsToFall();
    //     //PrintArray();
    //     return true;
    // }


}

