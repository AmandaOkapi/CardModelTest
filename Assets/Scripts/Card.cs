using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : GridObject
{
    private int id;

    public int getId(){return id;}

    public Card(int id, int row, int col):base(){
        this.id=id;
        this.rowPos = row;
        this.colPos = col;
        name= id.ToString();
    }

    public Card(int id):this(id,-1,-1){}

    private List<int[]> wallConnections;

}

public class Wall : GridObject {
    public Wall(int row, int col) {
        this.rowPos = row;
        this.colPos = col;
        name="W";
    }
}
[System.Serializable]
public abstract class GridObject
{
    public static float fallSpeed = 0.5f; 
    protected int rowPos;
    protected int colPos;
    
    public int getRowPos(){return rowPos;}
    public int getColPos(){return colPos;}

    public void setRowPos(int rp){rowPos=rp;}
    public void setColPos(int cp){colPos=cp;}

    private int cellsToFall;

    public void IncreaseCellsToFall(){cellsToFall+=1;}
    public void ResetCellsToFall(){cellsToFall=0;}
    public int GetCellsToFall(){return cellsToFall;}

    public String name;

    public GridObject(){
        cellsToFall=0;
    }
}
