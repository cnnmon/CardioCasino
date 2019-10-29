using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCoin : MonoBehaviour
{
    private bool overCoin = false;
    private bool triggeredEnd = false;

    private void OnMouseEnter()
    {
        LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.3f).setEaseInOutQuad();
        overCoin = true;
    }

    private void OnMouseExit()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.2f).setEaseInOutQuad();
        overCoin = false;
    }

    private void Update()
    {
        if(overCoin && Input.GetMouseButtonDown(0) && !triggeredEnd)
        {
            triggeredEnd = true;
            SlotManager.SM.TriggerEndScreen();
        }
    }
}
