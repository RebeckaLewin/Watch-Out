using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Plays sounds
 * Due to the Game&Watch nature of the game, only one sound should be played at a time. That's why there is only one audio source!
*/
public class SoundManager : MonoBehaviour
{
    public static AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip sound)
    {
        source.clip = sound;
        source.Play();
    }

    public static IEnumerator PlayNextSound(List<AudioClip> sounds, float delay)
    {
        PlaySound(sounds[0]);
        yield return new WaitForSeconds(delay);
        PlaySound(sounds[1]);
    }
}
