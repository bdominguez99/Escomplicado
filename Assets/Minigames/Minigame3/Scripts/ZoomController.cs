using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private float m_zoomSpeed;
    [SerializeField] private float m_zoomFactor;

    private float m_zoom;
    
    void Start()
    {
        m_zoom = m_mainCamera.orthographicSize;
    }

    public void ZoomIn()
    {
        m_zoom -= m_zoomFactor;
        //m_mainCamera.orthographicSize = Mathf.Lerp(m_mainCamera.orthographicSize, m_zoom, Time.deltaTime * m_zoomSpeed);
        m_mainCamera.orthographicSize = m_zoom;
    }

    public void ZoomOut()
    {
        m_zoom += m_zoomFactor;
        //m_mainCamera.orthographicSize = Mathf.Lerp(m_mainCamera.orthographicSize, m_zoom, Time.deltaTime * m_zoomSpeed);
        m_mainCamera.orthographicSize = m_zoom;
    }
}
