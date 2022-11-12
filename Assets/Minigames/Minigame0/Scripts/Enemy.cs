using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class Enemy : MonoBehaviour
    {
        public enum EnemyColor { Y, M, G, C };

        [SerializeField] private EnemyColor color;
        [SerializeField] private Color[] colors;
        [SerializeField] private Color initColor;
        [SerializeField] private SpriteRenderer renderBase;
        [SerializeField] private int maxLife;
        [SerializeField] private float shootCooldownTime = 0.5f;
        [SerializeField][Range(0, 1)] private float oscilationSpeed = 1f, oscilationSize = 0.5f;

        private SpriteRenderer renderTop;
        private Player player;
        private GameObject bullet, bulletParent;
        private Collider2D selfCollider;
        private int actualRow, life;
        private float absPositionX, oscilator;
        private bool canShoot, canTakeDamage = true, isActive = true;

        private void Start()
        {
            renderTop = GetComponent<SpriteRenderer>();
            absPositionX = transform.position.x;
            player = FindObjectOfType<Player>();
            selfCollider = GetComponent<BoxCollider2D>();
            life = maxLife;
            canShoot = true;
        }

        private void Update()
        {
            if (actualRow == 3 && isActive && canShoot && Mathf.Abs(transform.position.x - player.transform.position.x) < 0.3f)
            {
                canShoot = false;
                StartCoroutine(shootCooldown());
                Instantiate(bullet, transform.position, Quaternion.identity, bulletParent.transform);
            }
            oscilator += Time.deltaTime * oscilationSpeed * ((actualRow & 1) == 0 ? 1 : -1);
            transform.position = new Vector2(
                absPositionX + (Mathf.Sin(oscilator * 2 * Mathf.PI) * oscilationSize),
                transform.position.y
            );
        }

        public void setColor(EnemyColor color)
        {
            this.color = color;
        }

        public Color getColor(EnemyColor color)
        {
            return colors[(int)color];
        }

        public void setBulletTemplateAndParent(GameObject bullet, GameObject bulletParent)
        {
            this.bullet = bullet;
            this.bulletParent = bulletParent;
        }

        public void takeDamage(int dmg)
        {
            life -= dmg;
            if (life <= 0)
            {
                die();
            }
        }

        public void shoot()
        {
            Debug.Log("Enemy shooted");
            Instantiate(bullet, transform.position, Quaternion.identity, bulletParent.transform);
        }

        public void moveToTarget(Vector2 target, float movingTime)
        {
            StartCoroutine(move(target, movingTime));
        }

        public void setActive(bool active, bool canShoot)
        {
            isActive = canShoot;
            if (active) life = maxLife;
            selfCollider.enabled = active;
            renderBase.color = active ? getColor(color) : new Color(1, 1, 1, 0);
            renderTop.color = active ? initColor : new Color(1, 1, 1, 0);
        }

        public void setRow(int newRow)
        {
            actualRow = newRow;
        }

        public void reset()
        {
            life = maxLife;
            oscilator = 0;
        }

        public void setIsActive(bool isActive)
        {
            this.isActive = isActive;
        }

        private void die()
        {
            setActive(false, false);
            FindObjectOfType<GameController>().addPointsToAns(color, actualRow);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Shoot"))
            {
                if (canTakeDamage)
                {
                    takeDamage(1);
                }
                Destroy(collision.gameObject);
            }
        }

        private IEnumerator move(Vector2 target, float movingTime)
        {
            float lastPositionY = transform.position.y;
            float progress = 0;
            canTakeDamage = false;
            while (progress < 1)
            {
                progress += Time.deltaTime / movingTime;
                transform.position = new Vector2(transform.position.x, Mathf.Lerp(lastPositionY, target.y, progress));
                yield return null;
            }
            transform.position = new Vector2(transform.position.x, Mathf.Lerp(lastPositionY, target.y, 1));
            canTakeDamage = true;
        }

        private IEnumerator shootCooldown()
        {
            yield return new WaitForSeconds(shootCooldownTime);
            canShoot = true;
        }
    }
}