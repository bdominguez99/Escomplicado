using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TipoMinijuego { OpcionMultiple, Ordenamiento, Relacion};

public class MiniaturaMinijuego : MonoBehaviour
{
    [SerializeField] private Image m_referenciaImagen;
    [SerializeField] private Text m_textoNombre;
    
    public TipoMinijuego TipoMinijuego { get; private set; }
    private int m_numeroMinijuego;

    public void Init(Sprite miniatura, string nombre, TipoMinijuego tipo, int numeroMinijuego)
    {
        m_referenciaImagen.sprite = miniatura;
        m_textoNombre.text = nombre;
        TipoMinijuego = tipo;
        m_numeroMinijuego = numeroMinijuego;
    }

    public void SeleccionarMinijuego()
    {
        FindObjectOfType<ControladorModoLibre>().SeleccionarMinijuego(m_numeroMinijuego);
    }
}
