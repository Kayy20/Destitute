using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenScore : MonoBehaviour
{
    [SerializeField]
    Text scoreText,highScoreText,deathText;

    [SerializeField]
    GameObject deathGO, scoreGO;

    public static CauseofDeath causeofDeath;

    private void OnEnable()
    {

        deathGO.SetActive(true);
        scoreGO.SetActive(false);
        deathText.text = "You Died From " + causeofDeath;
        scoreText.text = "Score: " + StaticClassVariables.score;
        highScoreText.text = "High Score: " + StaticClassVariables.highScore;
    }

    public void OpenScore()
    {
        deathGO.SetActive(false);
        scoreGO.SetActive(true);
    }

}