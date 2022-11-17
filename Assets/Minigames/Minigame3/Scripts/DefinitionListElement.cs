using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefinitionListElement : MonoBehaviour
{
    [SerializeField] private Text m_elementText;
    [SerializeField] private GameObject m_sprite;

    public void Init(string definition, Sprite demonSprite)
    {
        m_elementText.text = definition;
        m_sprite.GetComponent<Image>().sprite = demonSprite;
    }
}
