using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryTeller : MonoBehaviour
{
    [System.Serializable]
    private class StoryPage
    {
        public Sprite sprite;
        [TextArea] public string paragraph;
    }

    [SerializeField] private float lettersDelay = 0.05f, fadeTime = 1f;
    [SerializeField] private Text storyText;
    [SerializeField] private Image topImage, storyImage;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private StoryPage[] pages;

    private int pageIndex = 0;
    private bool canSkipPage, canSkipText;

    private void Start()
    {
        StartCoroutine(introAnimation());
    }

    void Update()
    {
        if (canSkipPage && Input.GetMouseButtonDown(0) && pageIndex < pages.Length - 1)
        {
            nextPage();
        }
        else if (canSkipPage && Input.GetMouseButtonDown(0) && pageIndex == pages.Length - 1)
        {
            StartCoroutine(outroAnimation());
        }
        else if(!canSkipPage && Input.GetMouseButtonDown(0))
        {
            canSkipText = true;
        }
    }

    private void nextPage()
    {
        pageIndex++;
        if (pages[pageIndex].sprite != null)
        {
            storyImage.sprite = pages[pageIndex].sprite;
        }
        StartCoroutine(setText());
    }

    private IEnumerator setText()
    {
        canSkipPage = false;
        string paragraph = pages[pageIndex].paragraph;
        storyText.text = "";
        foreach (var letter in paragraph)
        {
            if (canSkipText)
            {
                storyText.text = paragraph;
                break;
            }
            storyText.text += letter;
            yield return new WaitForSeconds(lettersDelay);
        }
        canSkipPage = true;
        canSkipText = false;
    }

    private IEnumerator introAnimation()
    {
        float progress = 0f;
        while (progress < fadeTime)
        {
            topImage.color = new Color(1, 1, 1, (fadeTime - progress) / fadeTime);
            progress += Time.deltaTime;
            yield return null;
        }
        topImage.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(setText());
    }

    private IEnumerator outroAnimation()
    {
        float progress = 0f;
        while (progress < fadeTime)
        {
            topImage.color = new Color(1, 1, 1, progress / fadeTime);
            progress += Time.deltaTime;
            yield return null;
        }
        topImage.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync("Main");
    }
}
