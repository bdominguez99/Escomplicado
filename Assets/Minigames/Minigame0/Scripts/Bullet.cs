using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 1f, maxHeight = 5f, minHeight = -5f;

        void Update()
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            if (transform.position.y > maxHeight || transform.position.y < minHeight)
            {
                Destroy(gameObject);
            }
        }
    }
}