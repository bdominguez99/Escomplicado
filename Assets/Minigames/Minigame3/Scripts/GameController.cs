using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject m_demonPrefab;
    [SerializeField] private GameObject m_initDemonAreaObject;
    [SerializeField] private GameObject m_endDemonAreaObject;
    [SerializeField] private GameObject m_popUpControllerGameObject;
    [SerializeField] private GameObject m_loadingScreen;
    [SerializeField] private GameObject m_gameOverScreen;
    [SerializeField] private Text m_finalScoreText;
    [SerializeField] private int m_mistakePenalty;
    [SerializeField] private int m_timeLimit;
    [SerializeField] private int m_questionsPerPhase;
    [SerializeField] private int m_demonMovementFrequency;
    [SerializeField] private int m_maxQuestions;
    [SerializeField] private List<Sprite> m_rockSprites;
    [SerializeField] private List<Sprite> m_demonSprites;

    public int DemonMovementFrequency { get => m_demonMovementFrequency; }

    public bool EnableCameraMovement { get; set; } = true;

    public bool IsPopUpActive { get => m_popUpControllerGameObject.activeInHierarchy; }

    private List<List<RelationQuestion>> m_questions;
    private List<RelationQuestion> m_currentPhaseQuestions;
    private int m_currentPhase;
    private int m_totalPhases;
    private int m_correctQuestions;
    private int m_phaseQuestionsLeft;
    private int m_totalQuestions;

    private DataManager m_dataManager;
    private PopUpController m_popUpController;

    async void Start()
    {
        //m_dataManager = FindObjectOfType<CargadorDeDB>().DataManager;
        m_currentPhase = 0;
        m_loadingScreen.SetActive(true);
        InitClock();
        await GetQuestions();
        ConfigureGame();
    }

    private void InitClock()
    {
        var clock = FindObjectOfType<Clock>();
        clock.Penalty = m_mistakePenalty;
        clock.TimeLimit = m_timeLimit;
    }

    private async Task GetQuestions()
    {
        m_dataManager = new DataManager();
        List<RelationQuestion> questions = await m_dataManager.GetRelationQuestionsAsync("Diseño de Sistemas Digitales");
        
        m_totalQuestions = Mathf.Min(questions.Count, m_maxQuestions);

        m_totalPhases = m_totalQuestions / m_questionsPerPhase + (m_totalQuestions % m_questionsPerPhase != 0 ? 1 : 0);
        m_questions = new List<List<RelationQuestion>>();

        for (int i = 0; i < m_totalPhases; i++)
        {
            var phaseList = new List<RelationQuestion>();
            for (int j = 0; j < m_questionsPerPhase; j++)
            {
                if (i* m_questionsPerPhase + j >= m_totalQuestions)
                {
                    break;
                }
                phaseList.Add(questions[i * m_questionsPerPhase + j]);
            }
            m_questions.Add(phaseList);
        }
    }

    public void NextPhase()
    {
        m_currentPhase++;
        if (m_currentPhase >= m_totalPhases)
        {
            FinishGame();
        }
        else
        {
            ConfigureGame();
        }

    }

    public void UpdateStateCorrectAnswer()
    {
        m_phaseQuestionsLeft--;
        m_correctQuestions++;
        if (m_phaseQuestionsLeft == 0)
        {
            NextPhase();
        }
    }

    public void UpdateStateWrongAnswer()
    {
        m_phaseQuestionsLeft--;
        if (m_phaseQuestionsLeft == 0)
        {
            NextPhase();
        }
    }

    private void ConfigureGame()
    {
        m_loadingScreen.SetActive(true);
        m_currentPhaseQuestions = m_questions[m_currentPhase];
        m_phaseQuestionsLeft = m_currentPhaseQuestions.Count;
        m_popUpController = m_popUpControllerGameObject.GetComponent<PopUpController>();

        var initDemonArea = m_initDemonAreaObject.transform.position;
        var endDemonArea = m_endDemonAreaObject.transform.position;

        List<Pair<int, string>> concepts = new List<Pair<int, string>>();
        List<Pair<int,string>> definitions = new List<Pair<int,string>>();

        for (int i = 0; i < m_currentPhaseQuestions.Count; i++)
        {
            concepts.Add(new Pair<int, string>(i, m_currentPhaseQuestions[i].Concept));
            definitions.Add(new Pair<int, string>(i, m_currentPhaseQuestions[i].Definition));
        }

        ExtensionMethods.Shuffle<Pair<int, string>>(concepts);
        ExtensionMethods.Shuffle<Pair<int, string>>(definitions);
        ExtensionMethods.Shuffle<Sprite>(m_rockSprites);

        for (int i = 0; i < m_currentPhaseQuestions.Count; i++)
        {
            var demon = Instantiate(m_demonPrefab);
            var demonObject = demon.GetComponent<Demon>();
            var xCoordinate = Random.Range(initDemonArea.x, endDemonArea.x);
            var yCoordinate = Random.Range(initDemonArea.y, endDemonArea.y);
            var demonPosition = new Vector2(xCoordinate, yCoordinate);
            demonObject.transform.position = demonPosition;
            demonObject.Init(definitions[i].First, initDemonArea, endDemonArea);

            m_popUpController.AddConceptElement(concepts[i].Second, concepts[i].First, m_rockSprites[concepts[i].First]);
            m_popUpController.AddDefinitionElement(definitions[i].Second, definitions[i].First, m_demonSprites[i]);
        }

        var firstElement = m_popUpController.GetConceptElement(0);
        firstElement.SelectElement();

        m_popUpControllerGameObject.SetActive(false);
        m_loadingScreen.SetActive(false);
    }

    public void UpdateRockType(int idElement)
    {
        var tommy = FindObjectOfType<Tommy>();
        Debug.Log("IdElement: " + idElement);
        tommy.UpdateRockType(idElement, m_rockSprites[idElement]);
    }

    public void FinishGame()
    {
        m_finalScoreText.text = "Puntuacion: " + m_correctQuestions + "/" + m_totalQuestions;
        m_gameOverScreen.SetActive(true);
    }

    public void RestartMinigame()
    {
        m_gameOverScreen.SetActive(false);
        m_loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("AngryKids");
    }

    public void FinishMinigame()
    {
        FindObjectOfType<Minijuego>().setScore(((float)m_correctQuestions / m_totalQuestions) * 10f);
        m_gameOverScreen.SetActive(false);
        m_loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("Main");
    }
}
