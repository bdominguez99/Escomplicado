using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameAudio : MonoBehaviour
{
    [SerializeField] private AudioClip song, main;

    private AudioSource mainSource, secondaryAudioSource;

    private void Start()
    {
        mainSource = FindObjectOfType<InfoEntreEscenas>().gameObject.GetComponent<AudioSource>();
        mainSource.clip = main;
        mainSource.Play();
    }

    public void playSong()
    {
        if (mainSource.isPlaying) mainSource.Stop();
        mainSource.clip = song;
        mainSource.Play();
    }

    public void stopSong()
    {
        if (mainSource.isPlaying) mainSource.Stop();
    }

    public void playEffect(AudioClip clip)
    {
        if(secondaryAudioSource.isPlaying) secondaryAudioSource.Stop();
        secondaryAudioSource.clip = clip;
        secondaryAudioSource.Play();
    }
}
