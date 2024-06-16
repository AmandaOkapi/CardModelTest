using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class LuckyMatchAppearance : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float fadeSpeed; 
    [SerializeField] private float startingAlpha; 

    [SerializeField] private string word= "Nice";
    
    [SerializeField] private string[] words;

    private float MIN_DELTA_TIME = 0.01f; 
    private TextMeshProUGUI[] textMeshProUGUIComponents;

    private float scaleFactor;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello from the lucky appearance script" + transform.position + " " + transform.localPosition);
        //transform.position = new Vector3(transform.position.x + View.cardSize.x+ offset.x, transform.position.y + offset.y, transform.position.z);
        transform.localPosition = new Vector3(transform.localPosition.x + (View.cardSize.x+ offset.x)*transform.localScale.x, 
                                                transform.localPosition.y + (View.cardSize.y+ offset.y)*transform.localScale.y, transform.localPosition.z);
        textMeshProUGUIComponents = GetComponentsInChildren<TextMeshProUGUI>();
        string chosenWord = words[UnityEngine.Random.Range(0,words.Length)];
        foreach (TextMeshProUGUI tmpUGUI in textMeshProUGUIComponents){
            tmpUGUI.text = chosenWord; 
        }
        StartCoroutine(Decay());
    }

    private IEnumerator Decay(){
        float alpha = startingAlpha;
        float grv=2f;
        while(alpha>0.1){
            float deltaTime = Mathf.Max(Time.deltaTime, MIN_DELTA_TIME);
            transform.localPosition -= new Vector3(0, fallSpeed*deltaTime*grv, 0);
            alpha -= fadeSpeed * deltaTime;
            foreach (TextMeshProUGUI tmpUGUI in textMeshProUGUIComponents){
                Color color = tmpUGUI.color;
                color.a = alpha;
                tmpUGUI.color = color;
            }
            yield return null;
        }
        Debug.Log("Goodbye from lucky appearance script");
        Destroy(gameObject);

    }
}
