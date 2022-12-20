using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] AudioClip accept, back, main;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        AudioSource mainSource = FindObjectOfType<InfoEntreEscenas>().GetComponent<AudioSource>();
        mainSource.clip = main;
        mainSource.Play();
    }

    public void playAccept()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = accept;
        audioSource.Play();
    }

    public void playGoBack()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = back;
        audioSource.Play();
    }
}
