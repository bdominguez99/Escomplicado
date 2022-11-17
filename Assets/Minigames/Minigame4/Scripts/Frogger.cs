using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frogger : MonoBehaviour {
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
        flag = !val;
        anim.enabled = !val;
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
        
        if (!platform) {
            transform.SetParent(null);
            if(water) return -2;
        } else transform.SetParent(platform.transform);
        if(obstacle) return -1;
        if(barrier) return 0;
        return 1;
    }

    void Move(Vector3 direction) {
        Vector3 nextPosition = transform.position + direction;
        int option = CheckPosition(nextPosition); 
        if (option < 0) {
            if (option == -2) {
                transform.position = nextPosition;
            }
            ReturnBegin();
        } else if (option == 1) {
            transform.position = nextPosition;
        }
    }

    public void ReturnBegin(bool score = false) {
        if (score) {
            var objects = FindObjectsOfType<Goal>();
            if (objects.Length > 1) return;
            else {
                transform.SetParent(null);
                activeOption.gameObject.SetActive(false);
                activeOption.GetComponent<Package>().EnableAnimation();
                var ans = activeOption.GetComponent<Package>().option;
                if (carry == ans) gameManager.SetResult(true);
                else {
                    gameManager.SetResult(false);
                    gameManager.ReduceLife(flag);
                    score = false;
                } 
                activeOption = null;
                carry = "";
            }
        } else gameManager.ReduceLife(flag);
        StartCoroutine(ChangeAnimation(score));
    }

    public IEnumerator ChangeAnimation(bool score) {
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
                if (gameManager.total == 5) {
                    gameManager.gameOver();
                    Destroy(this.gameObject);
                }
            }
            StopAnimation(false);
            transform.position = spawnPosition;
            if (gameManager.lifes == 0) {
                gameManager.gameOver();
                Destroy(this.gameObject);
            }
            else gameManager.result.text = "";
            gameManager.timer = 0;
        }
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Package")) {
            carry = "";
            if (activeOption != null) {
                activeOption.GetComponent<Package>().EnableAnimation();
            }
            activeOption = other;
            other.GetComponent<Package>().ChangeSprite();
            var option = other.GetComponent<Package>().option;
            gameManager.CreateLetters(option);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Goal")) {
            carry += other.GetComponent<Goal>().value;
            Destroy(other.gameObject);
            ReturnBegin(true);
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
            ReturnBegin();
        }
    }
}
