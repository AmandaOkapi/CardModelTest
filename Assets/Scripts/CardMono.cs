using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;


public class CardMono : MonoBehaviour, IGridObjectAppearance
{
    [SerializeField] private bool debugMode;
    [SerializeField] private Animator animator;

    public CardData cardData;

    [SerializeField] private Button buttonComponent;

    [SerializeField] public GridObjectMono gridObjectMono;

    
    [Header ("View Related")]
    [SerializeField] public UnityEngine.UI.Image imageComponent;



    private Controller controllerLink;

    void Start()
    {
        controllerLink=GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();        
        if (cardData != null)
            {
            imageComponent.sprite = cardData.cardBack;
        }else{
            Debug.LogError("cardData is null! ");
        }

    }

    public void flipCard(){
        controllerLink.flipCard(this);
        animator.SetTrigger("Flip");
    }

    public void SetEnabled(bool x){
        buttonComponent.enabled=x;
    }

    // Update is called once per frame
    void Update()
    {
        if(debugMode){
            imageComponent.sprite = cardData.cardImages[((Card)(gridObjectMono).getCardBase()).getId()];
        }
    }

    public void CreateDownWardsAnimation(float yPos){
        // Create animation curves

    }

    public void Die(){
        Destroy(gameObject);
    }

}
