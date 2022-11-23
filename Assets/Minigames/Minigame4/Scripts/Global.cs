using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TripasDeGato;

public class Global : MonoBehaviour {
    public GameObject gameOverScreen, scoreText, loadingScreen, messageScreen, messageText;
    private List<OrderedQuestion> questionsList;
    private List<string> answers;
    private string question;

    public Transform prefab;
    public Text result, score, time;
    public bool start = false;
    public int counter = 0;
    public int lifes = 4;

    private int count = 0;
    private int seconds = 60;
    private bool setCanvas = false;

    private Heart life;
    private Frogger player;
    private GameObject canvas;
    private Transform quest, navbar;
    private List<string> aux = new List<string>();

    public float swipeThreshold = 50f;
    public float timeThreshold = 0.3f;
    public int maxScore = 5;
    public int total = 0;

    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;
    public UnityEvent OnSwipeUp;
    public UnityEvent OnSwipeDown;

    private Vector2 fingerDown;
    private DateTime fingerDownTime;
    private Vector2 fingerUp;
    private DateTime fingerUpTime;

    private async Task setPreguntas() {
        questionsList = await FindObjectOfType<CargadorDeDB>().DataManager.GetOrderedQuestions("Probabilidad y Estadística");
        ExtensionMethods.Shuffle(questionsList);
    }

    async void Start() {
        life = transform.Find("Life").GetComponent<Heart>();
        player = GameObject.Find("Player").GetComponent<Frogger>();
        canvas = GameObject.Find("Canvas");
        canvas.GetComponent<Canvas>().enabled = false;
        quest = canvas.transform.Find("Image").transform.Find("Text");
        navbar = GameObject.Find("Navbar").transform;
        result = navbar.Find("Result").GetComponent<Text>();
        score = navbar.Find("Score").GetComponent<Text>();
        time = navbar.Find("Time").GetComponent<Text>();
        // await setPreguntas();
        // SetUpMap();
    }

    public void gameOver() {
        start = false;
        gameOverScreen.SetActive(true);
        scoreText.GetComponent<Text>().text = "Puntuacion: " + total + "/" + maxScore;
    }

    public void restartMinigame() {
        gameOverScreen.SetActive(false);
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("FroggerScene");
    }

    public void backToMain() {
        gameOverScreen.SetActive(false);
        loadingScreen.SetActive(true);
        FindObjectOfType<Minijuego>().setScore(((float)total / maxScore)*10f);
        SceneManager.LoadSceneAsync("Main");
    }

    void FixedUpdate() {
        if (start) {
            counter += 1;
            if (messageScreen.activeSelf == true) {
                if (counter > 300) {
                    messageScreen.SetActive(false);
                    counter = 0;
                }
            } else {
                if (seconds*60 >= counter) {
                    var res = seconds - (int)(counter/60);
                    time.text = res.ToString();
                }
                else player.ReturnBegin();
            }
        }
    }

    private void Update () {
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
    
    void OnGUI() {
        if(Event.current.isMouse &&
            Event.current.button == 0 && 
            Event.current.clickCount > 1) {
            canvas.GetComponent<Canvas>().enabled = setCanvas;
            setCanvas = !setCanvas;
        }
    }

    public void setNewQuestion() {
        answers = questionsList[count].answers;
        question = questionsList[count].question;
        messageText.GetComponent<Text>().text = question;
        quest.GetComponent<Text>().text = question;
        messageScreen.SetActive(true);
        counter = 0;
    }

    public void SetUpMap() {
        setNewQuestion();
        for (int i = 0; i < 7; i++) {
            var obj = transform.Find("Package"+(i+1));
            if (i < answers.Count) {
                obj.gameObject.SetActive(true);
                obj.GetComponent<Package>().option = answers[i];
            } else {
                obj.GetComponent<Package>().option = "";
                obj.gameObject.SetActive(false);
            }
        }
        count++;
    }

    public void ReduceLife(bool flag) {
        if (flag) life.SetSprite(--lifes);
    }

    public void SetResult(bool isCorrect) {
        var str = "incorrecto!";
        if (isCorrect) {
            total += 1;
            str = "correcto!";
        }
        score.text = total+"/"+maxScore;
        result.text = str;
    }

    public bool CheckPosition(float posx, float posy) {
        string pos = (posx, posy).ToString(); 
        if (aux.Contains(pos)) return false;
        aux.Add(pos);
        return true;
    }

    public void CreateLetters(string option) {
        aux.Clear();
        var objects = FindObjectsOfType<Goal>();
        foreach (var obj in objects) Destroy(obj.gameObject);
        for (int i = 0; i < option.Length; i++) {
            Transform goal = Instantiate(this.prefab);
            goal.GetComponent<Goal>().SetLetter((option[i]).ToString());
        }
    }
}