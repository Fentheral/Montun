using UnityEngine;
using System.Collections.Generic;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<AudioClip> sfxClips;
    private bool isPlaying = false; 
    private Dictionary<string, AudioClip> sfxDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 


        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
        {
            sfxDictionary[clip.name] = clip;
        }
    }

    private void Start()
    {
       // PlayMusic(backgroundMusic);
    }

    /* public void PlayMusic(AudioClip clip)
     {
         if (musicSource.clip == clip) return; // Evita reiniciar la misma canción

         musicSource.clip = clip;
         musicSource.loop = true;
         musicSource.Play();
     }*/

    public void PlaySFX(string clipName)
    {
        if (isPlaying)
        {
            return; 
        }

        if (sfxDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            StartCoroutine(PlaySFXWithDelay(clip));
        }
        else
        {
            Debug.LogWarning("SFX no encontrado: " + clipName);
        }
    }

    internal float GetClipLength(string clipName)
    {
        if (sfxDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            return clip.length;
        }
        else
        {
            Debug.LogWarning("SFX no encontrado: " + clipName);
            return 0f; 
        }
    }

    private IEnumerator<WaitForSeconds> PlaySFXWithDelay(AudioClip clip)
    {
        isPlaying = true; 
        sfxSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        isPlaying = false; 
    }

    /*public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }*/

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}