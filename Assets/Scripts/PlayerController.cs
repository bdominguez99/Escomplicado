using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    Rigidbody2D rigidBody;
    Vector2 targetPosition;
    Vector2 direction;
    public float velocidad = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GestionarMovimiento();
        GestionarOrientacion();
    }

    void GestionarMovimiento(){
        if(Input.GetMouseButton(0)){
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
            direction.Normalize();
            Vector2 velocity = direction * velocidad;
            rigidBody.velocity = velocity;
            animator.SetBool("isMoving", true);
        } else{
            rigidBody.velocity = Vector2.zero;
            animator.SetBool("isMoving", false);
        } 
    }

    void GestionarOrientacion(){
        transform.localScale = new Vector2(direction.x > 0?1:-1, transform.localScale.y);
    }
}
