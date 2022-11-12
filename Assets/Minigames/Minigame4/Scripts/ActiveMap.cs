using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMap : MonoBehaviour
{
    void Start() {
        FindObjectOfType<Global>().start = true;
    }
}
