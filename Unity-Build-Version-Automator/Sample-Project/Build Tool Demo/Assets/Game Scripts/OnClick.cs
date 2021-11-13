using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    public GameObject score;
    // Start is called before the first frame update
    void Start()
    {
        score = GameObject.Find("Score");
    }

    private void OnMouseDown()
    {
        if (this.gameObject.name == "Circle")
            score.GetComponent<ScoreTracker>().updateScore(1);
        else
            score.GetComponent<ScoreTracker>().updateScore(-1);
        Destroy(this.gameObject);
    }
}
