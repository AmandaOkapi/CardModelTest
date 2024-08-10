using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTextAppearance : LuckyMatchAppearance
{
    public static int comboTextValue =1;

    protected override string DisplayString(){
        Debug.Log("hello from combo text");
        return comboTextValue +"x";
    }
}
