using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerTutorialFlag : MonoBehaviour
{
    public static BunkerTutorialFlag instance;

    public bool hasCompleted;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            Debug.Log("DESTROYING");
            return;
        }

        instance = this;
        
        DontDestroyOnLoad(this);
    }
}
