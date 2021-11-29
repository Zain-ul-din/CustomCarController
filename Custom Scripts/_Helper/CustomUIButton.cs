using UnityEngine;
using UnityEngine.EventSystems;


public class CustomUIButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler{

    public bool isPressing = false;
    
    public void OnPointerUp(PointerEventData e)
        => isPressing = false;

    public void OnPointerDown(PointerEventData e)
        => isPressing = true; 
    
}
