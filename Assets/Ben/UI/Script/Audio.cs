using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public void OnButtonClick(AudioSoundSO sound)
    { // Player Click Button
        SoundController.PlaySound(sound);
    }

}
