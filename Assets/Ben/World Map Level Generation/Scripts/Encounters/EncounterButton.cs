using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EncounterButton : MonoBehaviour
{

    public Text text;


    private string currentString;

    private string startingString;

    public void SetStartingString(string s)
    {
        startingString = s;
        CurrentString = s;
    }


    public string CurrentString
    {
        get
        {
            return currentString;
        }
        set 
        { 
            text.text = value;
            currentString = value;
        }
    }

    public void RevertToStartingString()
    {
        CurrentString = startingString;
    }

    public string GetStartingString()
    {
        return startingString;
    }

}
