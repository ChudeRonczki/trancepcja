using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }
    
    AudioSource[] AudioSources;
    int index = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AudioSources = GetComponents<AudioSource>();
    }

    public static void Play(AudioClip clip)
    {
        if (Instance)
            Instance.PlayClip(clip);
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip == null) return;

        AudioSources[index].clip = clip;
        AudioSources[index].pitch = 1f;
        AudioSources[index].Play();
        index = (index + 1) % AudioSources.Length;
    }

    public static void Play(AudioClip clip, float minPitch, float maxPitch)
    {
        if (Instance)
            Instance.PlayClip(clip, minPitch, maxPitch);
    }

    public void PlayClip(AudioClip clip, float minPitch, float maxPitch)
    {
        if (clip == null) return;

        AudioSources[index].clip = clip;
        AudioSources[index].pitch = minPitch + (maxPitch - minPitch) * Random.value;
        AudioSources[index].Play();
        index = (index + 1) % AudioSources.Length;
    }
}