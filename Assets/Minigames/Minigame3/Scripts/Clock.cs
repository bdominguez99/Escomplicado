using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    [SerializeField] private Text m_counterText;
    [SerializeField] private Text m_penaltyText;
    [SerializeField] private int m_penaltyAnimationIterations;
    [SerializeField] private float m_waitTime;
    
    public int Penalty { get; set; }

    public int TimeLimit { get; set; }

    private int m_elapsedTime;
    private bool m_isRunning;

    private void Start()
    {
        m_elapsedTime = 0;
        m_isRunning = true;
        m_penaltyText.text = "";
        StartCoroutine(ClockRoutine());
    }

    public void StopClock()
    {
        m_isRunning = false;
    }

    public void StartClock()
    {
        m_isRunning = true;
    }

    public void IncreaseTime()
    {
        m_elapsedTime += Penalty;
        ShowPenalty();
    }

    private IEnumerator ClockRoutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            if (m_isRunning)
            {
                if (m_elapsedTime >= TimeLimit)
                {
                    m_elapsedTime = 0;
                    // Find live manager and decrease lives
                    // var manejadorDeVidas = GameObject.FindObjectOfType<ManejadorVidas>();
                    // manejadorDeVidas.QuitarVida();
                    FindObjectOfType<GameController>().NextPhase();
                }
                else
                {
                    m_elapsedTime++;
                }
            }
            m_counterText.text = m_elapsedTime.ToString();
        }
    }

    private void ShowPenalty()
    {
        StartCoroutine(ShowPenaltyRoutine());
    }

    private IEnumerator ShowPenaltyRoutine()
    {
        
        m_penaltyText.text = "+" + Penalty;
        var colorTextoOpaco = m_penaltyText.color;
        colorTextoOpaco.a = 1;
        m_penaltyText.color = colorTextoOpaco;
        for (int i = 0; i < m_penaltyAnimationIterations; i++)
        {
            var colorTexto = m_penaltyText.color;
            colorTexto.a -= (float)1 / (float)m_penaltyAnimationIterations;
            m_penaltyText.color = colorTexto;
            yield return new WaitForSecondsRealtime(m_waitTime);
        }

        m_penaltyText.text = "";
    }
}
