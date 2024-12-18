using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    private Score score;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    private void Start()
    {
        this.score = FindAnyObjectByType<Level1>().score;
    }
    private void Update()
    {
        this.scoreDisplay.text = "Score : " + this.score.score;
    }

}
