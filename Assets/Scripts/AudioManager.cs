using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private AudioSource audio;
    private bool  playSound;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("MusicPlay"))
            playSound = (PlayerPrefs.GetInt("MusicPlay") == 1);
        else
        {
            playSound = false;
            PlayerPrefs.SetInt("MusicPlay", (playSound ? 1: 0));
        }

        audio.mute = playSound;
    }

}
