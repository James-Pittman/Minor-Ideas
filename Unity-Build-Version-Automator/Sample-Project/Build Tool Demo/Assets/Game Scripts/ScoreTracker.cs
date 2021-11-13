using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    //[HideInInspector]
    public int finalScore = 0;

    public void Start()
    {
        if (TryGetComponent(out Text output))
            output.text = finalScore.ToString();
    }

    public void updateScore(int score)
    {
        finalScore += score;
        if (TryGetComponent(out Text output))
            output.text = finalScore.ToString();
    }
}
