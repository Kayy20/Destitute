using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{

    public string GameSeed = "Default";
    public int CurrentSeed = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameSeed != "" && GameSeed != "Default")
            CurrentSeed = GameSeed.GetHashCode();
        if (GameSeed == "Random")
        {
            CurrentSeed = (int)System.DateTime.Now.Ticks;
        }
        Random.InitState(CurrentSeed);
    }
}
