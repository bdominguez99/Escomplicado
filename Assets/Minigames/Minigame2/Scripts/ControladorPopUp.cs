using System.Collections.Generic;
using UnityEngine;

namespace TripasDeGato
{
    public class ControladorPopUp
    {
        private GameObject m_popUpReference;
        private GameObject m_celdas;

        public ControladorPopUp(GameObject popUpReference, GameObject celdas)
        {
            m_popUpReference = popUpReference;
            m_popUpReference.SetActive(false);
            m_celdas = celdas;
        }

        public void ActivarPopUp()
        {
            m_popUpReference.SetActive(true);
            GameObject.FindObjectOfType<ControladorJuego>().PopUpActivo = true;
            m_celdas.SetActive(false);
        }
        public void AniadirElemento(GameObject elementoLista, GameObject listaPadre)
        {
            elementoLista.transform.SetParent(listaPadre.transform);
            elementoLista.transform.localScale = new Vector3(1, 1, 1);
        }

        public void RemoverElementos(GameObject listaPadre)
        {
            var transformHijos = new List<Transform>();
            for (int i = 0; i < listaPadre.transform.childCount; i++)
            {
                transformHijos.Add(listaPadre.transform.GetChild(i));
            }
            listaPadre.transform.DetachChildren();
            foreach (var transform in transformHijos)
            {
                GameObject.Destroy(transform.gameObject);
            }
        }
    }

}