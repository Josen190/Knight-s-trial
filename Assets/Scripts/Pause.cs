using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] public bool isPause;
    [SerializeField] private Text score;
    [SerializeField] private Button play;
    [SerializeField] private Button music;
    [SerializeField] private GameObject menu;
    [SerializeField] private Sprite musicON;
    [SerializeField] private Sprite musicOFF;
    [SerializeField] private AudioSource audio;
    public float timing = 1f;
    private bool playSound;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("MusicPlay"))
            playSound = (PlayerPrefs.GetInt("MusicPlay") == 1);
        else
        {
            playSound = false;
            PlayerPrefs.SetInt("MusicPlay", (playSound ? 1 : 0));
        }

        music.image.sprite = playSound ? musicON : musicOFF;
    }

    void Start()
    {
        timing = 1f;
        isPause = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                timing = 1f;
                menu.SetActive(false);

            } else {
                timing = 0f;
                menu.SetActive(true);
            }

            isPause = !isPause;
            Time.timeScale = timing;


        }
    }

    public void Resume()
    {
        timing = 1f;
        isPause = false;
        menu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void MusicPlay()
    {
        playSound = !playSound;
        PlayerPrefs.SetInt("MusicPlay", (playSound ? 1 : 0));
        music.image.sprite = playSound ? musicON : musicOFF;
        audio.mute = playSound;
    }

}
