using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : GridObject
{
    
    private int timesSeen;

    private bool isFlipped;

    public Card(int id, int row, int col):base(id){
        this.rowPos = row;
        this.colPos = col;
        timesSeen=0;
        name= id.ToString();
        isFlipped=false;
    }

    public Card(int id):this(id,-1,-1){}

    private List<int[]> wallConnections;
    public void IncreaseTimesSeen(){ timesSeen++;}
    public void flipModelCard(bool flip){
        if(flip){
            timesSeen++;
        }
        isFlipped=flip;
    }
}

public class Wall : GridObject {
    public Wall(int row, int col) :base(-1) {
        this.rowPos = row;
        this.colPos = col;
        name="W";
    }
}
[System.Serializable]
public abstract class GridObject
{
    private int id;    
    public int getId(){return id;}

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

    public void SetCellsToFall(int cellsToFall){this.cellsToFall = cellsToFall;}

    public String name;

    public GridObject(int id){
        this.id=id;
        cellsToFall=0;
    }
}
