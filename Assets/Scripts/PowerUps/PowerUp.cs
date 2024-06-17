using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PowerUp :ScriptableObject
{
    //make sure your water is boiled cuz we be cooking some spaghetti
    public Sprite icon;
    public bool isPlaying;
    public abstract void Activate(View view, Model model, float delay);
    public void Deactivate(){
        isPlaying=false;
    }
}
