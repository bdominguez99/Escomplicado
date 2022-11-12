using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite life3;
    public Sprite life2;
    public Sprite life1;
    public Sprite life0;

    void Start() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetSprite(int lifes) {
        if (lifes == 3) spriteRenderer.sprite = life3; 
        if (lifes == 2) spriteRenderer.sprite = life2; 
        if (lifes == 1) spriteRenderer.sprite = life1; 
        if (lifes == 0) spriteRenderer.sprite = life0; 
    }
}
