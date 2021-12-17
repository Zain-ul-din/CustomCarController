using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CustomUIButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler{

    public bool isPressing = false;
    private Sprite onUpSprite, onDownSprite;

    private Image img;
    private bool haveSprite;
    private void Start() {
        haveSprite = false;
        if (onUpSprite && onDownSprite) {
            img = GetComponent<Image>();
            haveSprite = true;
        }
    }

    public void OnPointerUp(PointerEventData e) {
        isPressing = false;
        if (haveSprite)
            img.sprite = onUpSprite;
    }


    public void OnPointerDown(PointerEventData e) {
        isPressing = true;
        if (haveSprite)
            img.sprite = onDownSprite;
    }

}
