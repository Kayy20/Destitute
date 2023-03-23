using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundController : MonoBehaviour
{
    private static SoundController Instance;
    
    [SerializeField] private int maxAudioSources;
    
    private List<GameObject> _sourceGOs = new List<GameObject>();
    private List<AudioInfo> _audioInfos = new List<AudioInfo>();
    
    private void Awake()
    {
        if (Instance != null && Instance.gameObject != this.gameObject)
        {
            Debug.LogWarning($"SOUND CONTROLLER ALREADY EXISTS ON {SoundController.Instance.gameObject}, DELETING {gameObject}");
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CD.Log(CD.Programmers.DANIEL, "CALLING START",Color.yellow);
        for (int i = 0; i < maxAudioSources; i++)
        {
            //create game object
            GameObject audioGO = new GameObject();
            audioGO.transform.parent = this.transform;
            audioGO.name = $"AudioSource{i + 1}";
            AudioSource source = audioGO.AddComponent<AudioSource>();
            
            //create info
            AudioInfo info = new AudioInfo(source, gameObject.transform);
            
            //add to internal lists
            _sourceGOs.Add(audioGO);
            _audioInfos.Add(info);

        }
    }

    private void Update()
    {
        AudioTimeTick();
    }

    private void AudioTimeTick()
    {
        foreach (AudioInfo audioInfo in _audioInfos)
        {
            if(!audioInfo.IsPlaying) continue;

            audioInfo.AddTime(Time.deltaTime);

            if (audioInfo.IsPlayingComplete())
            {
                audioInfo.FinishPlaying();
            }
        }
    }

    private AudioInfo NextFreeAudio()
    {
        foreach (AudioInfo audioInfo in _audioInfos)
        {
            if (!audioInfo.IsPlaying) return audioInfo;
        }

        return null;
    }


    public static void PlaySound(AudioSoundSO sound)
    {
        AudioInfo info = Instance.NextFreeAudio();
        try
        {
            if(info.Source == null) return;
        }
        catch (Exception e)
        {
            return;
        }
        info.StartPlaying(sound);

    }
    
    public static void PlaySound(AudioSoundSO sound, Transform attached)
    {
        AudioInfo info = Instance.NextFreeAudio();
        if(info == null) return;
        
        info.StartPlaying(sound, attached);
    }

    public static void StopSound(AudioSoundSO sound)
    {
        foreach (AudioInfo audioInfo in Instance._audioInfos)
        {
            if (audioInfo.CurrentSound == sound)
            {
                audioInfo.FinishPlaying(true);
            }
        }
    }
}
