using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/*
 * The player script, handling actions the player can take
 * Due to being one of the only scripts the player uses to directly interact with the game (in addition to time constraints),
 * it got a lot of responsibilities that requires direct triggers, such as sounds and animations
 * 
 * Note: the player is not the rat, but an invisible sprite following the rat position. The actual rat is made up of buttons
*/
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject start;
    [SerializeField] private AudioClip bing, bong;
    [SerializeField] private List<GameObject> soundMarkers = new List<GameObject>();
    [SerializeField] private BreakManager bm;
    [SerializeField] private BirdScript birds;
    [SerializeField] private BellScript bell;

    private bool byBell, byCog, firstRing;

    private GameObject currentCog;
    [SerializeField] private GameObject[] boos = new GameObject[2];
    [SerializeField] private AudioClip boo;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(start);
        firstRing = true;
    }

    private void Update()
    {
        if(Time.timeScale != 0)
        {
            Move();
            if (byBell)
            {
                BongBell();
            }
            else if (byCog)
            {
                FixCog();
            }
        }
    }

    private void Move()
    {
        if(EventSystem.current.currentSelectedGameObject != null)
            transform.position = EventSystem.current.currentSelectedGameObject.transform.position;

        if (GameManager.loseState)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void BongBell()
    {
        if(Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            bool missed = false;
            if (ClockManager.TimeIsTwelwe)
            {
                if (!ClockManager.ClockHasBeenRung)
                {
                    ClockManager.ClockHasBeenRung = true;
                    ScoreManager.UpdateScore(10);
                    TimeManager.IncreaseTickSpeed();
                    if (firstRing)
                    {
                        bm.StartBreaking();
                        firstRing = false;
                    }
                }
            }
            else
            {
                ScoreManager.UpdateScore(-5);
                int i = Random.Range(0, 2);
                GameObject currBoo = boos[i];
                currBoo.GetComponent<SpriteRenderer>().color = GameManager.OnColor;
                StartCoroutine(ChangeSpriteColor(currBoo, 1f));
                missed = true;
            }
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            obj.GetComponent<SpriteContainer>().ChangeSprite();
            if (birds.isSitting)
            {
                birds.Fly();
            }
            bell.Swing();
            RingBell(missed);
        }
    }

    private void FixCog()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            currentCog.GetComponent<CogScript>().FixCog();
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            obj.GetComponent<SpriteContainer>().ChangeSprite();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "bell")
        {
            byBell = true;
        }
        else if (collision.gameObject.CompareTag("cog"))
        {
            byCog = true;
            currentCog = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "bell")
        {
            byBell = false;
        }
        else if (collision.gameObject.CompareTag("cog"))
        {
            byCog = false;
            currentCog = null;
        }
    }

    public void RingBell(bool missed)
    {
        float delay = 0.4f;
        if (!missed)
        {
            List<AudioClip> rings = new List<AudioClip> { bing, bong };
            StartCoroutine(SoundManager.PlayNextSound(rings, delay));
        }
        else
        {
            List<AudioClip> wrongSound = new List<AudioClip> { bing, boo };
            StartCoroutine(SoundManager.PlayNextSound(wrongSound, delay));
        }
        soundMarkers[0].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        soundMarkers[1].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        StartCoroutine(Ring(delay));
    }

    private IEnumerator Ring(float delay)
    {
        yield return new WaitForSeconds(delay);
        soundMarkers[2].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        StartCoroutine(Silence());
    }

    private IEnumerator Silence()
    {
        yield return new WaitForSeconds(0.6f);
        soundMarkers[0].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
        soundMarkers[1].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
        soundMarkers[2].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
    }

    private IEnumerator ChangeSpriteColor(GameObject sprite, float time)
    {
        yield return new WaitForSeconds(time);
        sprite.GetComponent<SpriteRenderer>().color = GameManager.OffColor;
    }
}
