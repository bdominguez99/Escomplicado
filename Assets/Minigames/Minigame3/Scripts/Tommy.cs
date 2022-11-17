using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tommy : MonoBehaviour
{
    [SerializeField] private List<LineRenderer> m_lineRenderers;
    [SerializeField] private List<Transform> m_stringPositions;
    [SerializeField] private Transform m_center;
    [SerializeField] private Transform m_idlePosition;
    [SerializeField] private float m_maxStringLenght;
    [SerializeField] private GameObject m_rockPrefab;
    [SerializeField] private float m_rockPositionOffset;
    [SerializeField] private float m_rockForce;
    
    [SerializeField] private GameObject m_predictionDotPrefab;
    [SerializeField] private GameObject m_predictionDotsParent;
    [SerializeField] private int m_predictionPoints;
    [SerializeField] private float m_deltaTimeSimulation;

    private bool m_isMouseDown;
    private Vector2 m_currentPosition;
    private CameraController m_CameraController;

    // Rock
    private Sprite m_rockSprite;
    private int m_idLastRockSelected;
    private GameObject m_rock;
    private Rigidbody2D m_rockRigidBody;
    private Collider2D m_rockCollider;

    private const float GRAVITY = 9.8f;
    

    // Start is called before the first frame update
    void Start()
    {
        m_CameraController = FindObjectOfType<CameraController>();
        m_lineRenderers[0].positionCount = 2;
        m_lineRenderers[0].SetPosition(0, m_stringPositions[0].position);
        CreateRock();

        var initialScale = m_predictionDotPrefab.transform.localScale.x;
        for (int i = 0; i < m_predictionPoints; i++)
        {
            var newPredictionPoint = Instantiate(m_predictionDotPrefab);
            newPredictionPoint.transform.SetParent(m_predictionDotsParent.transform);

            var dotScale = initialScale / (float)(1 + i*0.1f);
            newPredictionPoint.transform.localScale = new Vector3(dotScale, dotScale, dotScale);
        }
        m_predictionDotsParent.SetActive(false);
    }

    void Update()
    {
        if (m_isMouseDown)
        {
            var mousePosition = Input.mousePosition;

            m_currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            m_currentPosition = (Vector2)m_center.position + 
                Vector2.ClampMagnitude(m_currentPosition - (Vector2)m_center.position, m_maxStringLenght);

            DrawPredictionDots();
            SetStrings(m_currentPosition);

            if (m_rockCollider)
            {
                m_rockCollider.enabled = true;
            }
        }
        else
        {
            ResetString();
            ErasePredictionDots();
        }
    }

    private void DrawPredictionDots()
    {
        if (!m_predictionDotsParent.activeSelf)
        {
            m_predictionDotsParent.SetActive(true);
        }
        var direction = ((Vector2)m_center.position - m_currentPosition);
        var rockForce = direction * m_rockForce;
        m_rockRigidBody.velocity = rockForce;
        DrawTrajectory(m_center.position, rockForce);
    }

    private void ErasePredictionDots()
    {
        m_predictionDotsParent.SetActive(false);
        
    }

    private float VerticalPosition (float initialPosition, float initialVelocity, float time, float acceleration)
    {
        var position = initialPosition + initialVelocity * time - (1f / 2f) * acceleration * time * time;
        return position;
    }

    private float HorizontalPosition(float initialPosition, float initialVelocity, float time)
    {
        var position = initialPosition + initialVelocity * time;
        return position;
    }

    private void DrawTrajectory(Vector2 initialPoint, Vector2 velocity)
    {
        var xo = initialPoint.x;
        var yo = initialPoint.y;

        for (float i = 1; i <= m_predictionPoints; i++)
        {
            var auxTime = i * m_deltaTimeSimulation;
            var xComponent = HorizontalPosition(xo, velocity.x, auxTime);
            var yComponent = VerticalPosition(yo, velocity.y, auxTime, GRAVITY);

            var predictionDot = m_predictionDotsParent.transform.GetChild((int)i - 1);
            predictionDot.transform.position = new Vector2(xComponent, yComponent);
        }
    }

    void Shoot()
    {
        m_rockRigidBody.isKinematic = false;
        var rockForce = ((Vector2)m_center.position - m_currentPosition) * m_rockForce;
        m_rockRigidBody.velocity = rockForce;

        m_CameraController.FollowRock(m_rock);

        m_rock.GetComponent<Rock>().IsThrown = true;

        m_rock = null;
        m_rockCollider = null;
        Invoke("CreateRock", 2);
    }

    void CreateRock()
    {
        m_rock = Instantiate(m_rockPrefab);
        m_rock.GetComponent<Rock>().UpdateRock(m_idLastRockSelected, m_rockSprite);
        m_rockRigidBody = m_rock.GetComponent<Rigidbody2D>();
        m_rockCollider = m_rock.GetComponent<Collider2D>();

        m_rockCollider.enabled = false;

        m_rockRigidBody.isKinematic = true;

        ResetString();
    }

    public void UpdateRockType(int idRelation, Sprite rockSprite)
    {
        m_rockSprite = rockSprite;
        m_idLastRockSelected = idRelation;
        Debug.Log("Asignado: " + m_idLastRockSelected);

        Destroy(m_rock);
        m_rock = null;
        if (m_rock == null)
        {
            CreateRock();
        }
        var rock = m_rock.GetComponent<Rock>();
        Debug.Log("IdElementAnteFinal: " + rock.RelationId);
        rock.UpdateRock(idRelation, rockSprite);
        Debug.Log("IdElementFinal: " + rock.RelationId);
    }

    void ResetString()
    {
        m_currentPosition = m_idlePosition.position;
        SetStrings(m_currentPosition);
    }

    private void OnMouseDown()
    {
        m_isMouseDown = true;
        FindObjectOfType<GameController>().EnableCameraMovement = false;
    }

    private void OnMouseUp()
    {
        m_isMouseDown = false;
        FindObjectOfType<GameController>().EnableCameraMovement = true;
        Shoot();
    }

    void SetStrings(Vector2 position)
    {
        m_lineRenderers[0].SetPosition(1, position);

        if (m_rock != null)
        {
            var rockDirection = position - (Vector2)m_center.position;
            m_rock.transform.position = position + rockDirection.normalized * m_rockPositionOffset;
            m_rock.transform.right = -rockDirection.normalized;
        }
    }
}
