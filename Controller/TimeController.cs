using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MonoBehaviour class displaying countdown timer formatted as (m:ss). 
/// </summary>
public class TimeController : MonoBehaviour
{
    public Text txtTime;
    private GameController gameController;
    private int time;
    
    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        time = gameController.GetTime();
        if (time > 10)
        {
            txtTime.text = TimeFormat();
        }
        else
        {
            txtTime.color = Color.red;
            txtTime.fontSize = 62;
            txtTime.text = time.ToString();
        }
    }
    
    /// <summary>
    /// Format time from seconds to min and seconds
    /// </summary>
    /// <returns></returns>
    private string TimeFormat()
    {
        var minute = time / 60;
        var seconds = time % 60;
        
        return seconds >= 10 ? minute.ToString() + ":" + seconds.ToString() : minute.ToString() + ":0" + seconds.ToString();
    }
}
