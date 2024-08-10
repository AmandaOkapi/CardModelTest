using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class FirstChildGetter : MonoBehaviour, ITextPrefab
{
    // Start is called before the first frame update
    public TextMeshProUGUI GetText(){
        return transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
}
