using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float animTime;

    public static GameObject currentCanvas;
    public GameObject myCanvas;

    public void DisableMyCanvas(){
        if(myCanvas!=null){
            myCanvas.SetActive(false);
        }else if(currentCanvas!=null){
            currentCanvas.SetActive(false);
        }
    }
    public void TurnonNewCanvas(GameObject canvas){
        canvas.SetActive(true);
        currentCanvas = canvas;
    }

    public void DisableCanvas(GameObject canvas){
        canvas.SetActive(false);
    }

    public void PlayTransitionAnimation(GameObject canvas){
        StartCoroutine(TransitionAnimation(canvas));
    }

    public void PlayAnimation(){
        animator.SetTrigger("CurtainDown");
    }

    public IEnumerator TransitionAnimation(GameObject canvas){
        PlayAnimation();
        yield return new WaitForSeconds(animTime);
        
        DisableMyCanvas();
        TurnonNewCanvas(canvas);
    }
}
