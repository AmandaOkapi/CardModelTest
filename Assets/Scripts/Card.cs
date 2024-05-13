using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : GridObject
{
    private int id;

    private int cellsToFall;

    public void IncreaseCellsToFall(){cellsToFall+=1;}
    public void ResetCellsToFall(){cellsToFall=0;}
    public int GetCellsToFall(){return cellsToFall;}
    public int getId(){return id;}

    public Card(int id, int row, int col){
        this.id=id;
        this.rowPos = row;
        this.colPos = col;
        cellsToFall=0;
    }

    public Card(int id):this(id,-1,-1){}

}

[System.Serializable]
public abstract class GridObject
{
    protected int rowPos;
    protected int colPos;
    
    public int getRowPos(){return rowPos;}
    public int getColPos(){return colPos;}

    public void setRowPos(int rp){rowPos=rp;}
    public void setColPos(int cp){colPos=cp;}
}
