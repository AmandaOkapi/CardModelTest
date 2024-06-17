using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "RevealRow", menuName = "PowerUps/RevealRow")]
public class RevealRow : PowerUp
{

    public override void Activate(View view, Model model, float delay)
    {
        isPlaying=true;
        int row = Random.Range(model.getRowsToHide(), model.getRow());
        view.RevealRow(row, delay);
    }
}