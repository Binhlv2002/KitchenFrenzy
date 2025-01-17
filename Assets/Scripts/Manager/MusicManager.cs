﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }

    private float volume = .3f;
    private AudioSource audioSource;


    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSource.volume = volume; 
    }
    public void ChangeVolume(float newVolume)
    {
        volume = newVolume; // Cập nhật giá trị âm lượng
        audioSource.volume = volume; // Áp dụng âm lượng mới cho AudioSource
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume); // Lưu vào PlayerPrefs
        PlayerPrefs.Save(); // Lưu thay đổi
    }


    public float GetVolume()
    {
        return volume;
    }
}
