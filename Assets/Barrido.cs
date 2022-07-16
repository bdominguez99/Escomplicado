using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrido : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 1f;
    private bool move;

    private void Start()
    {
        StartCoroutine(delay());
    }

    void Update()
    {
        if(move && Mathf.Abs(gameObject.transform.position.y - target.transform.position.y) > 0.1f)
            gameObject.transform.position += Vector3.up * speed * Time.deltaTime;
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        move = true;
    }
}
