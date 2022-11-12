using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class PreviewScreen : MonoBehaviour
    {
        [SerializeField] private float fadeTime = 1.5f, showingTime = 1.5f;
        [SerializeField] private GameObject nextScreen;

        private Image image;

        private void Start()
        {
            image = GetComponent<Image>();
            StartCoroutine(selfDeactivate());
        }

        private IEnumerator selfDeactivate()
        {
            float progress = 0f;
            while (progress < fadeTime)
            {
                progress += Time.deltaTime;
                image.color = new Color(1, 1, 1, progress / fadeTime);
                yield return null;
            }
            image.color = Color.white;
            yield return new WaitForSeconds(showingTime);
            progress = fadeTime;
            nextScreen.SetActive(true);
            while (progress > 0)
            {
                progress -= Time.deltaTime;
                image.color = new Color(1, 1, 1, progress / fadeTime);
                yield return null;
            }
            image.color = new Color(1, 1, 1, 0);
            gameObject.SetActive(false);
        }
    }
}