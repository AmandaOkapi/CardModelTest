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
        int col = Random.Range(0, model.getCol());
        view.RevealCol(col, model, delay);
    }
}