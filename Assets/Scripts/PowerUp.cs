using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PowerUp :ScriptableObject
{
    //make sure your water is boiled cuz we be cooking some spaghetti
    public Sprite icon;
    public abstract void Activate(View view, Model model);
    public PowerUp nextPowerUp;

    public void EnterNewPowerUp(View view){
        view.ResetPowerUpSlider(this);
    }
}

[System.Serializable]

[CreateAssetMenu(fileName = "RevealRow", menuName = "PowerUps/RevealRow")]
public class RevealRow : PowerUp
{

    public override void Activate(View view, Model model)
    {
        int row = Random.Range(model.getRowsToHide(), model.getRow());
        view.RevealRow(row);
    }
}

[System.Serializable]

[CreateAssetMenu(fileName = "RevealCol", menuName = "PowerUps/RevealCol")]
public class RevealCol : PowerUp
{
    public override void Activate(View view, Model model)
    {
        int col = Random.Range(0, model.getCol());
        view.RevealCol(col, model);
    }
}

[System.Serializable]

[CreateAssetMenu(fileName = "Frenzy", menuName = "PowerUps/Frenzy")]
public class Frenzy : PowerUp
{
    public override void Activate(View view, Model model)
    {
        view.RevealAll();
        Debug.Log("lol");
    }

}
