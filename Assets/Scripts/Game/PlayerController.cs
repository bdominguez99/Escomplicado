using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    private Animator animator;
    private Rigidbody2D rigidBody;
    private Vector2 direction;
    [SerializeField] private float velocity = 5f;
    [SerializeField] private bool canMove = true;

    private bool stoppedMoving;

    [Header("Interacción")]
    public GameObject ultimaColision;

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        HandleMovement();
        HandleOrientation();
    }

    private void HandleMovement(){
        if(canMove && Input.GetMouseButton(0)){
            if (stoppedMoving)
            {
                stoppedMoving = false;
                FindObjectOfType<AudioMenu>().playWalk();
            }
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
            direction.Normalize();
            Vector2 velocityVector = direction * velocity;
            rigidBody.velocity = velocityVector;
        } else{
            if (!stoppedMoving)
            {
                FindObjectOfType<AudioMenu>().stopPlaying();
                stoppedMoving = true;
                Debug.Log("Stopped moving");
            }
            rigidBody.velocity = Vector2.zero;
            animator.SetBool("isMoving", false);
        } 
    }

    private void HandleOrientation(){
        transform.localScale = new Vector2(direction.x > 0?1:-1, transform.localScale.y);
    }

    public void setCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive"))
        {
            ultimaColision = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactive"))
        {
            ultimaColision = null;
        }
    }
}
