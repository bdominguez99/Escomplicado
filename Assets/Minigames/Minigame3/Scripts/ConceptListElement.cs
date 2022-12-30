using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConceptListElement : MonoBehaviour
{
    [SerializeField] private GameObject m_background;
    [SerializeField] private Text m_elementText;
    [SerializeField] private GameObject m_sprite;
    [SerializeField] private Color m_backgroundColor;

    public int IdElement { get; private set; }

    public bool IsSelected { get; set; }


    public void Init(int idElement, bool isSelected, string text, Sprite sprite)
    {
        IdElement = idElement;
        IsSelected = isSelected;
        m_elementText.text = text;
        m_sprite.GetComponent<Image>().sprite = sprite;
    }

    private void OnMouseUpAsButton()
    {
        SelectElement();
    }

    public void SelectElement()
    {
        FindObjectOfType<AudioPlayer>().PlayButtonSound();
        var popUpController = FindObjectOfType<PopUpController>();
        if (popUpController != null)
        {
            popUpController.UpdateSelectedConcept(IdElement);
        }
        MarkAsSelected();
    }

    public void MarkAsSelected()
    {
        m_background.GetComponent<Image>().color = m_backgroundColor;
    }

    public void UnmarkAsSelected()
    {
        m_background.GetComponent<Image>().color = new Color(0,0,0,0);
    }
}
