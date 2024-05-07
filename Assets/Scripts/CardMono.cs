using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;


public class CardMono : MonoBehaviour
{
    public CardData cardData;

    [SerializeField] private Card cardBase;
    

    public Controller tempControllerLink;
    [Header ("View Related")]
    public UnityEngine.UI.Image imageComponent;
    [SerializeField] private float fallSpeed;

    public void setCardBase(Card cardBase){ this.cardBase=cardBase;}
    public Card getCardBase(){return cardBase;}


    void Start()
    {
        imageComponent = GetComponent<UnityEngine.UI.Image>();
        if(cardData !=null){
            imageComponent.sprite = cardData.cardBack;
        }
        tempControllerLink= GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();
    }

    public void flipCard(){        
        tempControllerLink.flipCard(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateDownWardsAnimation(float yPos){
        // Create animation curves

    }

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
        transform.localPosition = target;
    }           
}
