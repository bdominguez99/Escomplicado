using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int initialLifePoints = 3;
    [SerializeField] private float maxRange = 4f, timeBeforeShoot = 0.1f;
    [SerializeField] private GameObject lifeBar, bullet, bulletParent;

    private LifeBar lifeBarScript;
    private float targetx, initialPosX;
    private bool canShoot, isShooting, canTakeDamage = true;
    private int actualLifePoints;

    private void Start()
    {
        lifeBarScript = lifeBar.GetComponent<LifeBar>();
        initialPosX = transform.position.x;
        actualLifePoints = initialLifePoints;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            targetx = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            if (targetx > -maxRange && targetx < maxRange)
                transform.position = new Vector2(initialPosX + targetx, transform.position.y);
        }
        if ((isShooting || Input.GetKey(KeyCode.Space)) && !canShoot)
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
        if (actualLifePoints <= 0)
        {
            die();
        }
        else
        {
            lifeBarScript.updateLifeSize(actualLifePoints);
        }
    }

    public void resetState()
    {
        transform.position = new Vector2(initialPosX, transform.position.y);
        actualLifePoints = initialLifePoints;
        lifeBarScript.updateLifeSize(initialLifePoints);
    }

    public void setInvulnerable(bool invulnerable) {
        canTakeDamage = !invulnerable;
    }

    private void die()
    {
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
