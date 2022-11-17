using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    public string option;
    public Sprite newSprite;

    void Start() {
        anim = GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void EnableAnimation() {
        anim.enabled = true;
    }

    public void ChangeSprite() {
        anim.enabled = false;
        spriteRenderer.sprite = newSprite;
    }
}
