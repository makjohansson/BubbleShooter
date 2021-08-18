using UnityEngine;
/// <summary>
/// MonoBehaviour class used to control the Main menu
/// </summary>
public class MainMenuController : MonoBehaviour
{
    public GameObject HighscoreTable;
    public GameObject BackgroundDark;
    public GameObject helper;
    public LevelLoader levelLoader;
    
    public void PlayGameSoft()
    {
        levelLoader.LoadNextLevel(1);
    }
    
    public void PlayGameHardcore()
    {
        levelLoader.LoadNextLevel(4);
    }

    /// <summary>
    /// Activate the highscore list
    /// </summary>
    public void HighScore()
    {
        BackgroundDark.SetActive(true);
        HighscoreTable.SetActive(true);
    }

    /// <summary>
    /// Deactivate the highscore list
    /// </summary>
    public void BackFromHighscore()
    {
        HighscoreTable.SetActive(false);
        BackgroundDark.SetActive(false);
    }

    /// <summary>
    /// Activate the instruction panel
    /// </summary>
    public void Helper()
    {
        BackgroundDark.SetActive(true);
        helper.SetActive(true);
    }

    /// <summary>
    /// Deactivate the instruction panel
    /// </summary>
    public void BackFromHelper()
    {
        BackgroundDark.SetActive(false);
        helper.SetActive(false);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false; // Quit in Editor 
        Application.Quit(); // Quit when built 
    }
}
