using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class LifeBar : MonoBehaviour
    {
        [SerializeField] private Color fullColor, lowColor;
        [SerializeField] private GameObject fill;

        private int maxLife;
        private float maxSize, actualSize;
        private SpriteRenderer fillRender;

        private void Start()
        {
            fillRender = fill.GetComponent<SpriteRenderer>();
            maxLife = FindObjectOfType<Player>().getMaxLifePoints();
            maxSize = fill.transform.localScale.x;
        }

        public void updateLifeSize(int life)
        {
            actualSize = maxSize * ((float)life / maxLife);
            fillRender.color = Color.Lerp(lowColor, fullColor, actualSize);
            fill.transform.localScale = new Vector2(actualSize, fill.transform.localScale.y);
        }
    }
}