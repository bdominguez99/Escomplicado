using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {
    public string value;

    void Start() {
        RandomizePosition();
    }

    private void RandomizePosition() {
        float x = Random.Range(0.25f, 0.95f);
        float y = Random.Range(0.05f, 0.85f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        float posx = Mathf.Round(pos.x);
        float posy = Mathf.Round(pos.y);
        bool isFree = FindObjectOfType<Global>().CheckPosition(posx, posy);
        if (isFree) this.transform.position = new Vector3(posx, posy, 10);
        else RandomizePosition();
    }

    public void SetLetter(string letter) {
        this.transform.GetChild(0).transform.Find("Text").GetComponent<Text>().text = letter;
        value = letter;
    }
}
