using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class IconTextPrefab : MonoBehaviour, ITextPrefab
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI myText;
    [SerializeField] private GameObject goalMetObject;


    public TextMeshProUGUI GetText(){
        return myText;
    }
    public void GoalMet(){
        myText.enabled = false;
        goalMetObject.SetActive(true);
    }
}
