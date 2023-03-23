using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LeaveBunker : MonoBehaviour
{

    bool ableToLeave;
    public GameObject eKeyToLeave;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            ableToLeave = true;
            eKeyToLeave.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            ableToLeave = false;
            eKeyToLeave.SetActive(false);
        }
    }

    private void Update()
    {
        if (ableToLeave)
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyUp(KeyCode.Mouse0))
            {
                MapGeneration.Instance.ReturnToMap("Bunker");
            }
        

    }


}
