using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/*
 * The game manager who manages the game state, as well as player input not related to actual gameplay (such as resetting the game)
 * Also keeps track of the lives
*/
public class GameManager : MonoBehaviour
{
    [SerializeField] private Color onColor, offColor;
    public static Color OnColor { get; set; }
    public static Color OffColor { get; set; }

    [SerializeField] private List<GameObject> tries = new List<GameObject>();
    private static List<GameObject> missSprites = new List<GameObject>();
    private static int misses = 0;

    [SerializeField] private GameObject loss, quitMessage;
    [SerializeField] private AudioClip loseSound, pauseSound;
    [SerializeField] private GameObject player, eventsystem;

    private GameObject lossMessage;
    public static bool loseState = false;

    private bool soundHasBeenPlayed = false;
    private bool paused, quitting;
    
    private void Start()
    {
        OnColor = onColor;
        OffColor = offColor;

        misses = 0;
        missSprites = tries;
        foreach(GameObject miss in missSprites)
        {
            miss.GetComponent<Image>().color = OffColor;
        }
        lossMessage = loss;
    }

    private void Update()
    {
        if (loseState)
        {
            if (!soundHasBeenPlayed)
            {
                SoundManager.PlaySound(loseSound);
                LeanTween.moveY(loss.GetComponent<RectTransform>(), 0, 1f).setIgnoreTimeScale(true);
                soundHasBeenPlayed = true;
            }
            Time.timeScale = 0;
        }

        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            lossMessage.SetActive(false);
            loseState = false;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            if (paused)
            {
                Time.timeScale = 1f;
                eventsystem.GetComponent<EventSystem>().sendNavigationEvents = true;
            }

            else
            {
                Time.timeScale = 0f;
                eventsystem.GetComponent<EventSystem>().sendNavigationEvents = false;
                
            }
            SoundManager.PlaySound(pauseSound);
            paused = !paused;
        }

        if (Keyboard.current[Key.Q].wasPressedThisFrame)
        {
            if (quitting)
                Application.Quit();
            else
            {
                Time.timeScale = 0;
                LeanTween.moveY(quitMessage.GetComponent<RectTransform>(), 0, 0.5f).setIgnoreTimeScale(true);
                eventsystem.GetComponent<EventSystem>().sendNavigationEvents = false;
                quitting = true;
            }
        }

        if (quitting)
        {
            if (Keyboard.current[Key.Space].wasPressedThisFrame)
            {
                Time.timeScale = 1f;
                LeanTween.moveY(quitMessage.GetComponent<RectTransform>(), 464, 0.2f).setIgnoreTimeScale(true);
                eventsystem.GetComponent<EventSystem>().sendNavigationEvents = true;
                quitting = false;
            }
        }
    }


    public static void GivePenalty()
    {
        misses++;
        missSprites[misses - 1].GetComponent<Image>().color = OnColor;
        if(misses == 3)
        {
            loseState = true;
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<AudioSource>().Play();
    }
}
