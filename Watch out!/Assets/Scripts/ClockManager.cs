using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles everything to do with the clock, which boils down to ticking
public class ClockManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> hands = new List<GameObject>();
    private float actualTickSpeed;
    [SerializeField] private GameObject clockTrigger;

    [Header("SOUNDS")]
    [SerializeField] private AudioClip tick;
    [SerializeField] private AudioClip miss;

    public static bool TimeIsTwelwe = false;
    public static bool ClockHasBeenRung = false;
    private int currentTime;
    private bool firstDoesntCount = true;

    private void Start()
    {
        TimeManager.ChangeTickTime();
        for(int i = 0; i < hands.Count; i++)
        {
            if(i != 0)
            {
                hands[i].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
            }
        }
        currentTime = 0;
        StartCoroutine(Tick());
    }

    private void RotateTrigger()
    {
        Vector3 q = clockTrigger.transform.rotation.eulerAngles;
        q.z -= 30;
        clockTrigger.transform.eulerAngles = q;
    }

    private IEnumerator Tick()
    {
        TimeManager.ChangeTickTime();
        yield return new WaitForSeconds(TimeManager.tickTime);
        hands[currentTime].GetComponent<SpriteRenderer>().color = GameManager.OffColor;

        if (!firstDoesntCount)
        {
            if (currentTime == 0 && !ClockHasBeenRung)
            {
                GameManager.GivePenalty();
                SoundManager.PlaySound(miss);
            }
            else { ClockHasBeenRung = false; }
        }
        else { firstDoesntCount = false; }

        if(currentTime < 11)
            currentTime++;
        else
            currentTime = 0;

        hands[currentTime].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        RotateTrigger();

        if (!SoundManager.source.isPlaying)
        {
            SoundManager.PlaySound(tick);
        }
        StartCoroutine(Tick());
    }
}
