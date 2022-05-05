using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A script for the bell behaviour, its swinging more precisely
*/
public class BellScript : MonoBehaviour
{
    [SerializeField] private GameObject[] bells = new GameObject[2];

    public void Swing()
    {
        bells[0].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
        bells[1].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        StartCoroutine(ComeBack());
    }

    private IEnumerator ComeBack()
    {
        yield return new WaitForSeconds(0.4f);
        bells[1].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
        bells[0].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
    }
}
