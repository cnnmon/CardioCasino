using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    private bool overSlot;

    private void Update()
    {
        if (overSlot && Input.GetMouseButtonDown(0)) SlotManager.SM.CoinIn();
    }

    private void OnMouseEnter()
    {
        SlotManager.SM.CoinOver();
        overSlot = true;
    }

    private void OnMouseExit()
    {
        SlotManager.SM.CoinExit();
        overSlot = false;
    }
}
