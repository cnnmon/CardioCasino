using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinBehavior : MonoBehaviour
{
    private Coin coin;
    private Image img;
    public Sprite coinSprite;

    private void Start()
    {
        coin = GetComponent<Coin>();
        img = GetComponent<Image>();
    }

    public void ChangeCoin()
    {
        coin.isShifting = true;
        LeanTween.alphaCanvas(coin.cg, 0, 0.6f).setEaseInOutQuad();
        LeanTween.scale(gameObject, coin.biggestScale, 0.6f).setEaseInOutQuad().setOnComplete(() => {
            transform.localScale = coin.smolScale;
            img.sprite = coinSprite;
            LeanTween.alphaCanvas(coin.cg, 0.6f, 0.5f).setEaseInOutQuad();
            LeanTween.scale(gameObject, coin.regScale, 0.5f).setEaseInOutQuad().setOnComplete(() => { coin.isShifting = false; });
        });
    }
}
