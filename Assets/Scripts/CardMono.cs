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

    public CardData cardData;

    [SerializeField] private Button buttonComponent;

    [SerializeField] public GridObjectMono gridObjectMono;

    
    [Header ("View Related")]
    [SerializeField] public UnityEngine.UI.Image imageComponent;

    [Header ("Card Appearance Related")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;


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
    }

    //Animations
    public void AnimFlipCard(){animator.SetTrigger("Flip");}

    public void AnimReflipCard(){animator.SetTrigger("Reflip");}
    public void PlayUnflipCard(){animator.SetTrigger("Unflip");}

    //Sounds /Audio

    public void PlayFlip(){
        audioSource.Play();
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
