using System.Collections;
using UnityEngine;


public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_demonDeath;
    [SerializeField] private AudioClip m_openPopUp;
    [SerializeField] private AudioClip m_wrongAnswer;
    [SerializeField] private AudioClip m_buttonSound;

    private AudioClip m_selectedSound;

    private void PlaySound()
    {
        if (m_selectedSound == null)
        {
            return;
        }

        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }

        m_audioSource.clip = m_selectedSound;
        m_audioSource.Play();
    }

    public void PlayDemonDeathSound()
    {
        m_selectedSound = m_demonDeath;
        PlaySound();
    }

    public void PlayWrongAnswerSound()
    {
        m_selectedSound = m_wrongAnswer;
        PlaySound();
    }

    public void PlayOpenPopUpSound()
    {
        m_selectedSound = m_openPopUp;
        PlaySound();
    }

    public void PlayButtonSound()
    {
        m_selectedSound = m_buttonSound;
        PlaySound();
    }
}