using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TripasDeGato;

public class SnakeManager : MonoBehaviour {
    public GameObject gameOverScreen, loadingScreen, scoreText, messageScreen, messageText, againButton;
    private List<OrderedQuestion> questionsList;
    public List<string> answers;
    private string question;

    private List<string> actualAns;
    private List<string> aux = new List<string>();
    private Transform canvas;
    private Text msg, time;
    private Snake player;
    private int maxScore = 5;
    private int minScore = 3;

    public bool end = false;
    public float swipeThreshold = 50f;
    public float timeThreshold = 0.3f;

    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;
    public UnityEvent OnSwipeUp;
    public UnityEvent OnSwipeDown;

    private Vector2 fingerDown;
    private DateTime fingerDownTime;
    private Vector2 fingerUp;
    private DateTime fingerUpTime;
    private int timer = 0;
    private int count = 0;
    private int counter = 0;
    private float seconds = 300;

    private async Task setPreguntas() {
        if (FindObjectOfType<InfoEntreEscenas>().EsModoLibre)
        {
            var materia = FindObjectOfType<InfoEntreEscenas>().MateriaModoLibre;
            questionsList = await FindObjectOfType<CargadorDeDB>().DataManager.GetOrderedQuestions(materia);
        }
        else
        {
            questionsList = await FindObjectOfType<CargadorDeDB>().DataManager.GetOrderedQuestions("Redes de Computadoras");
        }

        ExtensionMethods.Shuffle(questionsList);
        actualAns = new List<string>(questionsList[count].answers);
        messageScreen.SetActive(true);
        actualAns.Shuffle();
    }

    async void Start() {
        canvas = GameObject.Find("Canvas").transform;
        player = GameObject.Find("Player").GetComponent<Snake>();
        msg = canvas.Find("Message").GetComponent<Text>();
        time = canvas.Find("Time").GetComponent<Text>();
        msg.gameObject.SetActive(false);
        await setPreguntas();
        SetTerrain();
    }

    void FixedUpdate() {
        if (player.lifes > 0) {
            counter += 1;
            if (messageScreen.activeSelf == true) {
                if (counter > 150 + timer) {
                    messageScreen.SetActive(false);
                    player.moving = true;
                    counter -= 150;
                }
            } else {
                if (seconds*60 >= counter) {
                    var res = seconds - (int)(counter/60);
                    time.text = res.ToString();
                    timer = counter;
                }
                else player.DieByTime();
            }
        }
    }

    public void gameOver(bool win = false) {
        if (!win) Destroy(player.gameObject);
        scoreText.GetComponent<Text>().text = "Puntuación: " + count + "/" + maxScore;
        if (count >= minScore) againButton.SetActive(false);
        gameOverScreen.SetActive(true);
    }

    public void restartMinigame() {
        gameOverScreen.SetActive(false);
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("SnakeScene");
    }

    public void backToMain() {
        gameOverScreen.SetActive(false);
        loadingScreen.SetActive(true);

        if (FindObjectOfType<InfoEntreEscenas>().EsModoLibre)
        {
            SceneManager.LoadSceneAsync("MainMenu");
            return;
        }

        FindObjectOfType<Minijuego>().setScore(((float)count / maxScore)*10f);
        SceneManager.LoadSceneAsync("Main");
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            this.fingerDown = Input.mousePosition;
            this.fingerUp = Input.mousePosition;
            this.fingerDownTime = DateTime.Now;
        }
        if (Input.GetMouseButtonUp(0)) {
            this.fingerDown = Input.mousePosition;
            this.fingerUpTime = DateTime.Now;
            this.CheckSwipe();
        }
        foreach (Touch touch in Input.touches) {
            if (touch.phase == TouchPhase.Began) {
                this.fingerDown = touch.position;
                this.fingerUp = touch.position;
                this.fingerDownTime = DateTime.Now;
            }
            if (touch.phase == TouchPhase.Ended) {
                this.fingerDown = touch.position;
                this.fingerUpTime = DateTime.Now;
                this.CheckSwipe();
            }
        }
    }

    private void CheckSwipe() {
        float duration = (float)this.fingerUpTime.Subtract(this.fingerDownTime).TotalSeconds;
        if (duration > this.timeThreshold) return;

        float deltaX = this.fingerDown.x - this.fingerUp.x;
        if (Mathf.Abs(deltaX) > this.swipeThreshold) {
            if (deltaX > 0) {
                // this.OnSwipeRight.Invoke();
                player.swipeDir = "right";
                Debug.Log("right");
            } else if (deltaX < 0) {
                // this.OnSwipeLeft.Invoke();
                player.swipeDir = "left";
                Debug.Log("left");
            }
        }

        float deltaY = fingerDown.y - fingerUp.y;
            if (Mathf.Abs(deltaY) > this.swipeThreshold) {
            if (deltaY > 0) {
                // this.OnSwipeUp.Invoke();
                player.swipeDir = "up";
                Debug.Log("up");
            } else if (deltaY < 0) {
                // this.OnSwipeDown.Invoke();
                player.swipeDir = "down";
                Debug.Log("down");
            }
        }
        this.fingerUp = this.fingerDown;
    }

    public IEnumerator ShowMessage(string t) {
        msg.text = t;
        msg.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        msg.gameObject.SetActive(false);
    }

    public void SetScore() {
        count += 1;
        canvas.Find("Score").GetComponent<Text>().text = count+"/"+maxScore;
        if (count == maxScore) {
            StartCoroutine(player.WinAnimation(true));
        } else {
            actualAns = new List<string>(questionsList[count].answers);
            StartCoroutine(player.WinAnimation());
            actualAns.Shuffle();
            SetTerrain();
        }
    }

    public void setNewQuestion() {
        answers = questionsList[count].answers;
        question = questionsList[count].question;
        messageText.GetComponent<Text>().text = question;
        canvas.Find("Question").GetComponent<Text>().text = question;
    }

    public void SetTerrain() {
        setNewQuestion();
        for (int i = 0; i < 7; i++) {
            var bag = transform.Find("Bag"+i);
            if (i < answers.Count) {
                bag.gameObject.SetActive(true);
                bag.GetComponent<Bag>().value = GetIndexOf(actualAns[i]);
                canvas.Find("Answer"+i).GetComponent<Text>().text = actualAns[i];
            } else {
                canvas.Find("Answer"+i).GetComponent<Text>().text = "";
                bag.gameObject.SetActive(false);
            }
        }
    }

    int GetIndexOf(string ans) {
        for (int i = 0; i < answers.Count; i++) {
            if (answers[i] == ans) return i;
        }
        return 0;
    }

    public bool CheckPosition(float posx, float posy) {
        if (posx == -3 && posy == -1) return false;
        string pos = (posx, posy).ToString(); 
        if (aux.Contains(pos)) return false;
        aux.Add(pos);
        return true;
    }
}