using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectMono : MonoBehaviour
{
    [SerializeField] private float fallSpeed;

    [SerializeField] private GridObject gridObjectBase;
    public void setCardBase(GridObject gridObjectBase){ this.gridObjectBase=gridObjectBase;}
    public GridObject getCardBase(){return gridObjectBase;}
    private void Start(){
        if(fallSpeed==0){
            //fallSpeed = GridObject.fallSpeed;
            fallSpeed=500;
        }
    }

    public void FallToPos(UnityEngine.Vector3 target){
        StartCoroutine(TranslateConstantSpeed(target, fallSpeed));
    }


    IEnumerator TranslateOverTime(UnityEngine.Vector3 target, float time) //0 references
    {
        UnityEngine.Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            //Debug.Log("Hello");
            transform.localPosition = UnityEngine.Vector3.Lerp(startPosition, target, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure exact positioning at the end
        transform.localPosition = target;
    }           


    IEnumerator TranslateConstantSpeed(UnityEngine.Vector3 target, float speed){
        while(UnityEngine.Vector3.Distance(transform.localPosition, target) > 0.001f){
            transform.localPosition =UnityEngine.Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = target;

    }
}
