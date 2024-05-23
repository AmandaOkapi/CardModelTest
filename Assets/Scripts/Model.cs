using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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

    protected virtual GridObject CreateNewGridObject(int row, int col){
        cardGrid[row,col] = new Card(UnityEngine.Random.Range(0, possibleCards.Length), row, col);
        return (Card)cardGrid[row,col];
    }

    public GridObject getCardAtIndex(int row, int col){
        return cardGrid[row,col];
    }    
    public void PopulateGrid(){
        for(int i=cardGrid.GetLength(0)-1; i>=0; i--){
            for(int j=cardGrid.GetLength(1)-1; j>=0; j--){
                cardGrid[i,j]=CreateNewGridObject(i,j);
            }
        }
    }    
    
    protected virtual void TranslateDown(int row, int col){
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
                (cardGrid[row,col]).IncreaseCellsToFall();
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
                myString+="myArray[" + i + "," + j + "] = " + (cardGrid[i, j]).name;
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
        Card newCard= (Card)CreateNewGridObject(0,col);
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
    protected override GridObject CreateNewGridObject(int row, int col){
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

    private bool[,] wallMatrix;
    
    public WallModel(int row, int col, bool matchThreeMode) : base(row,col, matchThreeMode){
        wallMatrix = new bool[row,col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                // Generate a random number between 0 and 99 (inclusive)
                float randomNumber = UnityEngine.Random.value;
                // Set the cell to true with a 10% chance
                if (randomNumber < 0.3f) {
                    wallMatrix[i, j] = true; 
                }else{
                    wallMatrix[i, j] = false;
                }
            
            }
        }
    }

    protected override GridObject CreateNewGridObject(int row, int col)
    {
        if(wallMatrix[row,col]){
            wallMatrix[row, col]=false;
            return new Wall(row, col);
        }
        return base.CreateNewGridObject(row, col);
    }
    public override void RemoveGridObject(int row, int col){
        if(getCardAtIndex(row,col) is Card){
            if(row> 0){
                if(cardGrid[row-1, col] !=null && cardGrid[row-1, col] is Wall){
                    RemoveGridObject(row-1, col);
                }
            }

            if(col> 0){
                if(cardGrid[row, col-1] !=null && cardGrid[row, col-1] is Wall){
                    RemoveGridObject(row, col-1);
                }
            }
            if(col<cardGrid.GetLength(1)-1){
                if(cardGrid[row, col+1] !=null && cardGrid[row, col+1] is Wall){
                    RemoveGridObject(row, col+1);
                }
            }            
            if(row<cardGrid.GetLength(0)-1){
                if(cardGrid[row+1, col] !=null && cardGrid[row+1, col] is Wall){
                    cardGrid[row,col]=null;
                    TranslateDown(row, col); 
                    RemoveGridObject(row+1, col);
                    return;
                }
            }
        }
        cardGrid[row,col]=null;
        TranslateDown(row, col);        
        PrintArray();
    }

    public List<int[]> CalculateWallsToDestroy(int row1, int col1, int row2, int col2){
        //graph theory is needed LOL
        List<int[]> returnList = new List<int[]>();

        TempFunction(row1, col1, returnList);
        TempFunction(row2, col2, returnList);

        return returnList;
    }

    private void TempFunction(int row, int col, List<int[]> arr){
        Debug.Log("checking");
        if(row> 0){
            if( cardGrid[row-1, col] != null && cardGrid[row-1, col] is Wall){
                arr.Add( new int[] {row-1, col}); 
            }
        }
        if(row<cardGrid.GetLength(0)-1){
            if(cardGrid[row+1, col] != null && cardGrid[row+1, col] is Wall){
                arr.Add( new int[] {row+1, col}); 
            }
        }
        if(col> 0){
            if(cardGrid[row, col-1] != null && cardGrid[row, col-1] is Wall){
                arr.Add(new int[] {row, col-1}); 
            }
        }
        if(col<cardGrid.GetLength(1)-1){
            if(cardGrid[row, col+1] != null && cardGrid[row, col+1] is Wall){
                arr.Add(new int[] {row, col+1}); 
            }
        }

    } 
    protected override void TranslateDown(int row, int col){
        //recursion go brrrrrrrrrr
        if(row==0 ){
            cardGrid[row,col]=null;
            Card newCard= (Card)CreateNewGridObject(0,col);
            newCard.IncreaseCellsToFall();
            return;
        }else{
            cardGrid[row,col]=cardGrid[row-1, col];
            if(cardGrid[row,col]==null ){
                return;
            }
            cardGrid[row,col].setRowPos(row);
            if(row<this.getRow()){
                cardGrid[row,col].IncreaseCellsToFall();
            }
            TranslateDown(row-1, col);
        }
    }
}