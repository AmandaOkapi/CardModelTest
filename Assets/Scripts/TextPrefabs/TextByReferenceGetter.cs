using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextByReferenceGetter : MonoBehaviour, ITextPrefab
{
    [SerializeField] private TextMeshProUGUI myText;
    [SerializeField] private GameObject goalMetObject;

    public TextMeshProUGUI GetText(){
        return myText;
    }
    public void GoalMet(){
        goalMetObject.SetActive(true);
    }
}
