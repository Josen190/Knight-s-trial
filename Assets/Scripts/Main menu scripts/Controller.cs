using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    [SerializeField] private Button start;
    [SerializeField] private Text HighScoreText;
    [SerializeField] private Sprite musicON;
    [SerializeField] private Sprite musicOFF;
    [SerializeField] private Button music;
    [SerializeField] private AudioSource audio;
    private bool playSound;

    private int highScore;
    private int sceneID = 1;


    private void Awake()
    {
        audio =  GameObject.Find("Main Camera").GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("HighScore"))
            highScore = PlayerPrefs.GetInt("HighScore");
        else
            highScore = 0;

        HighScoreText.text = "High Score: " + highScore;

        if (PlayerPrefs.HasKey("MusicPlay"))
            playSound = (PlayerPrefs.GetInt("MusicPlay") == 1);
        else
        {
            playSound = false;
            PlayerPrefs.SetInt("MusicPlay", (playSound ? 1 : 0));
        }

        music.image.sprite = playSound ? musicON : musicOFF;
        audio.mute = playSound;
    }

    public void StartLevel()
    {
        Scene activScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneID);
        SceneManager.UnloadSceneAsync(activScene);
    }

    public void MusicPlay()
    {
        playSound = !playSound;
        PlayerPrefs.SetInt("MusicPlay", (playSound ? 1 : 0));
        music.image.sprite = playSound ? musicON : musicOFF;
        audio.mute = playSound;
    }
    public void Exit()
    {
        Application.Quit();
    }
}
