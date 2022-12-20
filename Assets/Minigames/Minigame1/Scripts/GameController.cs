using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using SpaceInvaders;
using TripasDeGato;

namespace pipes
{
    public class GameController : MonoBehaviour
    {
        public static GameObject switchStart { get; set; }
        public static GameObject switchEnd { get; set; }
        public static bool isSiwtching { get; set; }
        public static bool isPaused { get; set; }

        [SerializeField] bool showOptionsInPause;

        [Header("Game")]
        [SerializeField] private Text sentence;
        [SerializeField] private Text[] answerTexts;
        [SerializeField] private GameObject loadingScreen, pauseScreen;
        [SerializeField] private float switchingTime = 0.3f;
        [SerializeField] private int targetQuestions = 10;

        [Header("Score screen")]
        [SerializeField] private float showScoreTime = 1f;
        [SerializeField] private Text scoreText, playingScoreText;
        [SerializeField] private GameObject levelScreen;

        [Header("Question screen")]
        [SerializeField] private Text showQuestionText;
        [SerializeField] private GameObject questionsScreen;
        [SerializeField] private float showQuestionTime = 1f;

        [Header("Game over screen")]
        [SerializeField] private Text gameOverScoreText;
        [SerializeField] private GameObject gameOverScreen, retryButton;

        private int actualQuestion = 1, actualQuestionAns, actualScore;
        private PipeController pipeController;
        private PipeStart pipeStart;
        private MultiOptionQuestion question;
        private List<MultipleOptionQuestion> questionsList;

        private async void Start()
        {
            questionsScreen.SetActive(true);
            await setPreguntas();
            pipeController = FindObjectOfType<PipeController>();
            pipeStart = FindObjectOfType<PipeStart>();
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
                questionsList = await FindObjectOfType<CargadorDeDB>().DataManager.GetMultipleOptionQuestions("Tecnologias para la Web");
            }
            ExtensionMethods.Shuffle(questionsList);
        }

        public void pauseGame(bool pause = true)
        {
            isPaused = pause;
            pauseScreen.SetActive(pause);
        }

        public void trySwitchPipes()
        {
            if(switchStart && switchEnd)
            {
                StartCoroutine(switchPipes());
            }
        }

        public void goToNextLevel(bool correctAns = false)
        {
            if (correctAns)
            {
                actualScore++;
            }
            StartCoroutine(nextLevel());
        }

        public void selectAnswer(int answerId)
        {
            goToNextLevel(answerId == actualQuestionAns);
        }


        public void restartMinigame()
        {
            gameOverScreen.SetActive(false);
            loadingScreen.SetActive(true);
            SceneManager.LoadSceneAsync("Pipes");
        }

        public void goHome()
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

        private void setInfoPregunta()
        {
            pauseScreen.SetActive(true);
            sentence.text = "Pregunta " + actualQuestion + ":\n" + question.sentence;
            if (showOptionsInPause)
            {
                sentence.text += "\nA) " + question.optionA;
                sentence.text += "\nB) " + question.optionB;
                sentence.text += "\nC) " + question.optionC;
                sentence.text += "\nD) " + question.optionD;
            }
            pauseScreen.SetActive(false);
        }

        private void resetLevel()
        {
            pipeStart.resetPipe();
            pipeController.destroyMtrx();
            pipeController.initialiceMtrx();
            StartCoroutine(pipeStart.startfilling());
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

        private IEnumerator switchPipes()
        {
            isSiwtching = true;
            switchEnd.GetComponent<Pipe>().prepareColorToSwitch();
            Transform start = switchStart.transform, end = switchEnd.transform;
            Vector2 startPos = start.position, endPos = end.position;
            float progress = 0f;
            while (progress < 1){
                progress += Time.deltaTime / switchingTime;
                start.position = Vector2.Lerp(startPos, endPos, progress);
                end.position = Vector2.Lerp(endPos, startPos, progress);
                yield return null;
            }
            start.position = endPos;
            end.position = startPos;
            switchStart.GetComponent<Pipe>().resetAfterSwitch();
            switchEnd.GetComponent<Pipe>().resetAfterSwitch();
            switchStart = null;
            switchEnd = null;
            isSiwtching = false;
        }

        private IEnumerator showFirstQuestion()
        {
            if (question != null && question.sentence != null)
            {
                showQuestionText.text = question.sentence;
                setInfoPregunta();
            }
            yield return new WaitForSeconds(showQuestionTime);
            resetLevel();
            questionsScreen.SetActive(false);
        }

        private IEnumerator nextLevel()
        {
            if (actualQuestion < targetQuestions)
            {
                actualQuestion++;
                levelScreen.SetActive(true);
                scoreText.text = "Pregunta: " + actualQuestion + "\nPuntuacion: " + actualScore;
                playingScoreText.text = "Pregunta: " + actualQuestion + "   Puntuacion: " + actualScore;
                if (actualQuestion <= questionsList.Count)
                {
                    setQuestion();
                }
                yield return new WaitForSeconds(showScoreTime);
                questionsScreen.SetActive(true);
                showQuestionText.text = question.sentence;
                levelScreen.SetActive(false);
                setInfoPregunta();
                yield return new WaitForSeconds(showQuestionTime);
                resetLevel();
                questionsScreen.SetActive(false);
            }
            else
            {
                gameOverScreen.SetActive(true);
                gameOverScoreText.text = "Puntuacion: " + actualScore + "/" + targetQuestions
                    + "\n" + ((float)actualScore / targetQuestions >= 0.6 ? "¡Felicidades, pasaste la prueba!" : "¡Lastima, has reprobado!");
                if ((float)actualScore / targetQuestions >= 0.6)
                {
                    retryButton.SetActive(false);
                }
            }
        }
    }
}