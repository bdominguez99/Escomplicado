using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float vel;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(0, vel, 0);
    }

    void OnBecameInvisible() {
        transform.position = new Vector3(transform.position.x, -1 * transform.position.y, transform.position.z);
    }
}
