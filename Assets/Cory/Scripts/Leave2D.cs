using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leave2D : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            BoidManager.Instance.CleanUp(); // Destroy the boids (birds)
            MapGeneration.Instance.ReturnToMap("TestBox");

            StaticClassVariables.score += 50;
        }
    }
}
