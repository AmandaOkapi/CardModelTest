using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSearchToolkit 
{
    //finding valid rows for powerUps
    public static int FindRowWithCards(Model model){
        //randomly pick a row and search upwards. upon reaching the top start at the bottom
        int row = Random.Range(model.getRowsToHide(), model.getRow());
        Debug.Log("performing a bottom up search starting at "+row);
        for(int i = row; i>= model.getRowsToHide(); i--){
            for(int j =0; j<model.getCol(); j++){
                if(model.getObjectAtIndex(i,j) is Card){
                    Debug.Log("valid row found " + i);
                    return i;
                }
            }
        }
        for(int i = model.getRow()-1; i>row; i--){
            for(int j =0; j<model.getCol(); j++){
                if(model.getObjectAtIndex(i,j) is Card){
                    Debug.Log("valid row found " + i);
                    return i;
                }
            }
        }
        Debug.Log("Row search failed");
        return model.getRow() -1;
    }
    public static int FindColWithCards(Model model){
        int col = Random.Range(0, model.getCol());
        Debug.Log("performing a right to left search starting at col "+col);
        //going right to left
        for(int i = col; i>= 0; i--){
            for(int j =0; j<model.getRow(); j++){
                if(model.getObjectAtIndex(i,j) is Card){
                    Debug.Log("valid col found " + i);
                    return i;
                }
            }
        }
        for(int i = model.getCol()-1; i>col; i--){
            for(int j =0; j<model.getRow(); j++){
                if(model.getObjectAtIndex(i,j) is Card){
                    Debug.Log("valid col found " + i);
                    return i;
                }
            }
        }
        Debug.Log("Col search failed");
        return model.getCol() -1;
    }
}
