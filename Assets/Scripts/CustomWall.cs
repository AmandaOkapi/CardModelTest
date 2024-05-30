using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CustomWall 
{
    public bool[][] data;


    public CustomWall(int row, int col){
        data = new bool[row][];
        for (int i = 0; i < row; i++)
        {
            data[i] = new bool[col];
        }
    }

    public bool[,] ConvertTo2DArray()
    {
        int rows = data.Length;
        int cols = data[0].Length;
        bool[,] array = new bool[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = data[i][j];
            }
        }
        return array;
    }
}


[System.Serializable]
public class ListWrapper
{
    public List<bool> myList;

    
}