using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TripasDeGato
{
    public class Reloj : MonoBehaviour
    {
        [SerializeField] private Text m_cronometro;
        [SerializeField] private Text m_textoPenalizacion;
        [SerializeField] private int m_penalizacion;
        [SerializeField] private int m_tiempoLimite;
        [SerializeField] private int m_iteracionesAnimacionPenalizacion;
        [SerializeField] private float m_esperaAnimacion;

        private int m_tiempoTranscurrido;
        private bool m_estaCorriendo;

        private void Start()
        {
            m_tiempoTranscurrido = 0;
            m_estaCorriendo = true;
            m_textoPenalizacion.text = "";
            StartCoroutine(CorrerReloj());
        }

        public void DetenerReloj()
        {
            m_estaCorriendo = false;
        }

        public void IniciarReloj()
        {
            m_estaCorriendo = true;
        }

        public void IncrementarTiempo()
        {
            m_tiempoTranscurrido += m_penalizacion;
            MostrarPenalización();
        }

        private IEnumerator CorrerReloj()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1);
                if (m_estaCorriendo)
                {
                    if (m_tiempoTranscurrido >= m_tiempoLimite)
                    {
                        m_tiempoTranscurrido = 0;
                        var manejadorDeVidas = GameObject.FindObjectOfType<ManejadorVidas>();
                        manejadorDeVidas.QuitarVida();
                    }
                    else
                    {
                        m_tiempoTranscurrido++;
                    }
                }
                m_cronometro.text = m_tiempoTranscurrido.ToString();
            }
        }

        private void MostrarPenalización()
        {
            StartCoroutine(MostrarPenalizacionRutina());
        }

        private IEnumerator MostrarPenalizacionRutina()
        {
            m_textoPenalizacion.text = "+" + m_penalizacion;
            var colorTextoOpaco = m_textoPenalizacion.color;
            colorTextoOpaco.a = 1;
            m_textoPenalizacion.color = colorTextoOpaco;
            for (int i = 0; i < m_iteracionesAnimacionPenalizacion; i++)
            {
                var colorTexto = m_textoPenalizacion.color;
                colorTexto.a -= (float)1 / (float)m_iteracionesAnimacionPenalizacion;
                m_textoPenalizacion.color = colorTexto;
                yield return new WaitForSecondsRealtime(m_esperaAnimacion);
            }

            m_textoPenalizacion.text = "";
        }
    }
}