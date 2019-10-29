using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndSlot : MonoBehaviour
{
    public CanvasGroup blackCanvasGroup;

    private void Start()
    {
        LeanTween.alphaCanvas(blackCanvasGroup, 0, 1f).setOnComplete(() => { blackCanvasGroup.gameObject.SetActive(false); });
    }

    public void CloseGame()
    {
        GetComponent<Button>().interactable = false;
        blackCanvasGroup.gameObject.SetActive(true);

        LeanTween.alphaCanvas(blackCanvasGroup, 1, 3f).setOnComplete(() => {
            ResetData();
            SceneManager.LoadScene("Main");
        });
    }

    public void ResetData() //where 1 means you got the flashback before and 0 means you haven't
    {
        for (int i = 0; i < 9; i++) PlayerPrefs.SetInt(i.ToString(), 0);
    }
}
