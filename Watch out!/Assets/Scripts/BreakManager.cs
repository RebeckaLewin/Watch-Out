using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Responisble for breaking cogs, as well as keeping track of the number of them. 
//If there is at least one intact cog, it start the cog's detoriation coroutine
public class BreakManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cogs = new List<GameObject>();
    public static List<GameObject> intactCogs = new List<GameObject>();
    public static int numOfBrokenCogs = 0;

    private void Start()
    {
        intactCogs = cogs;
        numOfBrokenCogs = 0;
    }

    public void StartBreaking()
    {
        StartCoroutine(Break());
    }

    private IEnumerator Break()
    {
        float time = Random.Range(TimeManager.MinTimeBreak, TimeManager.MaxTimeBreak);

        yield return new WaitForSeconds(time);

        int randomCog = Random.Range(0, intactCogs.Count - 1);
        CogScript cs = intactCogs[randomCog].GetComponent<CogScript>();
        cs.BrokenStage++;
        cs.Deteriorate();
        intactCogs.RemoveAt(randomCog);

        if(intactCogs.Any())
        {
            StartCoroutine(Break());
        }
    }

    public void AddCog(GameObject cog)
    {
        intactCogs.Add(cog);
        if(intactCogs.Count == 1)
        {
            StartCoroutine(Break());
        }
    }
}
