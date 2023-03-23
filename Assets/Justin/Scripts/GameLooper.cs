using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLooper : MonoBehaviour
{

    [SerializeField]
    PlayerMap pMap;

    private void OnEnable()
    {

        if(MapGeneration.Instance!=null)
            if (pMap.currentVisitingNode == MapGeneration.Instance.EndNode)
            {

                CD.Log(CD.Programmers.JUSTIN, "FINAL NODE. Resetting Map");

                //pMap.GenerateNewMap();

            }

    }
}
