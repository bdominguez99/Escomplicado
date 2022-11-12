using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SnakeManager : MonoBehaviour {
    public GameObject gameOverScreen, scoreText;

    private string question = "pregunta de prueba";
    private string[] answers = {"1"};
    private int count = 4;

    private List<string> aux = new List<string>();
    private List<string> actualAns;
    private Transform canvas;
    private Text msg, time;
    private Snake player;

    public bool end = false;
    public bool flag = true;
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
    private float timer = 0;

    void Start() {
        canvas = GameObject.Find("Canvas").transform;
        player = GameObject.Find("Player").GetComponent<Snake>();
        msg = canvas.Find("Message").GetComponent<Text>();
        time = canvas.Find("Time").GetComponent<Text>();
        msg.gameObject.SetActive(false);
        RandomArray();
    }

    void FixedUpdate() {
        if (player.carry == answers.Length) SetScore();
        if (flag) SetTerrain();
        flag = false;
        if (player.lifes > 0) {
            timer += Time.deltaTime;
            var seconds = 300 - (int)(timer % 60);
            if (seconds >= 0) time.text = seconds.ToString();
            else {
                player.DieByTime();
            } 
        }
    }

    public void gameOver(bool win = false) {
        if (!win) Destroy(player.gameObject);
        scoreText.GetComponent<Text>().text = "Puntuacion: " + count + "/" + 5;
        gameOverScreen.SetActive(true);
    }

    public void restartMinigame() {
        // gameOverScreen.SetActive(false);
        // loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("SnakeScene");
    }

    public void backToMain() {
        // gameOverScreen.SetActive(false);
        // loadingScreen.SetActive(true);
        FindObjectOfType<Minijuego>().setScore(((float)count / 5)*10f);
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

    void RandomArray() {
        var r = new System.Random();
        List<string> ans = new List<string>(answers);
        actualAns = new List<string>(ans.Count);
        while (ans.Count > 0) {
            var i = r.Next(ans.Count);
            actualAns.Add(ans[i]);
            ans.RemoveAt(i);
        }
    }

    public void SetScore() {
        count += 1;
        RandomArray();
        player.carry = 0;
        StartCoroutine(ShowMessage("Correcto!"));
        canvas.Find("Score").GetComponent<Text>().text = count+"/5";
        if (count == 5) {
            StartCoroutine(player.WinAnimation(true));
        } else SetTerrain();
    }

    int GetIndexOf(string ans) {
        for (int i = 0; i < answers.Length; i++) {
            if (answers[i] == ans) return i;
        }
        return 0;
    }

    public void SetTerrain(bool boal = false) {
        canvas.Find("Question").GetComponent<Text>().text = question;
        for (int i = 0; i <= 6; i++) {
            var bag = transform.Find("Bag"+i);
            if (i <= answers.Length-1) {
                bag.gameObject.SetActive(true);
                bag.GetComponent<Bag>().value = GetIndexOf(actualAns[i]);
                canvas.Find("Answer"+i).GetComponent<Text>().text = actualAns[i];
            } else {
                canvas.Find("Answer"+i).GetComponent<Text>().text = "";
                bag.gameObject.SetActive(false);
            }
        }
    }

    public bool CheckPosition(float posx, float posy) {
        if (posx == -3 && posy == -1) return false;
        string pos = (posx, posy).ToString(); 
        if (aux.Contains(pos)) return false;
        aux.Add(pos);
        return true;
    }
}