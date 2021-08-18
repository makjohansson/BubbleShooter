using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for the highscore entry
/// </summary>
public class HighscoreEntryForm : MonoBehaviour
{
    public GameObject inputField;
    public GameController gameController;
    private new string name;
    
    /// <summary>
    /// Save player name to higscore list
    /// </summary>
    public void SaveToHighscore()
    {
        name = inputField.GetComponent<Text>().text;
        gameController.HighscoreName(name);
    }

    /// <summary>
    /// Return to main menu
    /// </summary>
    public void GoToMainMenu()
    {
        gameController.GoToMainMenu();
    }
}
