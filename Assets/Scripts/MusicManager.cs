using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    AudioSource myAudioSource;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(AudioClip music)
    {
        if (myAudioSource.isPlaying)
        {
            myAudioSource.Stop();
        }
        myAudioSource.clip = music;
        myAudioSource.Play();
    }
}
