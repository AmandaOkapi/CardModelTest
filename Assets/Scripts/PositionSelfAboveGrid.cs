using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSelfAboveGrid : MonoBehaviour
{
    [SerializeField] private GameObject grid; 
    [SerializeField] private float cardHeightPercent;
    // Start is called before the first frame update
    void Start()
    {
        if(grid==null){
            grid= GameObject.Find("GridPane");
        }
        
    }
}
