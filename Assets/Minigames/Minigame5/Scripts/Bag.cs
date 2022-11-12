using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour {
    public int value;
    
    void Start() {
        RandomizePosition();
    }

    public void RandomizePosition() {
        float x = Random.Range(0.05f, 0.7f);
        float y = Random.Range(0.05f, 0.8f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        float posx = Mathf.Round(pos.x);
        float posy = Mathf.Round(pos.y);
        bool isFree = FindObjectOfType<SnakeManager>().CheckPosition(posx, posy);
        if (isFree) this.transform.position = new Vector3(posx, posy, 0);
        else RandomizePosition();
    }
}
