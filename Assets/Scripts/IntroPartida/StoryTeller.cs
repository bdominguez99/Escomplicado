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
        [TextArea(6, 15)] public string paragraph;
    }

    [SerializeField] private float lettersDelay = 0.05f, fadeTime = 1f, steptime = 0.5f;
    [SerializeField] private Text storyText;
    [SerializeField] private Image topImage, storyImage;
    [SerializeField] private GameObject loadingScreen, top;
    [SerializeField] private StoryPage[] pages;

    private int pageIndex = 0;
    private bool canSkipPage, canSkipText, isFading;

    private void Start()
    {
        StartCoroutine(introAnimation());
    }

    void Update()
    {
        if (canSkipPage && Input.GetMouseButtonDown(0) && pageIndex < pages.Length - 1)
        {
            StartCoroutine(nextPage());
        }
        else if (canSkipPage && Input.GetMouseButtonDown(0) && pageIndex == pages.Length - 1)
        {
            StartCoroutine(outroAnimation());
        }
        else if(!canSkipPage && !isFading && Input.GetMouseButtonDown(0))
        {
            canSkipText = true;
        }
    }

    public void skip()
    {
        StopAllCoroutines();
        canSkipPage = false;
        isFading = true;
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("Main");
    }

    private IEnumerator nextPage()
    {
        pageIndex++;
        canSkipPage = false;
        if (pages[pageIndex].sprite != null)
        {
            isFading = true;
            top.SetActive(true);
            float progress = 0f, relativeTime = steptime / 2;
            while (progress < relativeTime)
            {
                topImage.color = new Color(1, 1, 1, progress / relativeTime);
                progress += Time.deltaTime;
                yield return null;
            }
            progress = relativeTime;
            topImage.color = new Color(1, 1, 1, 1);
            storyText.text = "";
            storyImage.sprite = pages[pageIndex].sprite;
            yield return new WaitForSeconds(0.05f);
            while (progress > 0)
            {
                topImage.color = new Color(1, 1, 1, progress / relativeTime);
                progress -= Time.deltaTime;
                yield return null;
            }
            topImage.color = new Color(1, 1, 1, 0);
            top.SetActive(false);
            isFading = false;
        }
        StartCoroutine(setText());
    }

    private IEnumerator setText()
    {
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
        top.SetActive(false);
        StartCoroutine(setText());
    }

    private IEnumerator outroAnimation()
    {
        float progress = 0f;
        top.SetActive(true);
        while (progress < fadeTime)
        {
            topImage.color = new Color(1, 1, 1, progress / fadeTime);
            progress += Time.deltaTime;
            yield return null;
        }
        topImage.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("Main");
    }
}
