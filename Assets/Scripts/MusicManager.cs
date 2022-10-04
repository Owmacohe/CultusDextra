using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip clip;
    [Range(0, 1)]
    public float volume = 0.5f;
    
    AudioSource source;
    
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = true;
        source.clip = clip;
        source.volume = volume;
        
        source.Play();
    }
}