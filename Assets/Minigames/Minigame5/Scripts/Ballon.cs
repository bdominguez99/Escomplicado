using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : MonoBehaviour {
    public void StopBallon() {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    public void IncreaseSize() {
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
    }
}
