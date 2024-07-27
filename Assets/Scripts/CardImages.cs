using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Custom/Sprite List", order = 1)]
public class CardData : ScriptableObject
{
    public Sprite[] cardImages;
    public Sprite cardBack;

    public Sprite mysteryCardIcon;

}
