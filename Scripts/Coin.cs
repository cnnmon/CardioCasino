using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Coin : EventTrigger
{
    public bool isShifting = false;

    private Color initColor;
    public CanvasGroup cg;
    private CoinBehavior coinBehavior;
    private bool selected = false;

    public readonly Vector3 smolScale = new Vector3(0.15f, 0.15f, 0.15f);
    public readonly Vector3 regScale = new Vector3(0.25f, 0.25f, 0.25f);
    public readonly Vector3 biggerScale = new Vector3(0.3f, 0.3f, 0.3f);
    public readonly Vector3 biggestScale = new Vector3(0.4f, 0.4f, 0.4f);

    private void Start()
    {
        coinBehavior = GetComponent<CoinBehavior>();
        initColor = GetComponent<Image>().color;
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0.8f;
    }

    public void Update()
    {
        if (selected) transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public override void OnPointerDown(PointerEventData eventData) {

        if (!selected)
        {
            selected = true;

            coinBehavior.ChangeCoin();
            SlotManager.SM.FadeSolidColor();

            name = "Coin0"; //disables hover script
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.white;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = initColor;
    }

    public void GrowCoin()
    {
        if (selected && transform.localScale != biggerScale && !isShifting)
        {
            cg.alpha = 1f;
            LeanTween.scale(gameObject, biggerScale, 0.1f).setEaseInOutQuad();
        }
    }

    public void ShrinkCoin()
    {
        if (selected && transform.localScale != regScale && !isShifting)
        {
            cg.alpha = 0.6f;
            LeanTween.scale(gameObject, regScale, 0.1f).setEaseInOutQuad();
        }
    }

    //public override void OnPointerUp(PointerEventData eventData) { selected = false; }
}