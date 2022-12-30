using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frogger : MonoBehaviour {
    private AudioSource source;
    private Global gameManager;
    private SpriteRenderer spriteRenderer;
    private Collider2D activeOption;
    private Vector3 spawnPosition;
    private Animator anim;
    private bool flag = true;

    public string carry = "";
    public string swipeDir; 
    public Sprite winSprite;
    public Sprite jumpSprite;
    public Sprite deadSprite1;
    public Sprite deadSprite2;
    public Sprite deadSprite3;
    public Sprite deadSprite4;

    void Start() {
        spawnPosition = transform.position;
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<Global>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }

    void Update() {
        if (!flag) return;
        if (Input.GetKeyDown(KeyCode.UpArrow) || swipeDir == "up") {
            StartCoroutine(Jump(0, new Vector3(0, 1, 0)));
        } else if (Input.GetKeyDown(KeyCode.DownArrow) || swipeDir == "down") {
            StartCoroutine(Jump(0, new Vector3(0, -1, 0)));
        } else if (Input.GetKeyDown(KeyCode.LeftArrow) || swipeDir == "left") {
            StartCoroutine(Jump(-1, new Vector3(-1, 0, 0)));
        } else if (Input.GetKeyDown(KeyCode.RightArrow) || swipeDir == "right") {
            StartCoroutine(Jump(1, new Vector3(1, 0, 0)));
        }
        swipeDir = "";
    }

    void StopAnimation(bool val = true) {
        anim.enabled = !val;
        flag = !val;
    }

    IEnumerator Jump(int dir, Vector3 vec) {
        StopAnimation();
        if (dir != 0) {
            transform.localScale = new Vector2(dir, transform.localScale.y);
        }
        spriteRenderer.sprite = jumpSprite; 
        yield return new WaitForSeconds(0.1f);
        StopAnimation(false);
        Move(vec);
    }

    int CheckPosition(Vector3 pos) {
        var z = Vector2.zero;
        var water = Physics2D.OverlapBox(pos, z, 0f, LayerMask.GetMask("Water"));
        var barrier = Physics2D.OverlapBox(pos, z, 0f, LayerMask.GetMask("Barrier"));
        var platform = Physics2D.OverlapBox(pos, z, 0f, LayerMask.GetMask("Platform"));
        var obstacle = Physics2D.OverlapBox(pos, z, 0f, LayerMask.GetMask("Obstacle"));
        
        if(barrier) return 0;
        if(obstacle) return -1;
        if (!platform) {
            transform.SetParent(null);
            if(water) return -2;
        } else {
            transform.SetParent(platform.transform);
            return 1;
        } 
        return 1;
    }

    void Move(Vector3 direction) {
        Vector3 nextPosition = transform.position + direction;
        int option = CheckPosition(nextPosition); 
        if (option < 0) {
            if (option == -2) transform.position = nextPosition;
            ReturnBegin();
        } else if (option == 1) {
            transform.position = nextPosition;
            source.Play();
        }
    }

    public void ReturnBegin(bool score = false) {
        bool f = false;
        if (score) {
            var objects = FindObjectsOfType<Goal>();
            if (objects.Length > 1) return;
            else {
                var ans = activeOption.GetComponent<Package>().option;
                activeOption.GetComponent<Package>().EnableAnimation();
                if (carry == ans) {
                    gameManager.SetResult(true);
                    activeOption.gameObject.SetActive(false);
                    var packages = FindObjectsOfType<Package>();
                    foreach (var pac in packages) {
                        if (pac.gameObject.activeSelf == true) 
                            f = pac.gameObject.activeSelf;
                    }
                } else {
                    gameManager.SetResult(false);
                    gameManager.ReduceLife(flag);
                    score = false;
                } 
                transform.SetParent(null);
                activeOption = null;
                carry = "";
            }
        } else {
            gameManager.ReduceLife(flag);
            transform.SetParent(null);
        }
        StartCoroutine(ChangeAnimation(score, f));
    }

    public IEnumerator ChangeAnimation(bool score, bool f = true) {
        if (flag) {
            StopAnimation();
            if(!score) {
                spriteRenderer.sprite = deadSprite1;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.sprite = deadSprite2;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.sprite = deadSprite3;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.sprite = deadSprite4;
                yield return new WaitForSeconds(0.15f);
                spriteRenderer.sprite = deadSprite3;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.sprite = deadSprite2;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.sprite = deadSprite1;
                yield return new WaitForSeconds(0.1f);
                transform.position = spawnPosition;
            } else {
                spriteRenderer.sprite = winSprite;
                yield return new WaitForSeconds(0.75f);
                if (!f) gameManager.SetUpMap();
                if (gameManager.total == gameManager.maxScore) {
                    gameManager.gameOver();
                    Destroy(this.gameObject);
                }
            }
            StopAnimation(false);
            transform.position = spawnPosition;
            if (gameManager.lifes == 0) {
                gameManager.gameOver();
                Destroy(this.gameObject);
            } else gameManager.result.text = "";
            
            gameManager.counter = 0;
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Package")) {
            gameManager.pluck.Play();
            gameManager.start = true;
            carry = "";
            if (activeOption != null) {
                activeOption.GetComponent<Package>().EnableAnimation();
            }
            activeOption = other;
            other.GetComponent<Package>().ChangeSprite();
            var option = other.GetComponent<Package>().option;
            gameManager.CreateLetters(option);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Goal")) {
            gameManager.pluck.Play();
            carry += other.GetComponent<Goal>().value;
            Destroy(other.gameObject);
            ReturnBegin(true);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            ReturnBegin();
        }
    }
}
