using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundEffectManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] clips;
    [Range(0, 1)]
    public float volume = 0.5f;
    [SerializeField]
    bool changePitch;
    [SerializeField]
    bool makeSoundsRandomly;
    [SerializeField]
    bool disallowRepeats = true;
    public bool loopSounds;
    [SerializeField]
    float loopWaitTime;
    [SerializeField]
    int randomChance = 100;
    
    AudioSource source;
    AudioClip lastPlayed;
    
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    void FixedUpdate()
    {
        if (loopSounds)
        {
            if (source != null && !source.isPlaying && Time.time % loopWaitTime == 0)
            {
                Play();
            }
        }
        else
        {
            if (makeSoundsRandomly && Random.Range(0, randomChance) == 0 && source != null && !source.isPlaying)
            {
                Play();
            }   
        }
    }

    public void Play(AudioClip clip)
    {
        source.clip = clip;
        source.volume = volume;

        if (changePitch)
        {
            source.pitch = 1 + Random.Range(-0.5f, 0.5f);
        }

        source.Play();
        
        lastPlayed = source.clip;
    }
    
    public void Play()
    {
        AudioClip temp = clips[Random.Range(0, clips.Length)];

        if (disallowRepeats && clips.Length > 1 && lastPlayed != null)
        {
            if (clips.Length == 2)
            {
                if (clips[0].Equals(lastPlayed))
                {
                    temp = clips[1];
                }
                else
                {
                    temp = clips[0];
                }
            }
            else
            {
                while (source.clip.Equals(lastPlayed))
                {
                    temp = clips[Random.Range(0, clips.Length)];
                }   
            }
        }
        
        Play(temp);
    }
}