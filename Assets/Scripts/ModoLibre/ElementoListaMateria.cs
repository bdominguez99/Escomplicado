using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementoListaMateria : MonoBehaviour
{
    [SerializeField] private Text m_nombreMateriaText;

    private string m_nombreMateria;
    private HashSet<TipoMinijuego> m_tipoMinijuego;

    public void Init(string materia, HashSet<TipoMinijuego> tipoMinijuego)
    {
        m_nombreMateriaText.text = materia;
        m_nombreMateria = materia;
        m_tipoMinijuego = tipoMinijuego;
    }

    public void SetMateria()
    {
        FindObjectOfType<ControladorModoLibre>().MostrarMinijuegos(m_nombreMateria, m_tipoMinijuego);
    }
}
