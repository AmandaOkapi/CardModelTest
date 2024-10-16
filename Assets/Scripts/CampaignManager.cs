using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 
public class CampaignManager : MonoBehaviour
{
    [SerializeField] Transform panel;
    // Start is called before the first frame update
    void Start()
    {
        int i =0;
        Transform[] children = panel.GetComponentsInChildren<Transform>();

        foreach(Transform child in children){
            if(child.name =="Arrows"){
                break;
            }

            if(i==0){
                i++;
                continue;
            }

            Debug.Log(child.name);
            if (child.name.StartsWith("LevelButton")){
                TextMeshProUGUI  text = child.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
                text.text = i.ToString();
                i++;
            }
        }
    }

}
