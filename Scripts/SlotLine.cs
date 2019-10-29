using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotLine : MonoBehaviour
    //attaches to indiviudal slot line
{
    public bool random = true;
    public float baseReelSpeed = 15;
    private readonly float slowReelSpeed = 1f;
    private float reelSpeed;
    //private readonly float slotHeight = 0.7f;

    public SlotManager slotManager;

    public bool spinning = false;
    public bool easing = false;

    private Transform slotsParent;
    private List<GameObject> slots = new List<GameObject> { };
    private List<GameObject> slotResult = new List<GameObject> { };
    public GameObject finalResult = null;

    //COLLIDER//

    public List<Collider2D> windowObjs = new List<Collider2D> { };

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!windowObjs.Contains(coll)) windowObjs.Add(coll);
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (windowObjs.Contains(coll)) windowObjs.Remove(coll);
    }

    //ANIMATION//

    public void SpinReel()
    {
        if (!spinning && !easing && slotManager.animDone) StartCoroutine(SpinAndStop());
    }

    public void Start()
    {
        slotsParent = transform;
        foreach (Transform child in slotsParent) slots.Add(child.gameObject);
    }

    public void Update()
    {
        if (spinning)
        //manages spin action
        {
            foreach (GameObject slot in slots)
            {
                float newReelSpeed = reelSpeed * Time.deltaTime;
                slot.transform.Translate(Vector3.down * newReelSpeed);

                if (slot.transform.position.y <= 0.5f)
                {
                    Vector3 newPos = slot.transform.position;
                    newPos.y = (newPos.y + (8f*0.7f)+(1.0f));
                    //7.3f = slot# * slotHeight + 0.5
                    slot.transform.position = newPos;
                }
            }
        }
        else if (easing)
        {
            foreach (GameObject slot in slots)
            {
                if (finalResult != null)
                {
                    if (finalResult.transform.localPosition.y > 1.2f)
                    {
                        float newReelSpeed = slowReelSpeed * Time.deltaTime;
                        slot.transform.Translate(Vector3.down * newReelSpeed);

                        if (slot.transform.position.y <= -0.5f)
                        {
                            Vector3 newPos = slot.transform.position;
                            newPos.y = (newPos.y + (7.3f));
                            //7.3f = slot#+1 * slotHeight + 2*0.5
                            slot.transform.position = newPos;
                        }
                    }
                    else
                    {
                        easing = false;
                        random = true;
                        slotManager.GetResults();
                    }
                }
                else
                {
                    easing = false;
                    random = true;
                    SpinReel();
                }
            }
        }
    }

    IEnumerator SpinAndStop()
    //manages how long spin lasts
    {
        SetDefaults();
        spinning = true;

        StartCoroutine("SlowSpin");
        yield return new WaitForSeconds(2.5f);
        StopCoroutine("SlowSpin");
        reelSpeed = baseReelSpeed;

        spinning = false;

        finalResult = ApproxSlot();
        easing = true;
    }

    float randSlowSpin;

    private void SetDefaults()
    {
        StartCoroutine(SlotManager.FadeIn(SlotManager.SM.audioSource, 0.8f, SlotManager.SM.reel));
        slotManager.SetDefaults();

        if (random)
        {
            randSlowSpin = Random.Range(0.18f, 0.22f);
            float randBaseSpeed = Random.Range(10f, 25f);
            baseReelSpeed = randBaseSpeed;
        }

        randSlowSpin = 0.2f;
        reelSpeed = baseReelSpeed;
        finalResult = null;
        slotResult = new List<GameObject> { };
    }

    IEnumerator SlowSpin()
    //slows spin gradually
    {
        while (true)
        {
            reelSpeed *= 0.8f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private GameObject ApproxSlot()
    //finds highest object in trigger so that it can ease to that one
    //doesn't get stuck at one obj!
    {
        List<Collider2D> colls = windowObjs;
        GameObject nearestObj = null;

        foreach (Collider2D coll in colls)
        {
            if (nearestObj == null || coll.gameObject.transform.position.y > nearestObj.gameObject.transform.position.y) nearestObj = coll.gameObject;
        }
        return nearestObj;
    }
}
