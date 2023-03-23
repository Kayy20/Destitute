using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]

public class AudioNewSound : EditorWindow
{
    private string soundName;
    private AudioClip[] clips = new AudioClip[10];

    private AudioMixerGroup mixerGroup;

    private int numberOfClips = 1;
    private bool multipleClips;

    private float volume = 0.5f;
    
    private bool randomVolume = false;
    private float lowerVol = 0;
    private float upperVol = 1;
    
    private bool randomPitch = false;
    private float lowerPitch = 0;
    private float upperPitch = 1;
    
    private float pitch = 1;

    private bool isSound3D = true;

    private bool isLooping;
    
    [MenuItem("Tools/Audio/NewSound")]
    static void Init()
    {
        AudioNewSound window = (AudioNewSound) EditorWindow.GetWindow(typeof(AudioNewSound));
        window.Show();
    }

    private void OnDisable()
    {
        CD.Log(CD.Programmers.DANIEL, "DISABLE", Color.red);
        ClearTempFolder();
    }

    private void OnGUI()
    {
        GUILayout.Label("New Sound", EditorStyles.largeLabel);
        
        GUILayout.Space(10);
        
        BasicSoundSettingsView();
        
        GUILayout.Space(10);
        
        MixerView();
        
        GUILayout.Space(10);
        
        RandomVolumeView();
        
        GUILayout.Space(10);
        
        PitchView();
        
        GUILayout.Space(10);
        
        OtherSettingsView();
        
        GUILayout.Space(10);
        
        LoopView();
        
        GUILayout.Space(10);
        
        ConfirmView();
        
        GUILayout.Space(10);
        
        PreviewButton();

    }

    private void BasicSoundSettingsView()
    {
        soundName = EditorGUILayout.TextField("Sound Name", soundName);

        //clip = (AudioClip)EditorGUILayout.ObjectField("Sound Clip",(AudioClip)clip,typeof(AudioClip), true);

        if (multipleClips)
        {
            numberOfClips = EditorGUILayout.IntField("Number of clips: ", numberOfClips);
            numberOfClips = Mathf.Clamp(numberOfClips, 1, 10);

            for (int i = 0; i < numberOfClips; i++)
            {
                clips[i] = (AudioClip)EditorGUILayout.ObjectField($"Clip {i+1}",(AudioClip)clips[i],typeof(AudioClip), true);
            }
        }
        else
        {
            clips[0] = (AudioClip)EditorGUILayout.ObjectField($"Sound Clip",(AudioClip)clips[0],typeof(AudioClip), true);
        }
        
        multipleClips = EditorGUILayout.Toggle("Multiple Clips?", multipleClips);
        
    }

    private void MixerView()
    {
        mixerGroup = (AudioMixerGroup)EditorGUILayout.ObjectField($"Mixer",(AudioMixerGroup)mixerGroup,typeof(AudioMixerGroup), true);
    }

    private void RandomVolumeView()
    {
        randomVolume = EditorGUILayout.Toggle("Random Volume?", randomVolume);
        if (!randomVolume)
        {
            volume = EditorGUILayout.Slider("Volume: ", volume, 0, 1);
        }
        if (randomVolume)
        {
            EditorGUILayout.MinMaxSlider("Volume: ",ref lowerVol,ref upperVol, 0,1f);
            EditorGUILayout.LabelField($"Lower Limit: ", lowerVol.ToString());
            EditorGUILayout.LabelField($"Upper Limit: ", upperVol.ToString());
        }
    }

    private void PitchView()
    {
        randomPitch = EditorGUILayout.Toggle("Random Pitch?", randomPitch);
        
        if (!randomPitch)
        {
            pitch = EditorGUILayout.Slider("Pitch: ", pitch, 0, 2);
        }
        if (randomPitch)
        {
            EditorGUILayout.MinMaxSlider("Pitch: ",ref lowerPitch,ref upperPitch, 0,2f);
            EditorGUILayout.LabelField($"Lower Limit: ", lowerPitch.ToString());
            EditorGUILayout.LabelField($"Upper Limit: ", upperPitch.ToString());
        }
    }

    private void OtherSettingsView()
    {
        isSound3D = EditorGUILayout.Toggle("3D?", isSound3D);
    }

    private void LoopView()
    {
        isLooping = EditorGUILayout.Toggle("Looping?", isLooping);
    }

    private void ConfirmView()
    {
        if (GUILayout.Button("Confirm"))
        {
            AudioSoundSO audioSoundSo = CreateSO();
            
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = audioSoundSo;

        }
    }

    private AudioSoundSO CreateSO(string overridePath = "")
    {
        AudioSoundSO audioSoundSo = ScriptableObject.CreateInstance<AudioSoundSO>();
        
        Array.Resize(ref clips, numberOfClips);
        audioSoundSo.clips = clips;
        
        Array.Resize(ref clips, 10);
        
        audioSoundSo.numberOfClips = numberOfClips;

        audioSoundSo.mixerGroup = mixerGroup;
        
        if (randomVolume)
        {
            audioSoundSo.minVolume = lowerVol;
            audioSoundSo.maxVolume = upperVol;
        }
        else
        {
            audioSoundSo.minVolume = volume;
            audioSoundSo.maxVolume = volume;
        }

        audioSoundSo.soundName = soundName;
        audioSoundSo.is3D = isSound3D;
        audioSoundSo.isLooping = isLooping;
        
        if (randomPitch)
        {
            audioSoundSo.minPitch = lowerPitch;
            audioSoundSo.maxPitch = upperPitch;
        }
        else
        {
            audioSoundSo.minPitch = pitch;
            audioSoundSo.maxPitch = pitch;
        }
        
        string path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/Tools/Audio/Resources/{overridePath}{soundName}.asset");
        AssetDatabase.CreateAsset(audioSoundSo, path);
        AssetDatabase.SaveAssets();

        return audioSoundSo;

    }

    private void PreviewButton()
    {
        if(!EditorApplication.isPlaying) return;
        
        if (GUILayout.Button("Preview Sound Clip"))
        {
            AudioSoundSO previewSO = CreateSO("Temp/");
            SoundController.PlaySound(previewSO);
        }
    }

    private void ClearTempFolder()
    {
        string path = "Assets/Tools/Audio/Resources/Temp";
        
        var hi = Directory.GetFiles(path);
 
        for (int i = 0; i < hi.Length; i++) {
            File.Delete(hi[i]);
            CD.Log(CD.Programmers.DANIEL, $"deleting file {hi[i]}", Color.magenta);
        }
        
        Directory.Delete(path);
        Directory.CreateDirectory(path);

    }

}