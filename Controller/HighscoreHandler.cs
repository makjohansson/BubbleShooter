using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the saving and reading to higscorelist from the PlayerPrefs
/// </summary>
public class HighscoreHandler
{
    private Highscores highscores;
    
    public HighscoreHandler()
    {
        highscores = new Highscores();
    }

    /// <summary>
    /// Add highscore to PlayerPrefs
    /// </summary>
    /// <param name="name"></param>
    /// <param name="score"></param>
    public void AddHighscoreEntry(string name, int score)
    {
        
        var highscoreEntry = new HighscoreEntry { score = score, name = name};
        

        highscores = !PlayerPrefs.GetString("highscoretable").Equals("") ? ReadHighscores()
            : new Highscores {highscoreEntryList = new List<HighscoreEntry>()};
        
        highscores.highscoreEntryList.Add(highscoreEntry);
        
        // Save updated highscore
        var json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoretable", json);
        PlayerPrefs.Save();
        Debug.Log("Score: " + score + " and name: " + name + " was saved!!");
    }

    /// <summary>
    /// Read the highscore from the playerPrefs
    /// </summary>
    /// <returns></returns>
    public Highscores ReadHighscores()
    {
        var jsonString = PlayerPrefs.GetString("highscoretable");
        return JsonUtility.FromJson<Highscores>(jsonString);
        
    }
}
