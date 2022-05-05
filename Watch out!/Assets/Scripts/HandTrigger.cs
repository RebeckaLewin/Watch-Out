using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attaches to the clock, checks if time is twelve
public class HandTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ClockManager.TimeIsTwelwe = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ClockManager.TimeIsTwelwe = false;
    }
}
