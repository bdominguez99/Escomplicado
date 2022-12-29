using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TripasDeGato;

namespace SpaceInvaders
{
    public class GameController : MonoBehaviour
    {
        [Header("Game")]
        [SerializeField] private Text sentence;
        [SerializeField] private GameObject[] answers;
        [SerializeField] private GameObject bulletParent, loadingScreen;
        [SerializeField] private int targetAnsPoints = 10, targetQuestions = 10;

        [Header("Score screen")]
        [SerializeField] private float showScoreTime = 1f;
        [SerializeField] private Text scoreText, playingScoreText;
        [SerializeField] private GameObject levelScreen;

        [Header("Question screen")]
        [SerializeField] private Text showQuestionText;
        [SerializeField] private GameObject questionsScreen;
        [SerializeField] private float showQuestionTime = 1f, showAnswersTime = 1f;

        [Header("Game over screen")]
        [SerializeField] private Text gameOverScoreText;
        [SerializeField] private GameObject gameOverScreen, retryButton;

        private float maxSize;
        private int actualQuestion = 1, actualQuestionAns, actualScore;
        private int[] actualPoints;
        private Player player;
        private EnemyController enemyController;
        private MultiOptionQuestion question;
        private Image[] answerImages;
        private RectTransform[] answerBars;
        private Text[] answerTexts;
        private List<MultipleOptionQuestion> questionsList;

        private async void Start()
        {
            questionsScreen.SetActive(true);
            await setPreguntas();
            answerImages = new Image[answers.Length];
            answerBars = new RectTransform[answers.Length];
            answerTexts = new Text[answers.Length];
            for (int i = 0; i < answers.Length; i++)
            {
                answerImages[i] = answers[i].GetComponent<Image>();
                answerBars[i] = answers[i].GetComponentsInChildren<RectTransform>()[1];
                answerTexts[i] = answers[i].GetComponentInChildren<Text>();
            }
            player = FindObjectOfType<Player>();
            enemyController = FindObjectOfType<EnemyController>();
            maxSize = answerBars[0].sizeDelta.x;
            actualPoints = new int[answerImages.Length];
            resetAnswers();
            setQuestion();
            StartCoroutine(showFirstQuestion());
        }

        private async Task setPreguntas()
        {
            if (FindObjectOfType<InfoEntreEscenas>().EsModoLibre)
            {
                var materia = FindObjectOfType<InfoEntreEscenas>().MateriaModoLibre;
                questionsList = await FindObjectOfType<CargadorDeDB>().DataManager.GetMultipleOptionQuestions(materia);
            }
            else
            {
                questionsList = await FindObjectOfType<CargadorDeDB>().DataManager.GetMultipleOptionQuestions("Teor�a computacional");
            }
            ExtensionMethods.Shuffle(questionsList);
        }

        public void goToNextLevel(bool correctAns = false)
        {
            if (correctAns)
            {
                actualScore++;
            }
            StartCoroutine(nextLevel());
        }

        public void addPointsToAns(Enemy.EnemyColor color, int actualRow)
        {
            if (actualPoints[(int)color] < targetAnsPoints)
            {
                actualPoints[(int)color]++;
                setAnsBarSize((int)color, actualPoints[(int)color]);
            }
            enemyController.verifyRowIntegrity(color, actualRow);
            for (int i = 0; i < actualPoints.Length; i++)
            {
                if (actualPoints[i] >= targetAnsPoints)
                {
                    goToNextLevel(i == actualQuestionAns);
                }
            }
        }

        public void restartMinigame()
        {
            gameOverScreen.SetActive(false);
            loadingScreen.SetActive(true);
            SceneManager.LoadSceneAsync("SpaceInvaders");
        }

        public void backToMain()
        {
            gameOverScreen.SetActive(false);
            loadingScreen.SetActive(true);
            
            if (FindObjectOfType<InfoEntreEscenas>().EsModoLibre)
            {
                SceneManager.LoadSceneAsync("MainMenu");
                return;
            }

            FindObjectOfType<Minijuego>().setScore(((float)actualScore / targetQuestions) * 10f);
            SceneManager.LoadSceneAsync("Main");
        }

        public void destroyBullets()
        {
            foreach (Transform bullet in bulletParent.transform)
            {
                Destroy(bullet.gameObject);
            }
        }

        private void resetLevel()
        {
            resetAnswers();
            player.resetState();
            enemyController.resetEnemies();
        }

        private void resetAnswers()
        {
            for (int i = 0; i < answerImages.Length; i++)
            {
                answerImages[i].color = enemyController.getColor((Enemy.EnemyColor)i);
                actualPoints[i] = 0;
                setAnsBarSize(i, 0);
            }
        }

        private void setQuestion()
        {
            if (actualQuestion - 1 < questionsList.Count)
            {
                question = new MultiOptionQuestion(questionsList[actualQuestion - 1]);
                sentence.text = question.sentence;
                answerTexts[0].text = question.optionA;
                answerTexts[1].text = question.optionB;
                answerTexts[2].text = question.optionC;
                answerTexts[3].text = question.optionD;
                actualQuestionAns = question.correctId;
            }
        }

        private void setAnsBarSize(int ansewrId, int progressPoints)
        {
            float newSize = maxSize * ((float)progressPoints / targetAnsPoints);
            answerBars[ansewrId].sizeDelta = new Vector2(newSize, answerBars[ansewrId].sizeDelta.y);
        }

        private IEnumerator showFirstQuestion()
        {
            player.setInvulnerable(true);
            player.enabled = false;
            if (question != null && question.sentence != null)
            {
                showQuestionText.text = question.sentence;
            }
            yield return new WaitForSeconds(showQuestionTime);
            questionsScreen.SetActive(false);
            player.setInvulnerable(false);
            player.enabled = true;
            yield return new WaitForSeconds(showAnswersTime);
            StartCoroutine(enemyController.enableEnemies());
        }

        private IEnumerator nextLevel()
        {
            player.setInvulnerable(true);
            player.enabled = false;

            if (actualQuestion < targetQuestions)
            {
                actualQuestion++;
                levelScreen.SetActive(true);
                enemyController.disableEnemies();
                scoreText.text = "Pregunta: " + actualQuestion + "\nPuntuación: " + actualScore;
                playingScoreText.text = "Pregunta: " + actualQuestion + "   Puntuación: " + actualScore;
                destroyBullets();
                if (actualQuestion <= questionsList.Count)
                {
                    setQuestion();
                }
                resetLevel();
                yield return new WaitForSeconds(showScoreTime);
                levelScreen.SetActive(false);
                questionsScreen.SetActive(true);
                showQuestionText.text = question.sentence;
                yield return new WaitForSeconds(showQuestionTime);
                questionsScreen.SetActive(false);
                player.setInvulnerable(false);
                player.enabled = true;
                yield return new WaitForSeconds(showAnswersTime);
                StartCoroutine(enemyController.enableEnemies());
            }
            else
            {
                gameOverScreen.SetActive(true);
                destroyBullets();
                gameOverScoreText.text = "Puntuación: " + actualScore + "/" + targetQuestions
                    + "\n\n" + ((float)actualScore / targetQuestions >= 0.6 ? "¡Felicidades, pasaste la prueba!" : "¡Lástima, has reprobado!");
                if ((float)actualScore / targetQuestions >= 0.6){
                    retryButton.SetActive(false);
                }
            }
        }
    }
}