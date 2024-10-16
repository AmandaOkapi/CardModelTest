using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectMono : MonoBehaviour
{
    private IGridObjectAppearance gridObjectAppearance;

    public static float fallSpeed =500f;
    public static float defaultFallspeed =500f;
    public static float frenzyFallspeed =1000f;
    public static float skipForwardFallspeed =800f;

    [SerializeField] private GridObject gridObjectBase;
    public void SetCardBase(GridObject gridObjectBase){ this.gridObjectBase=gridObjectBase;}

    public GridObject getCardBase(){return gridObjectBase;}
    private void Start(){
        //fallSpeed = defaultFallspeed;
        gridObjectAppearance = GetComponent<IGridObjectAppearance>();
    }

    public void FallToPos(UnityEngine.Vector3 target){
        View.cardsFalling++;
        StartCoroutine(TranslateConstantSpeed(target));
    }
    public void FallToPosPopIn(UnityEngine.Vector3 target){
        View.cardsFalling++;
        StartCoroutine(TranslateOverTime(target, 0.3f));
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
        View.cardsFalling--;
    }           


    IEnumerator TranslateConstantSpeed(UnityEngine.Vector3 target){
        while(UnityEngine.Vector3.Distance(transform.localPosition, target) > 0.002f){
            transform.localPosition =UnityEngine.Vector3.MoveTowards(transform.localPosition, target, fallSpeed * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = target;
        View.cardsFalling--;
    }

    public void Die(){
        gridObjectAppearance.Die();
    }



}
