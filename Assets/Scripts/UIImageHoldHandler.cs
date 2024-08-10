using UnityEngine;
using UnityEngine.EventSystems;

public class UIImageHoldHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //private bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        //isHolding = true;
        GridObjectMono.fallSpeed = GridObjectMono.skipForwardFallspeed;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //isHolding = false;
        GridObjectMono.fallSpeed = GridObjectMono.defaultFallspeed;

    }

}