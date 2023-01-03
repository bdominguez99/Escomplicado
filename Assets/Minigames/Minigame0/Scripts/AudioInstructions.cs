using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstructions : MonoBehaviour
{
    [SerializeField] private AudioClip go, back;
    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void playGoSound()
    {
        audio.Stop();
        audio.clip = go;
        audio.Play();
    }

    public void playBackSound()
    {
        audio.Stop();
        audio.clip = back;
        audio.Play();
    }
}
