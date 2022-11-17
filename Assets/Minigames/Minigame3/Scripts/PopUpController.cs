using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    [SerializeField] private GameObject m_conceptsList;
    [SerializeField] private GameObject m_definitionsList;
    [SerializeField] private GameObject m_conceptElementPrefab;
    [SerializeField] private GameObject m_definitionListPrefab;

    private List<GameObject> m_conceptListElements;
    private List<GameObject> m_definitionListElements;

    public int m_idLastSelectedConcept { get; set; } = -1;

    private void Start()
    {
        m_conceptListElements = new List<GameObject>();
        m_definitionListElements = new List<GameObject>();
    }

    public void UpdateSelectedConcept(int idElement)
    {
        foreach (var conceptElement in m_conceptListElements)
        {
            var conceptListElement = conceptElement.GetComponent<ConceptListElement>();
            if (conceptListElement.IdElement == m_idLastSelectedConcept)
            {
                conceptListElement.UnmarkAsSelected();
                break;
            }
        }
        m_idLastSelectedConcept = idElement;
        FindObjectOfType<GameController>().UpdateRockType(idElement);
    }

    public void AddConceptElement(string concept, int idElement, Sprite rockSprite)
    {
        var newConceptElement = Instantiate(m_conceptElementPrefab, m_conceptsList.transform);
        newConceptElement.transform.localScale = new Vector3(1, 1, 1);
        newConceptElement.GetComponent<ConceptListElement>().Init(idElement, false, concept, rockSprite);
        m_conceptListElements.Add(newConceptElement);
    }

    public void AddDefinitionElement(string definition, int idElement, Sprite demonSprite)
    {
        var newConceptElement = Instantiate(m_definitionListPrefab, m_definitionsList.transform);
        newConceptElement.transform.localScale = new Vector3(1, 1, 1);
        newConceptElement.GetComponent<DefinitionListElement>().Init(definition, demonSprite);
        m_definitionListElements.Add(newConceptElement);
    }
}
