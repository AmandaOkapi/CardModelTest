using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class LuckyMatchAppearance : MonoBehaviour
{
    //currently instatiated by the controller/view
    [SerializeField] private Vector2 offset = new Vector2(-10f, -36f);
    [SerializeField] private float fallSpeed =5;
    [SerializeField] private float fadeSpeed =1.5f; 
    [SerializeField] private float startingAlpha =2.5f; 

    [SerializeField] private string word= "Nice";
    
    [SerializeField] private string[] words;

    private float MIN_DELTA_TIME = 0.01f; 
    private TextMeshProUGUI[] textMeshProUGUIComponents;

    //controlled by the view script yolo
    public static float scaleFactor;
    [SerializeField] private float extraScaleFactor= 1.5f;
    private RectTransform rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello from the lucky appearance script" + transform.position + " " + transform.localPosition);
        //transform.position = new Vector3(transform.position.x + View.cardSize.x+ offset.x, transform.position.y + offset.y, transform.position.z);
        transform.localPosition = new Vector3(transform.localPosition.x + (View.cardSize.x+ offset.x)*transform.localScale.x, 
                                                transform.localPosition.y + (View.cardSize.y+ offset.y)*transform.localScale.y, transform.localPosition.z);
        textMeshProUGUIComponents = GetComponentsInChildren<TextMeshProUGUI>();
        string chosenWord =DisplayString();

        //sizing
        extraScaleFactor = Mathf.Max(extraScaleFactor, 1); //just in case
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new UnityEngine.Vector3(scaleFactor*extraScaleFactor, scaleFactor*extraScaleFactor, 1f);

        foreach (TextMeshProUGUI tmpUGUI in textMeshProUGUIComponents){
            tmpUGUI.text = chosenWord; 
        }

        fallSpeed*=scaleFactor;
        StartCoroutine(Decay());
    }

    protected virtual string DisplayString(){
        return words[UnityEngine.Random.Range(0,words.Length)];
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
