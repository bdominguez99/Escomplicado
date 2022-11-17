using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementoPuntuacion : MonoBehaviour
{
    [SerializeField] private Text m_nombre;
    [SerializeField] private Text m_puntuacion;
    [SerializeField] private Text m_tiempo;

    public void Init(string nombre, string puntuacion, string tiempo)
    {
        m_nombre.text = nombre;
        m_puntuacion.text = puntuacion;
        m_tiempo.text = tiempo;
    }
}
