using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "Frenzy", menuName = "PowerUps/Frenzy")]
public class Frenzy : PowerUp
{
    public override void Activate(View view, Model model, float delay)
    {
        isPlaying=true;
        view.RevealAll(delay);
        Debug.Log("lol");
    }

}
