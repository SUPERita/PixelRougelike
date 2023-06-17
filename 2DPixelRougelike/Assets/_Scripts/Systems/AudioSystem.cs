using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Collections;
using System;
using UnityEditor;
using System.Linq;
using DG.Tweening;
using UnityEngine.Audio;

/// <summary>
/// Insanely basic audio system which supports 3D sound.
/// Ensure you change the 'Sounds' audio source to use 3D spatial blend if you intend to use 3D sounds.
/// </summary>
public class AudioSystem : Singleton<AudioSystem> {
    [SerializeField] private string soundFolderPath = "Audio/"; // Path to the folder containing sound clips
    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    [HideInInspector] [SerializeField] private List<AudioClip> clips = new List<AudioClip>();
    [SerializeField] private bool muteDebug = false;
    /*[HideInInspector]*/ [SerializeField] private List<AudioSource> availableSources = new List<AudioSource>();
    //private List<AudioSource> workingSources = new List<AudioSource>();

    [SerializeField] private AudioSource musicSource = null;

    [SerializeField] private AudioMixerGroup musicMixerGroup = null;
    [SerializeField] private AudioMixerGroup sfxMixerGroup = null;
    private void Start()
    {
        WriteListToDictionary();
    }

    [HorizontalGroup("a")]
    [Button]
    public void PlaySound(string _clipName/*parameters control*/)
    {
        //find available source
        AudioSource _a = FindAvailableSource();
        if(_a == null) { /*Debug.Log("no Available sources");*//*ye we get it*/ return; }

        //get the sound from name
        AudioClip _soundByName = null;
        if(!soundDictionary.TryGetValue(_clipName, out _soundByName)) { Debug.LogError("no sound named: " + _clipName + " in the system"); }
        _a.clip = _soundByName;

        //Play?
        _a.Play();
    }

    [HorizontalGroup("a")]
    [Button]
    public void PlayMusic(string _musicName/*parameters control*/)
    {

        //get the sound from name
        AudioClip _soundByName = null;
        if (!soundDictionary.TryGetValue(_musicName, out _soundByName)) { Debug.LogError("no sound named: " + _musicName + " in the system"); }

        musicSource.DOKill();
        //fade out, switch, fade in
        musicSource.DOFade(0f, .5f)
            .OnComplete(() =>
            {
                // Switch to the new audio clip
                musicSource.clip = _soundByName;
                musicSource.Play();

                // Fade in the new audio clip after a delay
                musicSource.DOFade(1f, .5f);
            });

        /*
        if (musicSource.isPlaying)
        {
            musicSource.DOFade(.5f, .5f).OnComplete(()=>{
                musicSource.clip = _soundByName;
                musicSource.DOFade(1f, .5f);

            });
        } else
        {
            musicSource.clip = _soundByName;
        }
        */
        //musicSource.Play();
    }


    [Button]
    private void GenerateSoundPool(int size = 50)
    {
        availableSources = new List<AudioSource>();

        //destroy children
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            DestroyImmediate(child);
        }

        GameObject emptyObject = new GameObject();

        //regular sfx
        for (int i = 0; i<size; i++)
        {
            GameObject _g = Instantiate(emptyObject, transform);
            availableSources.Add(_g.AddComponent<AudioSource>());
            _g.GetComponent<AudioSource>().outputAudioMixerGroup = sfxMixerGroup; 
            _g.GetComponent<AudioSource>().playOnAwake = false;
            _g.name = "sfx";
        }

        //music
        GameObject _m = Instantiate(emptyObject, transform);
        musicSource = _m.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.outputAudioMixerGroup = musicMixerGroup;
        //musicSource.playOnAwake = false;
        _m.name = "music";

        DestroyImmediate(emptyObject);
    }


#if UNITY_EDITOR // uses AssetDatabase
    [Button(ButtonSizes.Small)]
    private void LoadClips()
    {
        clips = new List<AudioClip>();
        string[] filePaths = Directory.GetFiles(Application.dataPath, soundFolderPath, SearchOption.AllDirectories);

        if (filePaths.Length > 0)
        {
            foreach(string _p in filePaths)
            {
                if (!IsFileMeta(_p))
                {
                    TryWrite(FileName(_p)+" File found at path: " + _p);
                    clips.Add(AssetDatabase.LoadAssetAtPath<AudioClip>(PathGlobalToLocal(_p)));
                }
                else {
                    //its a meta file
                }
            }
        }
        else
        {
            TryWrite("File not found.");
        }

        WriteListToDictionary();

    }
#endif
    //Utils
    int sourceCounter = 0;
    private AudioSource FindAvailableSource()
    {
        for(int i = sourceCounter; i<availableSources.Count; i++)
        {
            //index forward
            sourceCounter++;
            if (sourceCounter >= availableSources.Count)
            {
                sourceCounter = 0;
            }

            //if current is not playing return it
            if (!availableSources[sourceCounter].isPlaying)
            {
                return availableSources[sourceCounter];
            }
        }

        return null;
    }
    private void WriteListToDictionary()
    {
        soundDictionary = clips.ToDictionary(ac => ac.name, ac => ac);

        foreach (var kvp in soundDictionary)
        {
            TryWrite($"Key: {kvp.Key}, Value: {kvp.Value}");
        }
    }
    private bool IsFileMeta(string filePath)
    {
        string extension = ".meta";
        bool hasMetaExtension = filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
        return hasMetaExtension;
    }
    private string FileName(string filePath)
    {
        string[] pathComponents = filePath.Split('/', '\\');
        string lastPart = pathComponents[pathComponents.Length - 1];
        return lastPart;
    }
    private string PathGlobalToLocal(string filePath)
    {
        string keyword = "Assets";
        int startIndex = filePath.IndexOf(keyword);

        if (startIndex != -1)
        {
            return filePath.Substring(startIndex);
        }
        else
        {
            Debug.LogError("Could not find the 'Assets' keyword in the file path.");
            return string.Empty;
        }
    }
    private void TryWrite(string _m)
    {
        if (!muteDebug)
        {
            Debug.Log(_m);
        }
    }

    
}