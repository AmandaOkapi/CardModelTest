using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStars : MonoBehaviour
{
    [SerializeField] private GameObject star;

    [SerializeField] private Vector2 scaleRange;
    [SerializeField] private Vector2 offsetRange;

    [SerializeField] private Vector2 startingCardAmountRange;
    [SerializeField] private Vector2 newCardAmountRange;
    private Vector2 lastLocation;

    private float screenWidth, screenHeight;
    private void Start(){
        lastLocation = new Vector2(999,999);
        int index = (int)Random.Range(startingCardAmountRange.x, startingCardAmountRange.y);

        for(int i =0; i<index; i++){
            CreateNewCardStart();
        }
    }


    private void CreateNewCardStart(){
        GameObject newStar=Instantiate(star, transform);
        RectTransform starRT =  star.GetComponent<RectTransform>();
        starRT.anchoredPosition = RandomPositionOnScreen();
        starRT.localScale = RandomScale();
        Invoke("CreateNewCard", (int)Random.Range(newCardAmountRange.x, newCardAmountRange.y));
    }

    

    private void CreateNewCard(){
        GameObject newStar=Instantiate(star, transform);
        RectTransform starRT =  star.GetComponent<RectTransform>();
        starRT.anchoredPosition = NewCardPosition();
        starRT.localScale = RandomScale();
        Invoke("CreateNewCard", (int)Random.Range(newCardAmountRange.x, newCardAmountRange.y));

    }
    private Vector2 NewCardPosition(){
        Vector2 newVector = new Vector2( Random.Range(60, 120), Random.Range(-Screen.height, 100));
        while((Mathf.Abs(newVector.x - lastLocation.x) <offsetRange.x ) && (Mathf.Abs(newVector.y - lastLocation.y) <offsetRange.y ) ) {
            newVector = new Vector2( Random.Range(60, 120), Random.Range(-Screen.height, 300));
        }

        lastLocation = newVector; 
        return newVector;
    }
    private Vector2 RandomPositionOnScreen(){
        //random on screen new Vector2( Random.Range(-Screen.width, 10), Random.Range(-Screen.height, 10));
        Vector2 newVector = new Vector2( Random.Range(-Screen.width, 10), Random.Range(-Screen.height, 10));
        while((Mathf.Abs(newVector.x - lastLocation.x) <offsetRange.x ) && (Mathf.Abs(newVector.y - lastLocation.y) <offsetRange.y ) ) {
            newVector = new Vector2( Random.Range(-Screen.width, 10), Random.Range(-Screen.height, 10));
        }
        lastLocation = newVector; 
        return newVector;
    }

    private Vector3 RandomScale(){
        float val = Random.Range(scaleRange.x, scaleRange.y);
        return new Vector3( val, val, 1.0f);
    }
}
