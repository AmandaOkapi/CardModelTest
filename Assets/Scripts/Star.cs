using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    public float speed = 5f; // Speed of the movement

    private RectTransform rectTransform;

    void Start()
    {
        // Get the RectTransform component of the UI object
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Calculate the new position
        Vector3 newPosition = rectTransform.anchoredPosition + new Vector2(-1, -1) * speed * Time.deltaTime;

        // Update the position
        rectTransform.anchoredPosition = newPosition;

        if((newPosition.x <= -Screen.width-100f) || (-newPosition.y <= -Screen.height -100f)){
            Destroy(gameObject);
        } 
    }

    private void OnCollisionEnter2D(Collision2D col){
        Destroy(col.gameObject);
    }
}
