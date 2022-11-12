using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOn : MonoBehaviour {
    void Start() {
        FindObjectOfType<Snake>().lifes = 4;
    }
}
