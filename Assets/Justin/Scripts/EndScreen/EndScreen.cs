using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    GameObject visualsGO;
    [SerializeField]
    EndScreenScore ess;

    public bool isActive = false;

    private static EndScreen instance;
    public static EndScreen Instance 
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null || instance == this)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        LoadScore();
    }

    public void ShowEndScreen(CauseofDeath causeofDeath)
    {
        if (StaticClassVariables.score > StaticClassVariables.highScore)
        {
            StaticClassVariables.highScore = StaticClassVariables.score;
            SaveScore();
        }

        isActive = true;
        EndScreenScore.causeofDeath = causeofDeath;

        visualsGO.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        try
        {
            PlayerMap.Instance.StopAllCoroutines();
        }
        catch { }        

        PlayerMap.ResetInstance();
        MapGeneration.Instance = null;


        SaveSystem.DeleteInventory();

    }

    public void Restart()
    {
        StaticClassVariables.score = 0;
        StaticClassVariables.Wounds = 0;

        Debug.Log("RESETING MAP");

        SceneManager.LoadScene("MapScene");

        visualsGO.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isActive = false;
    }

    public void BackToMainMenu()
    {
        StaticClassVariables.score = 0;
        StaticClassVariables.Wounds = 0;

        isActive = false;

        SceneManager.LoadSceneAsync("Title Screen");
        SceneManager.UnloadSceneAsync("MapScene");
        visualsGO.SetActive(false); 
    }

    private void SaveScore()
    {
        SaveSystem.SaveScore(StaticClassVariables.score);
    }

    private void LoadScore() 
    {
        try
        {
            SaveSystem.ScoreSaveData ssd = SaveSystem.LoadScore();
            StaticClassVariables.highScore = ssd.Score;

        }
        catch (ArgumentException e)
        {

        }
    }
}
