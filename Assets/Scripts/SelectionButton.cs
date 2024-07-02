using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    [SerializeField] private bool status;
    [SerializeField] private Transform parent;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    private Image[] siblingImages;

    public void SetStatus(bool x){status=x;}
    void Start()
    {
        if(parent ==null){
            parent = gameObject.transform.parent.transform;
        }
        if(onSprite ==null){
            if(GameObject.Find("UiImages")){
                onSprite = GameObject.Find("UiImages").GetComponent<UiImages>().onSprite;   
            }
        }
        if(offSprite ==null){
            if(GameObject.Find("UiImages")){
                offSprite = GameObject.Find("UiImages").GetComponent<UiImages>().offSprite;   
            }
        }            
        siblingImages = new Image[parent.childCount - 1];
        int index=0;
        foreach (Transform sibling in parent)
        {
            // If the sibling is a button and not the current button
            if (sibling != transform && sibling.GetComponent<Button>() != null)
            {
                Image siblingImage = sibling.GetComponent<Image>();
                if (siblingImage != null)
                {
                    siblingImages[index++]=siblingImage;
                }
            }
        }
        if(status){
            gameObject.GetComponent<Image>().sprite =onSprite;
        }else{
            gameObject.GetComponent<Image>().sprite =offSprite;
        }
    }

    public void TurnOnButton(){
        gameObject.GetComponent<Image>().sprite =onSprite;
        foreach(Image image in siblingImages){
            image.sprite=offSprite;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
