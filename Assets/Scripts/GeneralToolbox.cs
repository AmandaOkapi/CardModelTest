using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralToolbox 
{
        // Static function to print a list to the console
    public static void PrintListToConsole<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.Log("The list is empty or null.");
            return;
        }

        foreach (var item in list)
        {
            Debug.Log(item);
        }
    }
}
