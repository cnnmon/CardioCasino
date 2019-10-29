using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using UnityEngine.SceneManagement;

public class SlotManager : MonoBehaviour
{
    public List<int> flashbacksGotten;

    [Header("Slot Machine Functions")]
    public bool resultsGotten = false;
    public bool animDone = true;
    public SlotLine[] slotLines;

    public Transform overlapParent;
    public List<GameObject> overlaps;

    public Transform notifParent;
    public GameObject negNotifPrefab, posNotifPrefab;

    private List<string> negList = new List<string> { "House R", "Moving", "Death", "MentalHealth", "Money R" };
    public CanvasGroup solidColor;
    public GameObject slotCover;
    public Slider slider;
    public Button button;
    public GameObject coinLayer;
    public GameObject coin;
    public GameObject endCoin;
    public GameObject shinyCoinSlot;

    public bool solidFaded = false;
    public CanvasGroup blackCanvasGroup;

    public GameObject grayObj;
    public GameObject kid;

    public BoxCollider2D boxCollider; //just so hand mouse wont be activated when going over slot again?

    //scenes
    [Header("Scene Stuff")]
    public Flashback[] flashbacks;
    public GameObject[] flashbackObjs;
    public int[] cheatSpeeds; //when you're tired of randomizing and just wanna see everything :(
    public Flashback currentFlashback;
    public GameObject flashbackObj;

    [Header("Mouse Art")]
    public Texture2D defCursor;
    public Texture2D handCursor;

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioSource bgSource;

    public AudioClip heartbeat;
    public AudioClip coinIn;
    public AudioClip coinOut;
    public AudioClip reel;

    public AudioClip[] flashbackClips;

    //main
    public static SlotManager SM;
    private InkManager inkManager;

    private void Start()
    {
        InitDefaults();
        foreach (Transform child in overlapParent) overlaps.Add(child.gameObject);
        if (SM == null) SM = this;
    }

    public void FadeSolidColor()
    {
        StartCoroutine(FadeOut(audioSource, 0.8f));
        LeanTween.alphaCanvas(solidColor, 0, 1f).setOnComplete(() => { solidFaded = true; });
    }

    public void RefreshButton()
    {
        button.interactable = true;
    }

    public void CheckIfDoneAll()
        //checks if you've collected all the flashbacks @ beginning and @ the moment AFTER you get a flashback. If so, it sends you to the end
    {
        if(flashbacksGotten.Count >= flashbacks.Length)
        {
            FadeOut(audioSource, 1f);
            LeanTween.alphaCanvas(blackCanvasGroup, 1, 2f).setOnComplete(() => {
                SceneManager.LoadScene("End");
            });
        }
    }

    public int GetListPos(string name)
    {
        for (int i = 0; i < flashbacks.Length; i++) if (flashbacks[i].name == name) return i;
        return -1;
    }

    public void CallFlashback(int flashNum)
    {
        inkManager.storyStart = false;
        currentFlashback = flashbacks[flashNum];
        inkManager.CallStory();

        GameObject obj = flashbackObjs[flashNum];

        if (!obj.activeSelf)
        {
            PlayerPrefs.SetInt(flashNum.ToString(), 1);
            flashbacksGotten.Add(GetListPos(obj.name));
            obj.SetActive(true);
        }

        LeanTween.alpha(obj, 0.7f, 2f).setEaseInOutQuad();
        StartCoroutine(FadeIn(audioSource, 0.8f, flashbackClips[flashNum]));
    }

    private void GetPreviousFlashbacks()
    {
        for(int i = 0; i < flashbacks.Length+1; i++)
        {
            if (PlayerPrefs.GetInt(i.ToString(), 0) == 1) flashbacksGotten.Add(i);
        }
    }

    private void SpawnPreviousFlashbackObjs()
    {
        if(flashbacksGotten.Count > 0)
        {
            foreach (int n in flashbacksGotten)
            {
                GameObject obj = flashbackObjs[n];
                obj.SetActive(true);
                LeanTween.alpha(obj, 0.4f, 2f).setEaseInOutQuad();
            }
        }
    }

    public void CoinOver() //when coin is over slot machine coin slot thing
    { 
        coin.GetComponent<Coin>().GrowCoin();
    } 
    public void CoinExit() //when coin leaves slot machine coin slot thing
    {
        coin.GetComponent<Coin>().ShrinkCoin();
    } 

    public void CoinIn() //when coin is deposited - mouse click
    {
        if(!button.interactable && solidFaded && coinLayer.activeSelf)
        {
            SpawnPreviousFlashbackObjs();
            CheckIfDoneAll();
            SpeedCheat();

            button.interactable = true;

            //kid yeehaw
            LeanTween.alpha(kid, 0.4f, 0.6f).setEaseInOutQuad().setOnComplete(() => { LeanTween.alpha(kid, 0, 0.65f).setEaseInOutQuad().setDelay(0.35f); });

            shinyCoinSlot.SetActive(false);

            coin.GetComponent<Coin>().enabled = false;
            LeanTween.scale(coin, Vector3.zero, 0.8f).setEaseInOutQuad().setOnComplete(() => {
                coinLayer.SetActive(false);
            });
            
            LeanTween.alpha(slotCover, 0, 0.5f);
            boxCollider.enabled = false;

            audioSource.PlayOneShot(coinIn); //coin sfx
        }
    }

    public void Spin()
    {
        foreach (SlotLine slotLine in slotLines)
        {
            float randomNum = Random.Range(1f, 8f);
            slotLine.SpinReel();
        }
    }

    private void InitDefaults()
    {
        GetPreviousFlashbacks();

        //VAR//

        audioSource = GetComponent<AudioSource>();
        inkManager = GetComponent<InkManager>();
        button.interactable = false;
        coinLayer.SetActive(true);

        //UI//

        blackCanvasGroup.gameObject.SetActive(true);
        blackCanvasGroup.alpha = 1;
    
        LeanTween.alphaCanvas(blackCanvasGroup, 0, 1f);

        StartCoroutine(FadeIn(audioSource, 0.8f, heartbeat));
    }

    private void SpeedCheat()
    {
        if(flashbacksGotten.Count > 3)
        {
            List<SlotLine> chosenSlotLines = new List<SlotLine> { slotLines[0], slotLines[Random.Range(1, 2)] };
            List<int> neededFlashbacks = new List<int> { };

            for (int i = 0; i < flashbacks.Length; i++) if (!flashbacksGotten.Contains(i)) neededFlashbacks.Add(i);

            int randNum = neededFlashbacks[Random.Range(0, neededFlashbacks.Count - 1)];
            Debug.Log(randNum);

            foreach (SlotLine sl in chosenSlotLines)
            {
                sl.random = false;
                sl.baseReelSpeed = cheatSpeeds[randNum];
            }
        }
    }

    public int MatchNameToNum(string name)
    {
        for (int i = 0; i < flashbacks.Length; i++) if (flashbacks[i].name == name) return i;
        return -1;
    }

    public void GetResults()
    //called every time a final is gotten - is actually called when all three final results are in
    //checks results for unique responses
    {

        if (AllSlotsDone() && !resultsGotten)
        {
            resultsGotten = true;
            audioSource.Stop();

            List<GameObject> res = new List<GameObject> { };
            for (int i = 0; i < 3; i++) res.Add(slotLines[i].finalResult);

            //if left two are the same
            if (res[0].name == res[1].name && res[1].name == res[2].name)
            {
                //all are same
                //returns coin if good? shatters if bad
                SetOverlap(5, 2.3f);
                CallFlashback(MatchNameToNum(res[0].name));

                SetNotifs(res);
            }
            else
            {
                //checks if any cancel out the opposite (good money/bad money)
                List<int> cancelRes = CheckCancel(res);
                if (cancelRes.Count > 0)
                {
                    animDone = false;
                    LeanTween.alpha(overlaps[cancelRes[0]], 0.6f, 0.5f).setEaseInOutQuad();
                    LeanTween.alpha(overlaps[cancelRes[1]], 0.6f, 0.5f).setEaseInOutQuad().setOnComplete(() => animDone = true);

                    List<GameObject> tempRes = res;
                    tempRes.Remove(res[cancelRes[0]]);
                    tempRes.Remove(res[cancelRes[1] - 1]);
                    res = tempRes;

                    RefreshButton();
                    SetNotifs(res);
                    if (shouldEnd) CallEnd();
                }
                else
                {
                    if (!isEnd)
                    {
                        SetNotifs(res);

                        //checks neighbors for SAME
                        if (res[0].name == res[1].name)
                        {
                            //left two are same
                            SetOverlap(3, 2f);
                            CallFlashback(MatchNameToNum(res[0].name));
                        }
                        else if (res[1].name == res[2].name)
                        {
                            //right two are same
                            SetOverlap(4, 2f);
                            CallFlashback(MatchNameToNum(res[1].name));
                        }
                        else if (res[0].name == res[2].name)
                        {
                            //right two are same
                            SetOverlap(6, 2.3f);
                            CallFlashback(MatchNameToNum(res[0].name));
                        }
                        else
                        {
                            if (shouldEnd) CallEnd();
                            RefreshButton();
                        }
                    }
                }
            }
            
        }
    }

    private void SetOverlap(int overlapNum, float sca)
    {
        animDone = false;
        GameObject overlap = overlaps[overlapNum];
        overlap.transform.localScale = Vector3.zero;
        LeanTween.scale(overlap, new Vector3(sca, sca, sca), 1f).setEaseInOutQuad().setOnComplete(() => animDone = true);
        LeanTween.alpha(overlap, 1f, 0.5f).setEaseInOutQuad();
    }

    //END VAR
    public bool shouldEnd = false;
    private bool isEnd = false;

    public void CallEnd()
    {
        LeanTween.alpha(slotCover, 1, 0.4f).setEaseInOutQuad();
        slider.value = 0;

        isEnd = true;
        
        overlapParent.gameObject.SetActive(false);
        //flashbackObj.SetActive(false);
        endCoin.transform.position = new Vector3(-1.5f, -2.6f, 0.3f);
        endCoin.SetActive(true);

        audioSource.PlayOneShot(coinOut); //coin sfx
        LeanTween.moveLocalY(endCoin, -3.3f, 0.6f).setEaseInOutCirc();
        LeanTween.moveLocalX(endCoin, -1.6f, 0.8f).setEaseInOutCirc();
        LeanTween.rotateZ(endCoin, -24f, 0.8f).setEaseInOutQuad();
    }

    public void TriggerEndScreen()
    {
        LeanTween.alphaCanvas(blackCanvasGroup, 1, 1f).setOnComplete(() => {
            SceneManager.LoadScene("Restart");
        });
    }

    private void SetNotifs(List<GameObject> res)
    {
        foreach(GameObject r in res)
        {
            GameObject notifPrefab;

            if(!negList.Contains(r.name))
            {
                notifPrefab = posNotifPrefab;
                if (slider.value + 5 <= slider.maxValue) slider.value += 5;
                else slider.value = slider.maxValue;
            }
            else
            {
                notifPrefab = negNotifPrefab;
                if (slider.value - 5 > 0) slider.value -= 5;
                else
                {
                    slider.value = 0;
                    button.interactable = false;
                    shouldEnd = true;
                }
            }

            //makes screen grayer if less health, brighter if more health
            float alpha = (((float)slider.maxValue - (float)slider.value) / (float)slider.maxValue) * 0.25f;
            UpdateGray(alpha);

            float localPrefY = notifPrefab.transform.localPosition.y;
            float yPos =
                notifParent.childCount == 0 ? localPrefY :
                notifParent.GetChild(notifParent.childCount - 1).localPosition.y - 30;

            GameObject notif = Instantiate(notifPrefab, notifParent);
            notif.transform.GetChild(0).GetComponent<Image>().sprite = r.GetComponent<SpriteRenderer>().sprite;

            notif.transform.localPosition = new Vector3(notif.transform.localPosition.x, yPos, notif.transform.localPosition.z);
            
            LeanTween.alphaCanvas(notif.GetComponent<CanvasGroup>(), 1, 1f);
            LeanTween.moveLocalY(notif, notif.transform.localPosition.y + 10, 1f).setEaseInOutQuad().setOnComplete(() => {
                LeanTween.moveLocalY(notif, notif.transform.localPosition.y + 5, 1f).setEaseInOutQuad().setDelay(0.3f);
                LeanTween.alphaCanvas(notif.GetComponent<CanvasGroup>(), 0, 0.4f).setDelay(0.3f).setOnComplete(() => { Destroy(notif); });              
            });

        }
    }

    private void UpdateGray(float num)
    {
        LeanTween.alpha(grayObj, num, 0.3f).setEaseInOutQuad();
    }

    public void SetDefaults()
    {
        resultsGotten = false;
        button.interactable = false;
        foreach (GameObject obj in overlaps) {
            obj.GetComponent<SpriteRenderer>().color = new Color(255,255,255, 0);
            //obj.transform.localScale = Vector3.one;
        }
    }

    private List<int> CheckCancel(List<GameObject> res)
    {
        //stores which places to cancel at
        List<int> cancelAt = new List<int> { };

        //iterates through all slots and finds opposites
        //checks if the string is present in any of the slots already
        for (int i = 0; i < 3; i++)
        {
            string oppString = FindOppString(res[i]);

            for (int x = 0; x < 3; x++)
            {
                //Debug.Log(res[i].name + " and " + oppString + " are " + (res[i].name == oppString).ToString());
                if (res[x].name == oppString && !cancelAt.Contains(i) && !cancelAt.Contains(x))
                {
                    cancelAt.Add(i);
                    cancelAt.Add(x);
                }
            }
        }

        return cancelAt;
    }

    private string FindOppString(GameObject res)
    {
        string opp =
            res.name == "House G" ? "House R" :
            res.name == "House R" ? "House G" :
            res.name == "Money G" ? "Money R" :
            res.name == "Money R" ? "Money G" :
            "NULL";

        return opp;
    }

    public bool AllSlotsDone()
    //checks if all three results are in (in case one is updated faster than the other -- dont want to use null as a result)
    {
        foreach(SlotLine slotLine in slotLines) if (slotLine.finalResult == null || slotLine.easing == true) return false;
        return true;
    }

    //AUDIO//
    public static IEnumerator FadeOut(AudioSource aSource, float FadeTime)
    {
        float startVolume = aSource.volume;

        while (aSource.volume > 0)
        {
            aSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        aSource.Stop();
        aSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(AudioSource aSource, float FadeTime, AudioClip clip)
    {
        aSource.clip = clip;
        aSource.Play();

        float endVolume = 0.95f;

        while (aSource.volume <  endVolume)
        {
            aSource.volume += endVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        aSource.volume = endVolume;
    }
}
