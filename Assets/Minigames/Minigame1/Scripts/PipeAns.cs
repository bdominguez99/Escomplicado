using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace pipes
{
    public class PipeAns : MonoBehaviour
    {
        [SerializeField] private int answerId;
        [SerializeField] private float fillTime;
        [SerializeField] private Image fill;
        [SerializeField] private GameObject pauseButton;

        private void Start()
        {
            GetComponent<Image>().color = FindObjectOfType<PipeController>().getDisabledColor();
        }

        public void selectAnswer()
        {
            pauseButton.SetActive(false);
            StartCoroutine(startFill());
        }

        private IEnumerator startFill()
        {
            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime / fillTime;
                fill.fillAmount = progress;
                yield return null;
            }
            fill.fillAmount = 1;
            FindObjectOfType<GameController>().selectAnswer(answerId);
            fill.fillAmount = 0;
            pauseButton.SetActive(true);
        }
    }
}