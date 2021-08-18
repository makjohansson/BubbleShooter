using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// MonoBehaviour class used to control the pause menu
/// </summary>
[RequireComponent(typeof(Toggle))]
public class PauseToggle : MonoBehaviour
{
    private bool paused = false;
    private Toggle pauseToggle;
    private AudioManager audioManager;
    public GameObject pauseMenuDark;
    public GameObject pauseMenu;
    public GameObject helper;
    private GameController gameController;

    private void Start()
    {
        pauseToggle = GetComponent<Toggle>();
        gameController = FindObjectOfType<GameController>();
        if (Time.timeScale == 0) return;
        pauseToggle.isOn = false;
        MenuSwitch(false);
        audioManager = AudioManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) & !gameController.IsGameOver())
        {
            if (!paused)
                PauseMenuActivator(true);
            else    
                PauseMenuActivator(false);
        }
        if (Time.timeScale == 0) return;
        pauseToggle.isOn = false;
    }

    /// <summary>
    /// Activates the pause menu if pause button is pressed
    /// </summary>
    /// <param name="pausePressed"></param>
    public void PauseMenuActivator(bool pausePressed)
    {
        if (pausePressed)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    private void Pause()
    {
        MenuSwitch(true);
        paused = true;
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void Resume()
    {
        MenuSwitch(false);
        paused = false;
    }

    /// <summary>
    /// Activate the instruction panel
    /// </summary>
    public void Help()
    {
        helper.SetActive(true);
    }

    /// <summary>
    /// Deactivate the instruction panel
    /// </summary>
    public void Back()
    {
        helper.SetActive(false);
    }

    /// <summary>
    /// Load main menu
    /// </summary>
    public void LoadMenu()
    {
        
        MenuSwitch(false);
        FindObjectOfType<GameController>().GoToMainMenu();
    }

    /// <summary>
    /// If true pause menu is presented
    /// </summary>
    /// <param name="isOn"></param>
    private void MenuSwitch(bool isOn)
    {
        if (isOn)
        {
            pauseMenuDark.SetActive(true);
            pauseMenu.SetActive(true);
            pauseToggle.isOn = true;
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuDark.SetActive(false);
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            pauseToggle.isOn = false;
        }
    }

    public void SetMasterVolume(float vol)
    {
        audioManager.SetMasterVolume(vol);
    }

    public void SetMusicVolume(float vol)
    {
        audioManager.SetMusicVolume(vol);
    }

    public void SetSFXVolume(float vol)
    {
        audioManager.SetSFXVolume(vol);
    }
}
