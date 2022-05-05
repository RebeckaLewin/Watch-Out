using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attaches to the four cogs in the scene
 * Does anything related to the singular instances of a cog, changing the sprite color, play sound, etc
 * Can break and be fixed
*/
public class CogScript : MonoBehaviour
{
    [SerializeField] private BreakManager bm;
    [SerializeField] private Color bad, broken;
    [SerializeField] private GameObject[] smoke = new GameObject[2];
    [SerializeField] private AudioClip crack, fire, fix;
    public int BrokenStage { get; set; }

    public void Deteriorate()
    {
        smoke[0].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        SoundManager.PlaySound(crack);
        StartCoroutine(Break());
    }

    private IEnumerator Break()
    {
        float time = Random.Range(TimeManager.MinTimeDet, TimeManager.MaxTimeDet);
        yield return new WaitForSeconds(time);
        BreakManager.numOfBrokenCogs++;
        smoke[0].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
        smoke[1].GetComponent<SpriteRenderer>().color = GameManager.OnColor;
        SoundManager.PlaySound(fire);

        if (BreakManager.numOfBrokenCogs == 4)
        {
            GameManager.loseState = true;
        }
        BrokenStage++;
    }

    public void FixCog()
    {
        if(BrokenStage > 0)
        {
            StopAllCoroutines();
            SoundManager.PlaySound(fix);
            if (BrokenStage == 2)
            {
                BreakManager.numOfBrokenCogs--;
            }
            BrokenStage = 0;
            smoke[0].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
            smoke[1].GetComponent<SpriteRenderer>().color = GameManager.OffColor;
            bm.AddCog(gameObject);
            ScoreManager.UpdateScore(2);
        }
    }
}
