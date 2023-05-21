using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_BACKGROUND_MUSIC_VOLUME = "BackgroundMusicVolume";
    
    public static BackgroundMusicManager Instance { get; private set;  }
    
    private AudioSource audioSource;
    private float volume = .3f;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_BACKGROUND_MUSIC_VOLUME, .3f);
        // This audioSource starts playing right away, so we need to set it's volume immediately on awake
        audioSource.volume = volume;
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }

        audioSource.volume = volume;
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_BACKGROUND_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
