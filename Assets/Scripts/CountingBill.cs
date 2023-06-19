using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountingBill : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text HighScoreText;



    private HeroKnight hero;
    private int highScore;
    private new AudioSource audio;
    private bool playSound;
    
    private void Awake()
    {
        GameObject ob = GameObject.Find("Hero");
        hero = ob.GetComponent<HeroKnight>();
        audio = ob.GetComponentInChildren<AudioSource>();

        if (PlayerPrefs.HasKey("HighScore"))
            highScore = PlayerPrefs.GetInt("HighScore");
        else
            highScore = 0;
        
        if (PlayerPrefs.HasKey("MusicPlay"))
            playSound = (PlayerPrefs.GetInt("MusicPlay") == 1);
        else
        {
            playSound = false;
            PlayerPrefs.SetInt("MusicPlay", (playSound ? 1 : 0));
        }

        audio.mute = playSound;
        
    }

    void Start()
    {
    }

    void Update()
    {
        if (hero.score > highScore)
        {
            highScore = hero.score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

       scoreText.text = "Score: " + hero.score;
       HighScoreText.text = "High Score: " + highScore;

    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(1);
    }


}
