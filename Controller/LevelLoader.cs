using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to load each level and delay each transition by a given time
/// </summary>
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime;
    

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    /// <summary>
    /// 0 to load main menu, 1 to play relax mode, 2 and 3 is also relax mode. 4, 5 and 6 is hardcore mode
    /// </summary>
    /// <param name="level"></param>
    public void LoadNextLevel(int level)
    {
        var audioManager = AudioManager.instance;
        switch (level)
        {
            case 0:
                Time.timeScale = 1;
                PlayerPrefs.SetInt("Score", 0);
                PlayerPrefs.Save();
                audioManager.set = false;
                audioManager.SetScene(0);
                StartCoroutine(LoadLevel(0));
                break;
            case 1:
                audioManager.set = false;
                audioManager.SetScene(1);
                audioManager.SetPrevScene(0);
                StartCoroutine(LoadLevel(1));
                break;
            case 2:
                StartCoroutine(LoadLevel(2));
                break;
            case 3:
                StartCoroutine(LoadLevel(3));
                break;
            case 4:
                audioManager.set = false;
                audioManager.SetScene(2);
                audioManager.SetPrevScene(1);
                StartCoroutine(LoadLevel(4));
                break;
            case 5:
                StartCoroutine(LoadLevel(5));
                break;
            case 6:
                StartCoroutine(LoadLevel(6));
                break;
            default:
                StartCoroutine(LoadLevel(0));
                break;
        }
    }
}
