using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] AudioClip accept, back, main;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip walk;
    [SerializeField] AudioClip door;
    [SerializeField] AudioClip climbStairs;

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

    public void playDoor()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = door;
        audioSource.Play();
    }

    public void playClimbStairs()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = climbStairs;
        audioSource.Play();
    }

    public void playWalk()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = walk;
        audioSource.Play();
    }

    public void stopPlaying(bool isStopWalkSignal = false)
    {
        audioSource.loop = false;
        if (audioSource.isPlaying) audioSource.Stop();
    }
}
