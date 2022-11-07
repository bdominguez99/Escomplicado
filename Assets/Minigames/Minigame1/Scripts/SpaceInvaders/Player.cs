using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int initialLifePoints = 3;
    [SerializeField] private float maxRange = 4f, timeBeforeShoot = 0.1f, dieImpulseForce = 1f;
    [SerializeField] private GameObject lifeBar, bullet, bulletParent;
    [SerializeField] private Animator shieldAnimator;

    private LifeBar lifeBarScript;
    private Rigidbody2D rbd;
    private Vector2 initialPos;
    private float targetx;
    private bool canShoot, isShooting, canTakeDamage = true, isDying;
    private int actualLifePoints;

    private void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        lifeBarScript = lifeBar.GetComponent<LifeBar>();
        initialPos = transform.position;
        actualLifePoints = initialLifePoints;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            targetx = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            if (targetx > -maxRange && targetx < maxRange)
                transform.position = new Vector2(initialPos.x + targetx, transform.position.y);
        }
        if ((isShooting || Input.GetKey(KeyCode.Space)) && !canShoot && !isDying)
        {
            canShoot = true;
            StartCoroutine(shoot());
        }
    }

    public void setShooting(bool isShooting)
    {
        this.isShooting = isShooting;
    }

    public int getMaxLifePoints()
    {
        return initialLifePoints;
    }

    public void takeDamage(int dmg)
    {
        actualLifePoints -= dmg;
        shieldAnimator.SetTrigger("Hurt");
        if (actualLifePoints <= 0)
        {
            isDying = true;
            GetComponent<Animator>().SetTrigger("Die");
            rbd.gravityScale = 1;
            rbd.AddForce(Vector2.up * dieImpulseForce, ForceMode2D.Impulse);
        }
        else
        {
            lifeBarScript.updateLifeSize(actualLifePoints);
        }
    }

    public void resetState()
    {
        rbd.gravityScale = 0;
        transform.position = new Vector2(initialPos.x, initialPos.y);
        actualLifePoints = initialLifePoints;
        lifeBarScript.updateLifeSize(initialLifePoints);
        isDying = false;
    }

    public void setInvulnerable(bool invulnerable) {
        canTakeDamage = !invulnerable;
    }

    private void die()
    {
        rbd.velocity = Vector2.zero;
        lifeBarScript.updateLifeSize(initialLifePoints);
        FindObjectOfType<GameController>().goToNextLevel();
    }

    private IEnumerator shoot()
    {
        Instantiate(bullet, transform.position, Quaternion.identity, bulletParent.transform);
        yield return new WaitForSeconds(timeBeforeShoot);
        canShoot = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (canTakeDamage)
            {
                takeDamage(1);
            }
            Destroy(collision.gameObject);
        }
    }
}
