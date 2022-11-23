using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demon : MonoBehaviour
{
    [SerializeField] private float m_speed;

    private string m_answer;
    private bool m_isMoving;
    private int m_idRelation;
    private int m_movementFrequency;
    private Vector2 m_targetPosition;
    private Vector2 m_initArea;
    private Vector2 m_endArea;


    public void Init(string answer, int idRelation, Vector2 initArea, Vector2 endArea)
    {
        m_answer = answer;
        m_idRelation = idRelation;
        var animationName = "Demon" + idRelation;
        GetComponent<Animator>().Play(animationName);

        m_initArea = initArea;
        m_endArea = endArea;

        m_movementFrequency = FindObjectOfType<GameController>().DemonMovementFrequency;

        StartCoroutine(MoveRandomly());
    }

    private IEnumerator MoveRandomly()
    {
        while (true)
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
        // Destroy rock
        FindObjectOfType<CameraController>().StopFollowingRock();
        Destroy(collision.gameObject);

        var rockId = collision.gameObject.GetComponent<Rock>().RelationId;

        if (rockId == m_idRelation)
        {
            FindObjectOfType<GameController>().UpdateStateCorrectAnswer();
        }
        else
        {
            FindObjectOfType<GameController>().UpdateStateWrongAnswer();
        }

        Destroy(gameObject);
    }
}
