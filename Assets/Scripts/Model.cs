using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model 
{
    private GridObject[,] cardGrid;
    private Card[] possibleCards;

    private int row;
    private int col;

    public int getRow(){return row;}
    public int getCol(){return col;}
    public Model(){
        row=4;
        col=4;
        cardGrid= new GridObject[row,col];
        possibleCards= new Card[] {new Card(0), new Card(1), new Card(2), new Card(3),new Card(4), new Card(5), new Card(6), new Card(7)  };
    }

    public void PopulateGrid(){
        for(int i=0; i<cardGrid.GetLength(0); i++){
            for(int j=0; j<cardGrid.GetLength(1); j++){
                cardGrid[i,j]=new Card(UnityEngine.Random.Range(0, possibleCards.Length), i, j);
            }
        }
    }

    public bool RemoveGridObject(int row, int col){
        cardGrid[row,col]=null;
        TranslateDown(row, col);
        CreateNewCard(0,col);
        return true;
    }

    //current bug = problem when t2o of same card are on top of eachother
    private void TranslateDown(int row, int col){
        //recursion go brrrrrrrrrr
        if(row==0){
            cardGrid[row,col]=null;
            return;
        }else{
            cardGrid[row,col]=cardGrid[row-1, col];
            cardGrid[row,col].setRowPos(row);
            TranslateDown(row-1, col);
        }
    }

    /*
        public bool RemoveGridObject(GridObject obj){
        //translate down?
        return true;
    }*/

    private void CreateNewCard(int row, int col){
        cardGrid[row,col] = new Card(UnityEngine.Random.Range(0, possibleCards.Length), row, col);
    }

    public GridObject getCardAtIndex(int row, int col){
        return cardGrid[row,col];
    }



    
}


