using System.Collections;
using System.Collections.Generic;
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


}
