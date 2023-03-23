using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    [SerializeField] private AudioSoundSO aso;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("PLAYING");
            SoundController.PlaySound(aso);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("STOPPING");
            SoundController.StopSound(aso);
        }
    }
}
