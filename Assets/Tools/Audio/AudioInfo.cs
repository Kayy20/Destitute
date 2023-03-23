using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioInfo
{
    private float _currentTime;
    private float _maxTime;
    private readonly AudioSource _source;
    private bool _isPlaying;
    private bool _isLooping;
    private readonly Transform _controllerParent;
    private Transform _currentParent;
    private AudioSoundSO _currentSound;

    public bool IsLooping => _isLooping;
    public bool IsPlaying => _isPlaying;
    public AudioSource Source => _source;
    public AudioSoundSO CurrentSound => _currentSound;
    
    public AudioInfo(AudioSource source, Transform parent)
    {
        _source = source;
        _controllerParent = parent;
    }

    public void StartPlaying(AudioSoundSO sound)
    {
        if (!_source)
        {
            return;
        }
        //CD.Log(CD.Programmers.DANIEL, $"{_source.gameObject.name} has started playing");
        AssignValues(sound);
        _source.Play();
    }
    
    public void StartPlaying(AudioSoundSO sound, Transform t)
    {
        //CD.Log(CD.Programmers.DANIEL, $"{_source.gameObject.name} has started playing");
        AssignParent(t);
        AssignValues(sound);
        _source.Play();
    }

    private void AssignValues(AudioSoundSO sound)
    {
        

        AudioClip clip = RandomClip(sound);
        _isPlaying = true;
        _maxTime = clip.length;
        _currentTime = default;
        _isLooping = sound.isLooping;

        _currentSound = sound;

        
        _source.clip = clip;
        _source.loop = sound.isLooping;
        
        float audioPitch = Random.Range(sound.minPitch, sound.maxPitch);
        _source.pitch = audioPitch;
        
        _source.outputAudioMixerGroup = sound.mixerGroup;

        _source.spatialBlend = sound.is3D ? 1 : 0;

        float audioVolume = Random.Range(sound.minVolume, sound.maxVolume);
        _source.volume = audioVolume;
    }

    public void FinishPlaying(bool forceLoopStop = false)
    {
        if (IsLooping && !forceLoopStop) {
            StartPlaying(_currentSound);
            return;
        }
        

        //CD.Log(CD.Programmers.DANIEL, $"{_source.gameObject.name} has stopped playing");
        try
        {
            _source.Stop();
            _isPlaying = false;
            _currentTime = default;
            _maxTime = default;
            _isLooping = false;
            _currentSound = null;
            _source.clip = null;
            ClearParent();
        }
        catch (Exception e)
        {
            
        }
        
    }

    public void AddTime(float time)
    {
        _currentTime += time;
    }

    public bool IsPlayingComplete()
    {
        return _currentTime >= _maxTime;
    }

    private void AssignParent(Transform t)
    {
        _currentParent = t;
        _source.transform.parent = t;
        _source.transform.position = t.position;
    }

    private void ClearParent()
    {
        _currentParent = null;
        _source.transform.parent = _controllerParent;
        _source.transform.position = _controllerParent.transform.position;
    }

    private static AudioClip RandomClip(AudioSoundSO soundSo)
    {
        int rand = Random.Range(0, soundSo.numberOfClips);
        return soundSo.clips[rand];
    }
}
