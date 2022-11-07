using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TripasDeGato
{
    public class ManejadorMarcador : MonoBehaviour
    {
        [SerializeField] private Text m_textoMarcador;

        private int m_totalPreguntas;
        private int m_preguntasResueltas;

        // Start is called before the first frame update
        void Start()
        {
            m_preguntasResueltas = 0;
        }

        private void Update()
        {
            m_textoMarcador.text = m_preguntasResueltas + "/" + m_totalPreguntas;
        }

        public void SetPreguntasResueltas(int preguntasResueltas)
        {
            m_preguntasResueltas = preguntasResueltas;
        }

        public void Reiniciar()
        {
            m_preguntasResueltas = 0;
        }

        public void SetTotalPreguntas(int totalPreguntas)
        {
            m_totalPreguntas = totalPreguntas;
        }

        public void Desactivar()
        {
            m_textoMarcador.text = "";
        }
    }
}