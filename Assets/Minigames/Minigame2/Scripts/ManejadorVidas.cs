using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TripasDeGato
{
    public class ManejadorVidas : MonoBehaviour
    {
        [SerializeField] private GameObject m_contenedorVidas;
        [SerializeField] private GameObject m_vidaPrefab;
        [SerializeField] private int m_numeroDeVidas;
        [SerializeField] private float m_separacion;

        private float m_anchoVida;
        private List<GameObject> m_vidas;

        void Start()
        {
            m_anchoVida = m_vidaPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            m_vidas = new List<GameObject>();
            var posicionVidas = m_contenedorVidas.transform.position;
            var offset = new Vector3(m_anchoVida + m_separacion, 0, 0);

            for (int i = 0; i < m_numeroDeVidas; i++)
            {
                var nuevaVida = Instantiate(m_vidaPrefab, posicionVidas + i * offset, Quaternion.identity);
                nuevaVida.transform.parent = m_contenedorVidas.transform;
                m_vidas.Add(nuevaVida);
            }
        }

        public bool QuitarVida()
        {
            if (m_vidas.Count <= 0)
            {
                FindObjectOfType<ControladorJuego>().FinalizarJuego();
                return false;
            }
            int indexUltimaVida = m_vidas.Count - 1;
            var vida = m_vidas[indexUltimaVida];
            var animator = vida.GetComponent<Animator>();
            animator.Play("LoseLife");
            m_vidas.RemoveAt(indexUltimaVida);

            Destroy(vida, 0.9f);
            return m_vidas.Count <= 0;
        }
    }
}