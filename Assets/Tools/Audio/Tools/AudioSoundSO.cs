using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSoundSO : ScriptableObject
{
    public string soundName;
    public AudioClip[] clips;
    public AudioMixerGroup mixerGroup;
    public int numberOfClips;
    public float minVolume;
    public float maxVolume;
    public float minPitch;
    public float maxPitch;
    public bool is3D;
    public bool isLooping;

}
