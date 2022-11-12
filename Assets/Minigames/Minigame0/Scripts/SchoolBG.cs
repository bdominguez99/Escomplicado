using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class SchoolBG : MonoBehaviour
    {
        [SerializeField] private float speed;

        private RawImage image;

        private void Start()
        {
            image = GetComponent<RawImage>();
        }

        void Update()
        {
            image.uvRect = new Rect(image.uvRect.x, image.uvRect.y + (Time.deltaTime * speed), image.uvRect.width, image.uvRect.height);
        }
    }
}