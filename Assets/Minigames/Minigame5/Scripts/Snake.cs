using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {
    private Vector3 spawnPosition, lastPosition;
    private SpriteRenderer spriteRenderer;
    private Sprite initialSprite;
    private SnakeManager gm;
    private Rigidbody2D rb;
    private Animator anim;
    private Life life;
    private AudioSource source;

    private bool flag = true;
    private float timer = 0.0f;
    private bool start = false;

    public bool moving = false;
    public Transform prefab;
    public Sprite failSprite;
    public Sprite winSprite;
    public string swipeDir; 
    public int carry = 0;
    public int lifes = 0;
    private float vel = 4;

    void Start() {
        life = FindObjectOfType<Life>();
        gm = FindObjectOfType<SnakeManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        initialSprite = spriteRenderer.sprite;
        anim = GetComponent<Animator>();
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
    }

    void Update() {
        anim.enabled = start;
        if (moving) {
            var objects = FindObjectsOfType<Ballon>();
            if (Input.GetKeyDown(KeyCode.UpArrow) || swipeDir == "up") {
                if (objects.Length > 0) {
                    if (rb.velocity.y >= 0) 
                        ChangeVelocity(new Vector3(0, vel, 0));
                } else ChangeVelocity(new Vector3(0, vel, 0));
            } else if (Input.GetKeyDown(KeyCode.DownArrow) || swipeDir == "down") {
                if (objects.Length > 0) {
                    if (rb.velocity.y <= 0) 
                        ChangeVelocity(new Vector3(0, -vel, 0));
                } else ChangeVelocity(new Vector3(0, -vel, 0));
            } else if (Input.GetKeyDown(KeyCode.LeftArrow) || swipeDir == "left") {
                if (objects.Length > 0) {
                    if (rb.velocity.x <= 0) 
                        ChangeVelocity(new Vector3(-vel, 0, 0), -1);
                } else ChangeVelocity(new Vector3(-vel, 0, 0), -1);
            } else if (Input.GetKeyDown(KeyCode.RightArrow) || swipeDir == "right") {
                if (objects.Length > 0) {
                    if (rb.velocity.x >= 0) 
                        ChangeVelocity(new Vector3(vel, 0, 0), 1);
                } else ChangeVelocity(new Vector3(vel, 0, 0), 1);
            }
            swipeDir = "";
        }
    }

    void ChangeVelocity(Vector3 direction, int dir = 0) {
        if (dir != 0) {
            transform.localScale = new Vector2(dir, transform.localScale.y);
        }
        rb.velocity = direction;
        source.Play();
        start = true;
    }

    void ChangePositions(Vector3 pos) {
        var nextPos = pos;
        var objects = FindObjectsOfType<Ballon>();
        foreach (var obj in objects) {
            nextPos = obj.transform.position;
            obj.transform.position = pos;
            pos = nextPos;
        }
    }

    public IEnumerator WinAnimation(bool win = false) {
        start = false;
        moving = false;
        rb.velocity = Vector3.zero;
        spriteRenderer.sprite = winSprite;
        var objects = FindObjectsOfType<Ballon>();
        for (int i = 0; i < objects.Length; i++) {
            yield return new WaitForSeconds(0.1f);
            Destroy(objects[i].gameObject);
        } 
        if (win) {
            gm.gameOver(true);
            lifes = 0;
        } else {
            gm.messageScreen.SetActive(true);
            ReturnSpawnPosition();
        }
    }

    IEnumerator invinsible() {
        flag = false;
        yield return new WaitForSeconds(0.25f);
        flag = true;
    }

    IEnumerator failAnimation() {
        start = false;
        moving = false;
        rb.velocity = Vector3.zero;
        spriteRenderer.sprite = failSprite; 
        yield return new WaitForSeconds(0.5f);
        var objects = FindObjectsOfType<Ballon>();
        if (objects.Length > 0) {
            var ballon = objects[0].GetComponent<Ballon>();
            ChangePositions(transform.position);
            ballon.StopBallon();
            for (int i = objects.Length-1; i >= 0; i--) {
                ballon.IncreaseSize();
                yield return new WaitForSeconds(0.1f);
                Destroy(objects[i].gameObject);
            }
        }
        ReturnSpawnPosition();
        if (lifes == 0) gm.gameOver();
        else moving = true;
    }

    void ReturnSpawnPosition() {
        spriteRenderer.sprite = initialSprite; 
        transform.position = spawnPosition;
        lastPosition = spawnPosition;
    }

    public void DieByTime() {
        lifes = 0;
        life.ReduceLife(4);
        gm.gameOver();
    }

    void ReturnBegin() {
        gm.source.Play();
        lifes -= 1;
        ReloadBags();
        life.ReduceLife(4 - lifes);
        StartCoroutine(failAnimation());
    }

    void ReloadBags() {
        gm.SetTerrain();
        carry = 0;
    }

    private void FixedUpdate() {
        if (start) {
            timer += Time.deltaTime;
            if (timer > 0.3) {
                timer = 0.0f;
                ChangePositions(lastPosition);
                lastPosition = transform.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (start) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Food")) {
                gm.pluck.Play();
                StartCoroutine(invinsible());
                Instantiate(this.prefab);
                var bag = other.gameObject.GetComponent<Bag>();
                bag.RandomizePosition();
                if (bag.value > carry) ReloadBags();
                else {
                    carry += 1;
                    other.gameObject.SetActive(false);
                    if (carry == gm.answers.Count) {
                        gm.SetScore();
                        carry = 0;
                    }
                }
            } else if (other.gameObject.layer == LayerMask.NameToLayer("Ballon") && flag) {
                ReturnBegin();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor")) {
            ReturnBegin();
        }
    }

}
