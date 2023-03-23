using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioImplementationTest : MonoBehaviour
{
    [SerializeField] private AudioSoundSO sound01;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SoundController.PlaySound(sound01);
        }
    }
}
