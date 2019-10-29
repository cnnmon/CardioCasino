using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using TMPro;

public class InkManager : MonoBehaviour
{
    public bool storyStart = false;
    public GameObject flashbackObject;
    private Story story;
    public Image img;

    //NARRATIVE
    public GameObject narrObject;
    public Button narrButton;
    public TextMeshProUGUI narrText;

    //OPTIONS
    public GameObject buttonObject;
    public Button leftButton;
    public Button rightButton;

    Flashback flashback;

    public void DisableSelf()
    {
        narrObject.SetActive(false);
    }

    public void CallStory()
    {
        flashback = SlotManager.SM.currentFlashback;
        LeanTween.scale(flashbackObject, Vector3.one, 2f).setEaseInOutQuad();
        StartStory();
    }

    void StartStory()
    {
        if (!storyStart)
        {
            InitDefaults();
            RefreshView();
        }
    }

    private void InitDefaults()
    {
        narrButton.interactable = true;
        storyStart = true;
        img.sprite = flashback.bgSprite;
        story = new Story(flashback.storyJSON.text);
        BindFunctions();
    }

    public void SwitchKnots(string name)
    {
        story.ChoosePathString(name);
        RefreshView();
    }

    private void BindFunctions()
    {
        story.BindExternalFunction("closeOut", () => {
            SlotManager.SM.CheckIfDoneAll();

            if (SlotManager.SM.shouldEnd)
            {
                narrButton.interactable = false;
                StartCoroutine(SlotManager.FadeOut(SlotManager.SM.audioSource, 0.8f));
                LeanTween.scale(flashbackObject, Vector3.zero, 2f).setEaseInOutQuad().setDelay(0.6f).setOnComplete(() => { SlotManager.SM.CallEnd(); });
            }
            else
            {
                LeanTween.scale(flashbackObject, Vector3.zero, 2f).setEaseInOutQuad().setDelay(0.6f);
                narrButton.interactable = false;
                StartCoroutine(SlotManager.FadeOut(SlotManager.SM.audioSource, 0.8f));
                SlotManager.SM.RefreshButton();
            }
        });
    }

    void RefreshView()
    {

        if (story.canContinue)
        {
            string text = story.Continue();
            text = text.Trim();
            CreateContentView(text);
        }

        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                Button button = CreateChoiceView(choice.text.Trim(), i);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate {
                    OnClickChoiceButton(choice);
                });
            }
        }
    }

    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }

    void CreateContentView(string text)
    {
        narrText.text = text;
        narrObject.SetActive(true);
    }

    Button CreateChoiceView(string text, int num)
    {
        Button choice = num == 0 ? leftButton : rightButton;
        TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
        choiceText.text = text;

        return choice;
    }
}