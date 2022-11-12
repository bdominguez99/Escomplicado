using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {
    private SpriteRenderer sr;

    public Sprite life4;
    public Sprite life3;
    public Sprite life2;
    public Sprite life1;

    void Start() {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ReduceLife(int val) {
        if (val == 4) sr.sprite = life4;
        if (val == 3) sr.sprite = life3; 
        if (val == 2) sr.sprite = life2; 
        if (val == 1) sr.sprite = life1; 
    }
}
