using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    [Header("Music Clips")]
    public List<NamedAudioClip> musicClips = new List<NamedAudioClip>();

    [Header("Menu Music Clips")]
    public List<NamedAudioClip> menuMusicClips = new List<NamedAudioClip>();

    [Header("SFX Clips")]
    public List<NamedAudioClip> sfxClips = new List<NamedAudioClip>();

    [Header("Menu SFX Clips")]
    public List<NamedAudioClip> menuSfxClips = new List<NamedAudioClip>();

    [Header("Environment SFX Clips")]
    public List<NamedAudioClip> environmentSfxClips = new List<NamedAudioClip>();

    [Header("Character SFX Clips")]
    public List<NamedAudioClip> characterSfxClips = new List<NamedAudioClip>();
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource menuMusicSource;
    public AudioSource sfxSource;
    public AudioSource menuSfxSource;
    public AudioSource environmentSfxSource;
    public AudioSource characterSfxSource;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }




    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayMenuMusic(AudioClip clip)
    {
        menuMusicSource.clip = clip;
        menuMusicSource.loop = true;
        menuMusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMenuSFX(AudioClip clip)
    {
        menuSfxSource.PlayOneShot(clip);
    }

    public void PlayEnvironmentSFX(AudioClip clip)
    {
        environmentSfxSource.PlayOneShot(clip);
    }

    public void PlayCharacterSFX(AudioClip clip)
    {
        characterSfxSource.PlayOneShot(clip);
    }
    public void PlayLoopingEnvironmentSFX(AudioClip clip)
    {
        if (environmentSfxSource.clip == clip && environmentSfxSource.isPlaying)
            return;

        environmentSfxSource.clip = clip;
        environmentSfxSource.loop = true;
        environmentSfxSource.Play();
    }

    public void StopLoopingEnvironmentSFX()
    {
        if (environmentSfxSource.isPlaying)
            environmentSfxSource.Stop();

        environmentSfxSource.clip = null;
        environmentSfxSource.loop = false;
    }

    public AudioClip GetClipByName(string name, List<NamedAudioClip> clipList)
    {
        foreach (NamedAudioClip namedClip in clipList)
        {
            if (namedClip.name == name)
                return namedClip.clip;
        }
        Debug.LogWarning($"Audio Clip '{name}' not found in provided list!");
        return null;
    }
}