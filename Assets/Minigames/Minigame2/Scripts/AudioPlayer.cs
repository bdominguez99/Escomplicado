using System.Collections;
using UnityEngine;


namespace TripasDeGato
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource m_audioSource;
        [SerializeField] private AudioClip m_drawingSound;
        [SerializeField] private AudioClip m_openPopUp;
        [SerializeField] private AudioClip m_wrongAnswerSound;
        [SerializeField] private AudioClip m_correctAnswerSound;
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

        public void PlayDrawingSound()
        {
            m_selectedSound = m_drawingSound;
            PlaySound();
        }

        public void StopPlaying()
        {
            if (m_audioSource.isPlaying)
            {
                m_audioSource.Stop();
            }
        }

        public void PlayCorrectAnswerSound()
        {
            m_selectedSound = m_correctAnswerSound;
            PlaySound();
        }

        public void PlayWrongAnswerSound()
        {
            m_selectedSound = m_wrongAnswerSound;
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
}