using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class Model{
    private int rowsToHide;
    private bool matchThreeMode;
    private bool hasGlass;
    private bool hideTopRows;
    protected GridObject[,] cardGrid;
    protected Card[] possibleCards;

    protected int row;
    protected int col;

    public int getRow(){return row;}
    public int getCol(){return col;}
    public int getRowsToHide(){return rowsToHide;}

    public bool isMatchThreeMode(){return matchThreeMode;}
    public bool isHideTopRows(){return hideTopRows;}

    public bool isHasGlass(){return hasGlass;}
    public Score score;
    
    public Model(int row, int col, int rowsToHide, bool hideTopRows){
        this.row=row;
        this.col=col;
        cardGrid= new GridObject[row,col];
        possibleCards= new Card[] {new Card(0), new Card(1), new Card(2), new Card(3),new Card(4), new Card(5), new Card(6), new Card(7), new Card(8), new Card(9), new Card(10), new Card(11), new Card(12)};
        this.rowsToHide =rowsToHide;
        this.hideTopRows = hideTopRows;
        matchThreeMode=false;
        hasGlass=false;
    }

    public void SetPossibleCards(List<int> customPossibleCards){
        Card[] newArr = new Card[customPossibleCards.Count];
        for(int i=0; i<customPossibleCards.Count; i++){
            newArr[i]=new Card(customPossibleCards[i]);
        }
        SetPossibleCards(newArr);
    }
    public virtual void SetPossibleCards(Card[] possibleCards){
        this.possibleCards=possibleCards;
    }
    public Model(int row, int col, int rowsToHide, bool hideTopRows, bool matchThreeMode) : this(row, col, rowsToHide, hideTopRows){
        this.matchThreeMode=matchThreeMode;
    }

//glass crap
    protected bool[,] glassMatrix;
    private float glassRarity;
    protected int glassCount;    
    public void AddGlass(float glassRarity){
        hasGlass=true;
        this.glassRarity =glassRarity;
        glassCount=0;
        CreateWallMatrix();        
    }
    

    public void AddGlass(float glassRarity, bool[,] customGlassMatrix){
        hasGlass=true;
        this.glassRarity =glassRarity;
        glassMatrix= customGlassMatrix;
        FixGlassMatrix();
        CountGlass(); // set glasscount
    }
    public bool GetGlassAtIndex(int row, int col){
        return glassMatrix[row, col];
    }
    private void FixGlassMatrix(){
        for(int i=0; i< getRowsToHide(); i++){
            for(int j=0; j< col; j++){
                glassMatrix[i,j]=false;
            }
        }
    }
    private void CountGlass(){
        for(int i=0; i< glassMatrix.GetLength(0); i++){
            for(int j=0; j<glassMatrix.GetLength(1); j++){
                if(glassMatrix[i,j]){
                    glassCount++;
                }
            }
        }
    }

    private void CreateWallMatrix(){
        Debug.Log("ROWS TO HIDE " + getRowsToHide());
        glassMatrix = new bool[row,col];

        for (int i = getRowsToHide(); i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                // Generate a random number between 0 and 99 (inclusive)
                float randomNumber = UnityEngine.Random.value;
                // Set the cell to true with a 10% chance
                if (randomNumber < glassRarity) {
                    glassMatrix[i, j] = true; 
                    glassRarity++;
                }else{
                    glassMatrix[i, j] = false;
                }
            
            }
        }
    }

    public void breakGlass(int row, int col){
        if(hasGlass){
            if(glassMatrix[row, col]){
                glassMatrix[row, col] =false;
            }
            PrintArray(glassMatrix);
        }
    }
//glass crap done
    public virtual void RemoveGridObject(int row, int col){
        cardGrid[row,col]=null;
        TranslateDown(row, col);
    }

    protected virtual GridObject CreateNewGridObject(int row, int col){
        //Debug.Log("Hello from WallModel CreateNewGridObject");
        cardGrid[row,col] = new Card(UnityEngine.Random.Range(0, possibleCards.Length), row, col);
        return cardGrid[row,col];
    }

    public GridObject getObjectAtIndex(int row, int col){
        return cardGrid[row,col];
    }    
    public void PopulateGrid(){
        for(int i=cardGrid.GetLength(0)-1; i>=0; i--){
            for(int j=cardGrid.GetLength(1)-1; j>=0; j--){
                cardGrid[i,j]=CreateNewGridObject(i,j);
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
                (cardGrid[row,col]).IncreaseCellsToFall();
            }
            TranslateDown(row-1, col);
        }
    }
    public void  PrintArray<T>(T[,] array){
        string myString = "";
        for (int i = 0; i < array.GetLength(0); i++){
            for (int j = 0; j < array.GetLength(1); j++){
                if (array[i, j] == null){
                    myString += "array[" + i + "," + j + "] = null";
                }
                else{
                    myString += "array[" + i + "," + j + "] = " + array[i, j].ToString();
                }
                myString += "\t"; // Adding tab for better readability
            }
            myString += "\n"; // New line after each row
        }
        Debug.Log(myString);
    }
}

public class OriginalModel :Model {
    //base values
    static bool hideTopRows=true;
    static int rowsToHide =2;
    public OriginalModel(int row, int col) : base(Mathf.Max(4, row), col, rowsToHide, hideTopRows){
        InitializeOriginalModel();
    }
    public OriginalModel(int row, int col, bool matchThreeMode) : base(Mathf.Max(4, row), col, rowsToHide, hideTopRows, matchThreeMode){
        InitializeOriginalModel();
    }

    private void InitializeOriginalModel(){
        possibleCards= new Card[] {new Card(0), new Card(1), new Card(2), new Card(3),new Card(4), new Card(5)};
    }
    public override void RemoveGridObject(int row, int col){
        base.RemoveGridObject(row, col);
        Card newCard= (Card)CreateNewGridObject(0,col);
        newCard.IncreaseCellsToFall();
    }

}


public class EliminationModel : Model{
    
    static bool hideTopRows=false;
    static int rowsToHide =0;
    private Dictionary<Card, int> cardUsgae;
    public EliminationModel(int row, int col) : base(row, col, rowsToHide , hideTopRows){
        InitializeEliminationModel();
    }

    public EliminationModel(int row, int col, bool matchThreeMode) : base(row, col, rowsToHide, hideTopRows, matchThreeMode){ 
        InitializeEliminationModel();
        }

    private void InitializeEliminationModel(){
        cardUsgae = new Dictionary<Card, int>();
        ContructDictionary();
    }

    public override void SetPossibleCards(Card[] possibleCards){
        cardUsgae = new Dictionary<Card, int>();
        ContructDictionary();
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


public abstract class WallModel : Model{
    protected bool[,] wallMatrix;
    static bool hideTopRows=true;
    static int rowsToHide =4;
    static float wallRarity =0.25f;
    protected int wallCount;

    public int GetWallCount(){
        return wallCount;
    }
    private void FixWallMatrix(){
        for(int i=0; i< getRowsToHide(); i++){
            for(int j=0; j< col; j++){
                wallMatrix[i,j]=false;
            }
        }
    }
    public WallModel(int row, int col, bool[,] customWallMatrix) : base(row,col, rowsToHide, hideTopRows){
        wallMatrix= customWallMatrix;
        FixWallMatrix();
        CountWalls(); // set wallCount
    }
    public WallModel(int row, int col, bool matchThreeMode, bool[,] customWallMatrix) : base(row,col, rowsToHide,hideTopRows, matchThreeMode){
        wallMatrix =customWallMatrix;
        FixWallMatrix();
        CountWalls(); // set wallCount
    }
    public WallModel(int row, int col) : base(row,col, rowsToHide,hideTopRows){
        wallCount=0;
        CreateWallMatrix();
    }

    public WallModel(int row, int col, bool matchThreeMode) : base(row,col, rowsToHide,hideTopRows, matchThreeMode){
        wallCount=0;
        CreateWallMatrix();
    }



    protected GridObject CallBaseCreateGridObject(int row, int col){
        return base.CreateNewGridObject(row, col);
    }
    private void CountWalls(){
        for(int i=0; i< wallMatrix.GetLength(0); i++){
            for(int j=0; j<wallMatrix.GetLength(1); j++){
                if(wallMatrix[i,j]){
                    wallCount++;
                }
            }
        }
    }

    private void CreateWallMatrix(){
        Debug.Log("ROWS TO HIDE " + getRowsToHide());
        wallMatrix = new bool[row,col];


        for (int i = getRowsToHide(); i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                // Generate a random number between 0 and 99 (inclusive)
                float randomNumber = UnityEngine.Random.value;
                // Set the cell to true with a 10% chance
                if (randomNumber < wallRarity) {
                    wallMatrix[i, j] = true; 
                    wallCount++;
                }else{
                    wallMatrix[i, j] = false;
                }
            
            }
        }
    }

    protected override GridObject CreateNewGridObject(int row, int col)
    {
        if(wallMatrix[row,col] ){
            wallMatrix[row, col]=false;
            return new Wall(row, col);
        }else{
            float randomNumber = UnityEngine.Random.value;
                if (randomNumber < wallRarity) {
                    cardGrid[row,col]= new Wall(row, col);
                    return cardGrid[row,col];
                }
        }
        return base.CreateNewGridObject(row, col);
    }

    public void RemoveWalls(){
        foreach(Wall w in wallstoDestroy){
            RemoveGridObject(w.getRowPos(), w.getColPos());
        }
    }

    HashSet<Wall> wallstoDestroy= new HashSet<Wall>();

    public List<int[]> CalculateWallsToDestroy(int row1, int col1, int row2, int col2){
        //graph theory wouldve helped lmao
        wallstoDestroy= new HashSet<Wall>();

        List<int[]> returnList = new List<int[]>();

        TempFunction(row1, col1, returnList);
        TempFunction(row2, col2, returnList);

        return returnList;
    }
    //match 3
    public List<int[]> CalculateWallsToDestroy(int row1, int col1, int row2, int col2, int row3, int col3){
        List<int[]> returnList = CalculateWallsToDestroy(row1, col1, row2, col2);
        TempFunction(row3, col3, returnList);
        return returnList;
    }
    private void TempFunction(int row, int col, List<int[]> arr){
        Debug.Log("checking");
        if(row> 0){
            if( cardGrid[row-1, col] != null && cardGrid[row-1, col] is Wall){
                if (wallstoDestroy.Add((Wall)getObjectAtIndex(row-1, col))){
                    arr.Add( new int[] {row-1, col});   
                }
            }
        }
        if(row<cardGrid.GetLength(0)-1){
            if(cardGrid[row+1, col] != null && cardGrid[row+1, col] is Wall){
                if(wallstoDestroy.Add((Wall)getObjectAtIndex(row+1, col))){
                    arr.Add( new int[] {row+1, col}); 
                }
            }
        }
        if(col> 0){
            if(cardGrid[row, col-1] != null && cardGrid[row, col-1] is Wall){
                if(wallstoDestroy.Add((Wall)getObjectAtIndex(row, col-1))){
                    arr.Add(new int[] {row, col-1}); 
                }
            }
        }
        if(col<cardGrid.GetLength(1)-1){
            if(cardGrid[row, col+1] != null && cardGrid[row, col+1] is Wall){
                if(wallstoDestroy.Add((Wall)getObjectAtIndex(row, col+1))){
                    arr.Add(new int[] {row, col+1}); 
                }
            }
        }

    } 

}

public class WallModelElimination : WallModel{
    
    private Dictionary<Card, int> cardUsgae;

    public WallModelElimination(int row, int col) : base(row,col){
        cardUsgae = new Dictionary<Card, int>();
        ContructDictionary();
    }
    public WallModelElimination(int row, int col, bool matchThreeMode) : base(row,col, matchThreeMode){
        cardUsgae = new Dictionary<Card, int>();
        ContructDictionary();
    }

    public WallModelElimination(int row, int col, bool[,] customWallMatrix) : base(row,col, customWallMatrix){
        cardUsgae = new Dictionary<Card, int>();
        ContructDictionary();
    }
    public WallModelElimination(int row, int col, bool matchThreeMode, bool[,] customWallMatrix) : base(row,col, matchThreeMode, customWallMatrix){
        cardUsgae = new Dictionary<Card, int>();
        ContructDictionary();

    }

    public override void SetPossibleCards(Card[] possibleCards){
        this.possibleCards=possibleCards;
    }
    private void ContructDictionary(){
        //this code can be better
            int totalCards = getCol()* getRow();
            int setsSize = this.isMatchThreeMode()==true ? 3 : 2;
            totalCards-=wallCount;
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

    protected override GridObject CreateNewGridObject(int row, int col)
    {
        
        if(wallMatrix[row,col] ){
            wallMatrix[row, col]=false;
            return new Wall(row, col);
        }
        return CreateNewCardFromDict(row, col);
    }
    private Card CreateNewCardFromDict(int row, int col){
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

public class WallModelOriginal : WallModel{
    public WallModelOriginal(int row, int col) : base(row,col){
    }

    public WallModelOriginal(int row, int col, bool matchThreeMode) : base(row,col, matchThreeMode){
    }

    public WallModelOriginal(int row, int col, bool[,] customWallMatrix) : base(row,col, customWallMatrix){
    }

    public WallModelOriginal(int row, int col, bool matchThreeMode, bool[,] customWallMatrix) : base(row,col, matchThreeMode, customWallMatrix){
    }
    public override void RemoveGridObject(int row, int col){
        if(isHasGlass()){
            if(glassMatrix[row, col]){
                glassMatrix[row, col] =false;
            }
        }
        cardGrid[row,col]=null;
        TranslateDown(row, col);        
        GridObject newGridObject= CreateNewGridObject(0,col);
        newGridObject.IncreaseCellsToFall();
        //PrintArray();
    }

}


public class WallModelDestroyWalls : WallModel{
    public WallModelDestroyWalls(int row, int col) : base(row,col){
    }

    public WallModelDestroyWalls(int row, int col, bool matchThreeMode) : base(row,col, matchThreeMode){
    }

    public WallModelDestroyWalls(int row, int col, bool[,] customWallMatrix) : base(row,col, customWallMatrix){
    }

    public WallModelDestroyWalls(int row, int col, bool matchThreeMode, bool[,] customWallMatrix) : base(row,col, matchThreeMode, customWallMatrix){
    }
    

    protected override GridObject CreateNewGridObject(int row, int col){
        if(wallMatrix[row,col]){
            wallMatrix[row, col]=false;
            return new Wall(row, col);
        }
        return base.CallBaseCreateGridObject(row, col);
    }

    public override void RemoveGridObject(int row, int col){
        base.RemoveGridObject(row, col);
        Card newGridObject= (Card)base.CallBaseCreateGridObject(0,col);
        newGridObject.IncreaseCellsToFall();
    
    }
}
