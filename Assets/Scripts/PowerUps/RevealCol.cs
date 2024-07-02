using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "RevealCol", menuName = "PowerUps/RevealCol")]
public class RevealCol : PowerUp
{
    public override void Activate(View view, Model model, float delay)
    {
        isPlaying=true;
        int col = ModelSearchToolkit.FindColWithCards(model);
        view.RevealCol(col, model, delay);
    }
}