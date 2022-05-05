using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles the timing of the clock and sends the data to the ClockManager
 * Calculates time based on two factors: time since start, and number of cogs broken
*/
public class TimeManager : MonoBehaviour
{
    [Header("TICKING")]
    [SerializeField] private float timeBetweenTick;
    public static float tickTime, standardTickTime, startTickTime;

    [Header("BREAKAGE")]
    [SerializeField] private float minTimeBreak;
    [SerializeField] private float maxTimeBreak;
    [SerializeField] private float lowestMinTimeBreak, lowestMaxTimeBreak;
    public static float MinTimeBreak, MaxTimeBreak;

    [Header("DETERIORATION")]
    [SerializeField] private float minTimeDet;
    [SerializeField] private float maxTimeDet;
    [SerializeField] private float lowestMinTimeDet, lowestMaxTimeDet;
    public static float MinTimeDet, MaxTimeDet;

    [Header("INTERVAL BETWEEN DIFFICULTY")]
    [SerializeField] private int interval;
    private float secSinceStart = 0;
    private int flooredSec, prevSec;
    bool secondHasPassed;

    private void Start()
    {
        startTickTime = timeBetweenTick;
        standardTickTime = timeBetweenTick;
        tickTime = standardTickTime;

        MinTimeBreak = minTimeBreak;
        MaxTimeBreak = maxTimeBreak;

        MinTimeDet = minTimeDet;
        MaxTimeDet = maxTimeDet;

        tickTime = timeBetweenTick;

        secSinceStart = 0;
    }

    private void Update()
    {
        DoAfterTime();
    }

    public static void IncreaseTickSpeed()
    {
        if(standardTickTime > startTickTime / 2.5)
            standardTickTime -= startTickTime / 40;
        ChangeTickTime();
    }

    public static void ChangeTickTime()
    {
        if (BreakManager.numOfBrokenCogs > 0)
        {
            tickTime = standardTickTime - ((standardTickTime / 4) * BreakManager.numOfBrokenCogs);
        }
        else { tickTime = standardTickTime; }
    }

    private void DoAfterTime()
    {
        secSinceStart += Time.deltaTime;
        flooredSec = Mathf.FloorToInt(secSinceStart);

        if(flooredSec != prevSec)
        {
            secondHasPassed = true;
            prevSec = flooredSec;
        }

        if (secondHasPassed)
        {
            if (flooredSec % interval == 0)
            {
                if(MinTimeDet > lowestMinTimeDet)
                    MinTimeDet *= 0.8f;
                if(MaxTimeDet > lowestMaxTimeDet)
                    MaxTimeDet *= 0.8f;

                if(MinTimeBreak > lowestMinTimeBreak)
                    MinTimeBreak *= 0.8f;
                if(MaxTimeBreak > lowestMaxTimeBreak)
                    MaxTimeBreak *= 0.8f;
            }
            secondHasPassed = false;
        }
    }
}
