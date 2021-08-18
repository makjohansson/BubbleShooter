using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The main controller of the game. 
/// </summary>
public class GameController : MonoBehaviour
{

    public Text playerScoreText;
    public Text aiScoreText;
    public LevelLoader levelLoader;
    
    private const string playerScoreString = "Score: ";
    public string aiScoreString;
    private const int playerScoreZeroSoft = 1;
    private const int playerScoreZeroHard = 4;
    
    private int playerScore = 0;
    public int maxAiHits = 5;
    public int time = 180; // 180 = 3 minute
    private bool gameOver;
    private bool sceneChange;
    private bool backToMenu;
    private float SFXVolume;
    private TimeController timeController;
    private BubbleController bubbleController;
    public GameObject highscoreEntryForm;
    public GameObject victory;
    private bool stopUpdate;
    private bool completed;
    private bool isAdded;
    private AudioManager audioManager;
    private const int lastRelaxLevel = 3;
    private const int lastHardcoreLevel = 6;


    private void Start()
    {
        
        InvokeRepeating(nameof(Count), 0.0f, 1.0f);
        gameOver = false;
        sceneChange = false;
        completed = false;
        isAdded = false;
        bubbleController = FindObjectOfType<BubbleController>();
        SFXVolume = AudioManager.instance.GetSFXVolume();
        highscoreEntryForm.SetActive(false);
        audioManager = AudioManager.instance;
        var temp = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.GetActiveScene().buildIndex == playerScoreZeroSoft
            || SceneManager.GetActiveScene().buildIndex == playerScoreZeroHard)
        {
            playerScore = 0;
        }
        else
        {
            playerScore = PlayerPrefs.GetInt("Score");
        }
        
    }
                     

    private void Update()
    {
        if (bubbleController.BubbleOnfinalRow())
        {
            gameOver = true;
        }
        
        if (completed) return;
        playerScoreText.text = playerScoreString + playerScore;
        aiScoreText.text = aiScoreString + maxAiHits;
        if (stopUpdate)
        {
            Time.timeScale = 1;
            return;
        }

        if (sceneChange) // If true, go to next scene
            GoToNextLevel();

        if (!gameOver) return; // Return if game is not over
        GameOverActions();
        
    }

    private void GoToNextLevel()
    {
        stopUpdate = true;
        PlayerPrefs.SetInt("Score", playerScore);
        PlayerPrefs.Save();
        levelLoader.LoadNextLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Play the game over music and activate the game over panel
    private void GameOverActions()
    {
        var bgMusic = SceneManager.GetActiveScene().buildIndex  < 4 ? "BgMusicSoft" : "BgMusicHard";
        AudioManager.instance.Stop(bgMusic);
        SFXVolume = AudioManager.instance.GetSFXVolume();
        AudioManager.instance.SetSFXVolume(-80f);
        AudioManager.instance.Play("GameOver");
        Time.timeScale = 0f;
        highscoreEntryForm.SetActive(true);
        completed = true;
    }

    /// <summary>
    /// Quit ongoing game and return to main menu
    /// </summary>
    public void GoToMainMenu()
    {
        stopUpdate = true;
        AudioManager.instance.SetSFXVolume(SFXVolume);
        levelLoader.LoadNextLevel(0);
    }

    /// <summary>
    /// Return if the game is over or not
    /// </summary>
    /// <returns></returns>
    public bool IsGameOver()
    {
        return gameOver;
    }

    /// <summary>
    /// Get current time of count down timer
    /// </summary>
    /// <returns></returns>
    public int GetTime()
    {
        return time;
    }

    /// <summary>
    /// update times enemy/friend is hit
    /// </summary>
    /// <param name="timesHit"></param>
    public void CheckEnemyHits(int timesHit)
    {
        maxAiHits -= timesHit;
        if (maxAiHits <= 0)
        {
            gameOver = true;
        }
    }

    /// <summary>
    /// Update player score
    /// </summary>
    /// <param name="score"></param>
    public void UpdatePlayerScore(int score)
    {
        playerScore += score;
    }

    /// <summary>
    /// Save score to highscore list
    /// </summary>
    /// <param name="name"></param>
    public void HighscoreName(string name)
    {
        if (isAdded) return;
        new HighscoreHandler().AddHighscoreEntry(name, playerScore);
        isAdded = true;
    }

    /// <summary>
    /// Return true if the game is on level 3 or on level 6
    /// </summary>
    /// <returns></returns>
    private bool EndOfGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == lastRelaxLevel)
            return true;
        return SceneManager.GetActiveScene().buildIndex == lastHardcoreLevel;
    }

    /// <summary>
    /// Count down timer for time to play each level.
    /// If the game is on the last level it will play a victory sound.
    /// </summary>
    private void Count()
    {
        if (time == 0)
        {
            if (EndOfGame() & !completed)
            {
                var whatToPlay = SceneManager.GetActiveScene().buildIndex == lastRelaxLevel;
                if (whatToPlay)
                {
                    var bgMusic = "BgMusicSoft";
                    AudioManager.instance.Stop(bgMusic);
                    AudioManager.instance.Play("VictorySoft");
                }
                else
                {
                    var bgMusic = "BgMusicHard";
                    AudioManager.instance.Stop(bgMusic);
                    AudioManager.instance.Play("VictoryHard");
                }
                victory.SetActive(true);
                completed = true;
                gameOver = true;
                Time.timeScale = 0f;
                CancelInvoke(nameof(Count));
                playerScoreText.text = playerScoreString + playerScore;
                return;
            }
            sceneChange = true;
            CancelInvoke(nameof(Count));
        }
        else
        {
            time--;
        }
    }
}
