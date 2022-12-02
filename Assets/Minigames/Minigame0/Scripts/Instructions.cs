using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    [Header("Instructions")]
    [SerializeField] private Sprite[] images;
    [TextArea(5, 100)][SerializeField] private string[] instructions;
    [SerializeField] private Image instructionsImage;
    [SerializeField] private Text instructionsText;
    [SerializeField] private GameObject instructionsScreen;
    [Header("Ready screen")]
    [SerializeField] private GameObject readyScreen;
    [SerializeField] private Text readyText;
    [SerializeField] private string goText;
    [SerializeField] private float readyTime = 0.5f, goTime = 0.2f;
    [SerializeField] private GameObject[] objectsToEnableAfterInstructions;

    private int actualInstruction;

    private void Start()
    {
        if(images == null || instructions == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instructionsImage.sprite = images[0];
            instructionsText.text = instructions[0];
        }
    }

    public void skip()
    {
        actualInstruction = Mathf.Min(instructions.Length - 1, images.Length - 1);
        StartCoroutine(readyAnim());
    }

    public void nextInstruction()
    {
        if (actualInstruction < (instructions.Length - 1) && actualInstruction < (images.Length - 1))
        {
            actualInstruction++;
            instructionsImage.sprite = images[actualInstruction];
            instructionsText.text = instructions[actualInstruction];
        }
        else
        {
            StartCoroutine(readyAnim());
        }
    }

    public void prevInstruction()
    {
        if (actualInstruction > 0)
        {
            actualInstruction--;
            instructionsImage.sprite = images[actualInstruction];
            instructionsText.text = instructions[actualInstruction];
        }
    }

    private IEnumerator readyAnim()
    {
        instructionsScreen.SetActive(false);
        readyScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(readyTime);
        readyText.text = goText;
        yield return new WaitForSecondsRealtime(goTime);
        foreach (var item in objectsToEnableAfterInstructions)
        {
            item.SetActive(true);
        }
        readyScreen.SetActive(false);
    }
}