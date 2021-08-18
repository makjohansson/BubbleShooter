using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Setup for a sound clip
/// </summary>

[System.Serializable]
public class Sound
{
    public AudioMixerGroup audioMixerGroup;
    private AudioSource source;
    public string name;
    public AudioClip clip;
    [Range(0,1)]
    public float volume;
    public bool loop;
    
    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.outputAudioMixerGroup = audioMixerGroup;
    }

    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
