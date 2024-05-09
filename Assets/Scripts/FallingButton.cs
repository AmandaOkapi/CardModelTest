using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingButton : MonoBehaviour
{
    [SerializeField] private float fallSpeed;

    // Start is called before the first frame update
    public void FallToPos(UnityEngine.Vector3 target){
        StartCoroutine(TranslateOverTime(target, fallSpeed));
    }


    IEnumerator TranslateOverTime(UnityEngine.Vector3 target, float time)
    {
        UnityEngine.Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            Debug.Log("Hello");
            transform.localPosition = UnityEngine.Vector3.Lerp(startPosition, target, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure exact positioning at the end
        Destroy(gameObject);
    }
}
