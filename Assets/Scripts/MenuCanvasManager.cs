using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasManager : MonoBehaviour
{
    public GameObject myCanvas;

    public void DisableMyCanvas(){
        myCanvas.SetActive(false);
    }
    public void TurnonNewCanvas(GameObject canvas){
        canvas.SetActive(true);
    }

    public void DisableCanvas(GameObject canvas){
        canvas.SetActive(false);
    }
}
