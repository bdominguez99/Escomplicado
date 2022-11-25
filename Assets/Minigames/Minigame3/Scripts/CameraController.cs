using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float m_velocityThreshold;
    [SerializeField] private Camera cam;
    [SerializeField] private float m_zoomStep;
    [SerializeField] private float m_maxZoom;
    [SerializeField] private float m_minZoom;
    [SerializeField] private SpriteRenderer m_backgroundSpriteRenderer;
    [SerializeField] private GameController m_gameController;

    private Vector3 m_defaultPosition;
    private GameObject m_rock;
    private bool m_followRock;
    private Vector3 m_mouseOriginPosition;

    private Vector2 m_maxCamPosition;
    private Vector2 m_minCamPosition;

    private void Start()
    {
        m_maxCamPosition = new Vector2();
        m_minCamPosition = new Vector2();

        m_defaultPosition = transform.position;

        var bgTransformPosition = m_backgroundSpriteRenderer.transform.position;
        var bgBoundsSize = m_backgroundSpriteRenderer.bounds.size;

        m_minCamPosition.x = bgTransformPosition.x - bgBoundsSize.x / 2f;
        m_maxCamPosition.x = bgTransformPosition.x + bgBoundsSize.x / 2f;

        m_minCamPosition.y = bgTransformPosition.y - bgBoundsSize.y / 2f;
        m_maxCamPosition.y = bgTransformPosition.y + bgBoundsSize.y / 2f;
    }

    void Update()
    {
        if (m_followRock && m_rock != null)
        {
            var rockPosition = m_rock.transform.position;
            transform.position = ClampCamera(new Vector3(rockPosition.x, rockPosition.y, m_defaultPosition.z));
        }
        else if (m_gameController.EnableCameraMovement)
        {
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_mouseOriginPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = m_mouseOriginPosition - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - m_zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, m_minZoom, m_maxZoom);

        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + m_zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, m_minZoom, m_maxZoom);

        cam.transform.position = ClampCamera(cam.transform.position);
    }

    public void FollowRock(GameObject rock)
    {
        m_followRock = true;
        m_rock = rock;
    }

    public void StopFollowingRock()
    {
        m_followRock = false;
        cam.transform.position = ClampCamera(m_defaultPosition);
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = m_minCamPosition.x + camWidth;
        float maxX = m_maxCamPosition.x - camWidth;
        float minY = m_minCamPosition.y + camHeight;
        float maxY = m_maxCamPosition.y - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
