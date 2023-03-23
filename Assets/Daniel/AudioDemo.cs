using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDemo : MonoBehaviour
{
    public List<AudioSoundSO> sounds;
    public List<KeyCode> codes;
    
    void Update()
    {
        for (int i = 0; i < codes.Count; i++)
        {
            if (Input.GetKeyDown(codes[i]))
            {
                SoundController.PlaySound(sounds[i]);
            }
        }
    }
}
