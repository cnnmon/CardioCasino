using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    public CanvasGroup blackCanvasGroup;
    public CanvasGroup infoCanvasGroup;

    private void Start()
    {
        LeanTween.alphaCanvas(blackCanvasGroup, 0, 1f).setOnComplete(() => { blackCanvasGroup.gameObject.SetActive(false); });
    }

    public void Info()
    {
        infoCanvasGroup.gameObject.SetActive(true);
        LeanTween.alphaCanvas(infoCanvasGroup, 1, 1f);
    }

    public void CloseInfo()
    {
        LeanTween.alphaCanvas(infoCanvasGroup, 0, 1f).setOnComplete(() => { infoCanvasGroup.gameObject.SetActive(false); });
    }

    public void Restart()
    {
        blackCanvasGroup.gameObject.SetActive(true);
        LeanTween.alphaCanvas(blackCanvasGroup, 1, 1f).setOnComplete(() => {
            SceneManager.LoadScene("Main");
        });
    }
}
