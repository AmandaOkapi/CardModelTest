using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

    [SerializeField] public GameObject frenzy;


    [Header ("Card Appearance Related")]
    public static float Time_To_Flip;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Color defaultDesignCol, defaultBackCol;       
    private Color designCol;
    private Color backCol;
    private Controller controllerLink;

    void Start()
    {
        SetUpColours();
        controllerLink=GameObject.FindGameObjectWithTag("Controller").GetComponent<Controller>();        
        if (cardData != null)
            {
            imageComponent.sprite = cardData.cardBack;
            backCol.a =1f;
            imageComponent.color = backCol;
        }else{
            Debug.LogError("cardData is null! ");
        }

    }

    private void SetUpColours(){
        if (PlayerPrefs.HasKey("BackCol_r") && PlayerPrefs.HasKey("BackCol_g") && PlayerPrefs.HasKey("BackCol_b") && PlayerPrefs.HasKey("BackCol_a"))
        {
            backCol = PlayerPrefsUtility.LoadColor("BackCol", Color.white);
        }
        else
        {
            // Convert hex string to Color
            backCol = defaultBackCol;
        }
        if (PlayerPrefs.HasKey("DesignCol_r") && PlayerPrefs.HasKey("DesignCol_g") && PlayerPrefs.HasKey("DesignCol_b") && PlayerPrefs.HasKey("DesignCol_a"))
        {
            designCol = PlayerPrefsUtility.LoadColor("DesignCol", Color.white);
        }
        else
        {
            // Convert hex string to Color
            backCol = defaultDesignCol;
        }

    }

    public void flipCard(){        
        controllerLink.flipCard(this);
    }
    // public void flipCard(int rowPos, int colPos){        
    //     controllerLink.flipCard(rowPos, colPos);
    // }
    public void ShowflipCard(bool playSound){    
        StartCoroutine(changeCardImage(cardData.cardImages[((Card)gridObjectMono.getCardBase()).getId()], Color.white));    
        AnimFlipCard();
        if(playSound){
            PlayFlip();
        }
    }

    public void ShowflipCard(){
        ShowflipCard(true);
    }

    public void ShowUnflipCard(bool playSound){    
        StartCoroutine(changeCardImage(cardData.cardBack, backCol));    
        AnimUnflipCard();
        if(playSound){
            PlayFlip();
        }    
    }
    private IEnumerator changeCardImage(Sprite image, Color col ){
        float time = 0f;

        while (time < Time_To_Flip)
        {
            time += Time.deltaTime;
            yield return null;
        }
        imageComponent.sprite= image;        
        imageComponent.color= col;        

    }

    
    //Animations
    public void AnimFlipCard(){animator.SetTrigger("Flip");}

    public void AnimReflipCard(){animator.SetTrigger("Reflip");}
    public void AnimUnflipCard(){animator.SetTrigger("Unflip");}

    //Sounds /Audio

    public void PlayFlip(){
        audioSource.Play();
    } 
    public void SetEnabled(bool x){
        buttonComponent.enabled=x;
    }

    public void ShowFrenzy(bool x){
        frenzy.SetActive(x);
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
