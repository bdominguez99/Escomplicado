using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demon : MonoBehaviour
{
    [SerializeField] private float m_speed;

    private bool m_changePosition;
    private bool m_isMoving;
    private int m_idRelation;
    private int m_movementFrequency;
    private Vector2 m_targetPosition;
    private Vector2 m_initArea;
    private Vector2 m_endArea;


    public void Init(int idRelation, Vector2 initArea, Vector2 endArea)
    {
        m_idRelation = idRelation;
        var animationName = "Demon" + idRelation;
        GetComponent<Animator>().Play(animationName);

        m_initArea = initArea;
        m_endArea = endArea;

        m_changePosition = true;

        m_movementFrequency = FindObjectOfType<GameController>().DemonMovementFrequency;

        StartCoroutine(MoveRandomly());
    }

    private IEnumerator MoveRandomly()
    {
        while (m_changePosition)
        {
            var xCoordinate = Random.Range(m_initArea.x, m_endArea.x);
            var yCoordinate = Random.Range(m_initArea.y, m_endArea.y);
            var newPosition = new Vector2(xCoordinate, yCoordinate);
            Move(newPosition);
            yield return new WaitForSecondsRealtime(m_movementFrequency);
        }
    }

    private void Update()
    {
        if (m_isMoving && (Vector2)transform.position != m_targetPosition)
        {
            float step = m_speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, m_targetPosition, step);
        }
        m_isMoving = (Vector2)transform.position != m_targetPosition;
    }

    public void Move(Vector2 newPosition)
    {
        m_isMoving = true;
        m_targetPosition = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var rockId = collision.gameObject.GetComponent<Rock>().RelationId;

        if (rockId == m_idRelation)
        {
            m_changePosition = false;
            GetComponent<Animator>().Play("DisappearSmoke");
            FindObjectOfType<GameController>().UpdateStateCorrectAnswer();
            Destroy(collision.gameObject);
            StartCoroutine(WaitTillAnimationOver());
        }
        else
        {
            FindObjectOfType<GameController>().UpdateStateWrongAnswer();
        }
    }

    public IEnumerator WaitTillAnimationOver()
    {
        yield return new WaitForSeconds(1);
        FindObjectOfType<CameraController>().StopFollowingRock();
        Destroy(gameObject);
    }
}
