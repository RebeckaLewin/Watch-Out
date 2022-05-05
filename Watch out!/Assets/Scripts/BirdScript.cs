using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Behaviour for the animation of the birds on the clocktower
//A bit special due to it being a Game&Watch game!

public class BirdScript : MonoBehaviour
{
    [SerializeField] private GameObject[] sprites = new GameObject[2];
    public bool isSitting { get; set; }

    private void Start()
    {
        isSitting = true;
    }

    public void Fly()
    {
        sprites[0].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
        sprites[1].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        isSitting = false;
        StartCoroutine(GoAway());
    }

    private IEnumerator GoAway()
    {
        yield return new WaitForSeconds(0.5f);
        sprites[1].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
        StartCoroutine(Appear());
    }

    private IEnumerator Appear()
    {
        yield return new WaitForSeconds(6f);
        sprites[1].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        StartCoroutine(SitDown());
    }

    private IEnumerator SitDown()
    {
        yield return new WaitForSeconds(0.5f);
        sprites[0].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        sprites[1].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
        isSitting = true;
    }
}
