using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : GridObject
{
    private int id;
    public int getId(){return id;}


    public CardState cardState;

    public enum CardState{
        Idle,
        Flipped,
        Falling,
        
    }

    public Card(int id, int row, int col){
        this.id=id;
        this.rowPos = row;
        this.colPos = col;
        cardState=CardState.Idle;
    }

    public Card(int id){
        this.id=id;
        this.rowPos = -1;
        this.colPos = -1;
        cardState=CardState.Idle;
    }


}



public abstract class GridObject
{
    protected int rowPos;
    protected int colPos;
    
    public int getRowPos(){return rowPos;}
    public int getColPos(){return colPos;}

    public void setRowPos(int rp){rowPos=rp;}
    public void setColPos(int cp){colPos=cp;}
}
