using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempSSpawn : MonoBehaviour
{
    public GameObject temp;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(temp); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
