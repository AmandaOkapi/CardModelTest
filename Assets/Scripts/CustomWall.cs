using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomWallDataBase
{
    public static bool[,] customWall1 = {
    {false, false, false, false, false, false, false},
    {false, false, false, false, false, false, false},
    {false, false, false, false, false, false, false},
    {false, false, false, false, false, false, false},
    {false, false, false, false, false, false, false},
    {false, false, true, false, true, false, false},
    {false, false, true, false, true, false, false},
    {false, false, false, false, false, false, false},
    {false, false, false, false, false, false, false},
    {false, false, false, false, false, false, false},
    {false, true, false, false, false, true, false},
    {false, false, true, true, true, false, false},
    {false, false, false, false, false, false, false},
    {false, false, false, false, false, false, false},
    };
}

[CreateAssetMenu(fileName = "New CustomWall", menuName = "Custom/CustomWallScriptableObject", order = 1)]

public class CustomWallRows :ScriptableObject{

    public bool[,] combinedRows;
    public bool[] row0;
    public bool[] row1;
    public bool[] row2;
    public bool[] row3;
    public bool[] row4;
    public bool[] row5;
    public bool[] row6;
    public bool[] row7;
    public bool[] row8;
    public bool[] row9;
    public bool[] row10;
    public bool[] row11;
    public bool[] row12;
    public bool[] row13;
    public bool[] row14;
    public bool[] row15;
    public bool[] row16;
    public bool[] row17;
    public bool[] row18;
    public bool[] row19;
    public bool[] row20;
    public bool[] row21;
    public bool[] row22;
    public bool[] row23;
    public bool[] row24;
    public bool[] row25;
    public bool[] row26;
    public bool[] row27;
    public bool[] row28;
    public bool[] row29;

    public CustomWallRows(){
        
    }
}