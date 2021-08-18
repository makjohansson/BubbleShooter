using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fill the highscore to the UI
/// </summary>
public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private Highscores highscores;
    
    private void Awake()
    {
        entryContainer = GameObject.Find("HighscoreContainer").transform;
        entryTemplate = entryContainer.Find("HighscoreTemplate");
        
        entryTemplate.gameObject.SetActive(false);
        
        highscores = new HighscoreHandler().ReadHighscores();
        if (highscores == null) return;
        var highscoreEntryList = highscores.highscoreEntryList;
        highscoreEntryList.Sort((x,y)=>y.score.CompareTo(x.score));
        var highscoreEntryTransformList = new List<Transform>();
        var numberOfHighscores = highscoreEntryList.Count <= 10 ? highscoreEntryList.Count : 10; 
        for (var i = 0; i <  numberOfHighscores; i++)
        {
            CreateHighscoreEntryTransform(highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
        }
    }

    // Fill the highscore list with names read from the PlayerPrefs
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, 
        List<Transform> transformList)
    {
        var templateHeight = 40f;
        var entryTransform = Instantiate(entryTemplate, container);
        var entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition3D = new Vector3(0, -templateHeight * transformList.Count, -2);
        entryTransform.gameObject.SetActive(true);
            
        var rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1: rankString = "1st"; break;
            case 2: rankString = "2nd"; break;
            case 3: rankString = "3th"; break;
            default:
                rankString = rank + "th"; break;
        }
        entryTransform.Find("PositionText").GetComponent<Text>().text = rankString;
        
        entryTransform.Find("ScoreText").GetComponent<Text>().text = highscoreEntry.score.ToString();
        
        entryTransform.Find("NameText").GetComponent<Text>().text = highscoreEntry.name;

        transformList.Add(entryTransform);
    }
}
