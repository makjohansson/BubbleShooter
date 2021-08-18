using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// Singleton that managers the sound in the game
/// </summary>

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;
    private int scene;
    private int prevScene;

    public Sound[] sounds;
    
    public bool set;
    private float musicVolume;
    private bool firstTime;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        
        foreach (var sound in sounds)
        {
            var soundObject = new GameObject("Sound_" + sound.name);
            soundObject.transform.SetParent(this.transform);
            sound.SetSource(soundObject.AddComponent<AudioSource>());
        }
        UnMute();
        scene = 0;
        prevScene = 0;
    }


    private void Update()
    {
        if (set) return;
        Sound s;
        switch (scene)
        {
            case 0:
                s = prevScene == 0 ? FindSound("BgMusicSoft") : FindSound("BgMusicHard") ;
                s.Stop();
                Play("BgMusicMenu");
                set = true;
                break;
            case 1:
                s = FindSound("BgMusicMenu");
                s.Stop();
                Play("BgMusicSoft");
                set = true;
                break;
            case 2:
                s = FindSound("BgMusicMenu");
                s.Stop();
                Play("BgMusicHard");
                set = true;
                break;
            default:
                Debug.Log("Oops I should not be here");
                return;
        }
    }
    
    /// <summary>
    /// Play a specific sound
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        var sound = FindSound(name);
        sound.Play();
    }

    /// <summary>
    /// Stop a specific sound
    /// </summary>
    /// <param name="name"></param>
    public void Stop(string name)
    {
        var sound = FindSound(name);
        sound.Stop();
    }

    
    /// <summary>
    /// Returns a specific sound
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private Sound FindSound(string name) 
    {
        var s = Array.Find(sounds, sound => sound.name  == name);
        if (s == null)
            throw new ArgumentNullException(nameof(s), "not found");
        return s;
    }

    /// <summary>
    /// Set to 0 if loading main menu, 1 if loading relax mode, 2 if loading hardcore mode
    /// </summary>
    /// <param name="scene"></param>
    public void SetScene(int scene)
    {
        this.scene = scene;
    }

    /// <summary>
    /// Set to 0 if loading relax mode, set to 1 if loading hard core mode
    /// </summary>
    /// <param name="prevScene"></param>
    public void SetPrevScene(int prevScene)
    {
        this.prevScene = prevScene;
    }
    
    // Unmute the audioLister used in the Awake()
    private void UnMute()
    {
        AudioListener.volume = 1;
    }

    public void SetMasterVolume(float vol)
    {
        audioMixer.SetFloat("MasterVolume", vol);
    }

    public void SetMusicVolume(float vol)
    {
        audioMixer.SetFloat("MusicVolume", vol);
    }

    public void SetSFXVolume(float vol)
    {
        audioMixer.SetFloat("SFXVolume", vol);
    }

    public float GetSFXVolume()
    {
        float sfxVolume;
        var result = audioMixer.GetFloat("SFXVolume", out sfxVolume);
        return result ? sfxVolume : 0f;
    }

    /// <summary>
    /// Play a random enemy/friend sfx. Scene number is smaller then four the friend sfx will play.
    /// If above the enemy sfx will play
    /// </summary>
    /// <param name="mode"></param>
    public void PlayRandomEnemySound(bool mode)
    {
        
        var start = mode ? 0 : 5;
        var stop = mode ? 4 : 10;
        string[] sounds =
        {
            "EnemyCommentOne", "EnemyCommentTwo", "EnemyCommentThree", "EnemyCommentFour", "EnemyCommentFive",
            "EnemyCommentSix", "EnemyCommentSeven", "EnemyCommentEight", "EnemyCommentNine", "EnemyCommentTen" 
        };
        var temp = sounds.Length;
        var enemyComment = Random.Range(start, stop);
        Play(sounds[enemyComment]);
    }
}
