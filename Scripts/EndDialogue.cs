using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndDialogue : MonoBehaviour
{
    public string[] dialogue;
    public TextMeshProUGUI text;
    private int arrayNum = 0;
    public Button slot;
    public GameObject shadow;

    private void Start()
    {
        ClickNext();
    }

    public void ClickNext()
    {
        if(arrayNum < dialogue.Length)
        {
            text.text = dialogue[arrayNum];
            arrayNum++;
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
            slot.interactable = true;
        }

    }
}
