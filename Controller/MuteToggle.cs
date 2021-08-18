using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MonoBehaviour class used to mute or unmute the game music
/// </summary>
[RequireComponent(typeof(Toggle))]
public class MuteToggle : MonoBehaviour
{
    private Toggle muteToggle;
    private float savedVolume = 1;
    private GameController gameController;
    private void Start()
    {
        muteToggle = GetComponent<Toggle>();
        gameController = FindObjectOfType<GameController>();
        if (AudioListener.volume == 0)
        {
            muteToggle.isOn = false;
        }
    }

    private void Update()
    {
        if (!(Input.GetKeyDown(KeyCode.M) & !gameController.IsGameOver())) return;
        if (AudioListener.volume != 0)
            savedVolume = AudioListener.volume;
        if (muteToggle.isOn)
        {
            Mute();
        }
        else
        {
            Unmute();
        }
    }

    // Turn sound off
    private void Mute()
    {
        muteToggle.isOn = false;
        AudioListener.volume = 0;
    }

    // Turn sound back on
    private void Unmute()
    {
        muteToggle.isOn = true;
        AudioListener.volume = savedVolume;
    }
}
