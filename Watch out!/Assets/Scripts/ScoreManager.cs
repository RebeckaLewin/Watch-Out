using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//Keeps track of score, updates the scoretext
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textObject;
    private static int totalScore;
    private static TextMeshProUGUI text;

    void Start()
    {
        text = textObject;
        totalScore = 0;
    }

    public static void UpdateScore(int addedScore)
    {
        if(totalScore + addedScore >= 0)
            totalScore += addedScore;
        else { totalScore = 0; }

        text.text = totalScore.ToString();
    }

    public static int TotalScore()
    {
        return totalScore;
    }
}
