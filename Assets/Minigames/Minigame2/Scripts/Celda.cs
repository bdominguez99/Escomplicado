using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TripasDeGato
{
    public enum TipoColor { Default, Pintando, Pintado, Borde };

    public class Celda : MonoBehaviour
    {
        [SerializeField] private Color m_colorBase;
        [SerializeField] private Color m_colorOffset;
        [SerializeField] private Color m_colorDibujo;
        [SerializeField] private SpriteRenderer m_spriteRenderer;

        private ControladorLineas m_controladorLineas;
        private GameObject m_referenciaPopUp;
        private float m_tamanioCelda;
        private Color m_colorDefault;

        public int Id { get; private set; }

        public int IdNodoPar { get; private set; }

        public string Texto { get; private set; }

        public Vector2 Posicion { get; private set; }

        public bool EsNodo { get; set; } = false;

        public void Init(bool esNodo, Vector2 posicion, float tamanioCelda, ControladorLineas controladorLineas, GameObject referenciaPopUp)
        {
            m_controladorLineas = controladorLineas;
            m_referenciaPopUp = referenciaPopUp;
            EsNodo = esNodo;
            Posicion = posicion;
            m_tamanioCelda = tamanioCelda;
            Texto = "";
            Id = -1;

            // Cambiar el tamaño de la celda
            this.transform.localScale = new Vector3(tamanioCelda, tamanioCelda, 1);

            PintarCelda(TipoColor.Default);
        }

        public void InitNodo(bool esNodo, Color color, string texto, int idNodo, int idNodoPar)
        {
            EsNodo = esNodo;
            Texto = texto;
            IdNodoPar = idNodoPar;
            m_colorDefault = color;
            Id = idNodo;
            PintarCelda(TipoColor.Default);
        }

        public void PintarCelda(TipoColor tipoColor)
        {
            switch (tipoColor)
            {
                case TipoColor.Default:
                    var esOffset = (Posicion.x / m_tamanioCelda) % 2 != (Posicion.y / m_tamanioCelda) % 2;
                    if (!EsNodo)
                    {
                        m_colorDefault = esOffset ? m_colorOffset : m_colorBase;
                        m_spriteRenderer.color = esOffset ? m_colorOffset : m_colorBase;
                    }
                    else
                    {
                        m_spriteRenderer.color = m_colorDefault;
                    }
                    m_colorDefault = esOffset ? m_colorOffset : m_colorBase;
                    m_spriteRenderer.color = esOffset ? m_colorOffset : m_colorBase;
                    break;
                case TipoColor.Pintado:
                    m_spriteRenderer.color = m_colorDibujo;
                    break;
                case TipoColor.Pintando:
                    m_spriteRenderer.color = m_colorDibujo;
                    break;
                default:
                    m_spriteRenderer.color = Color.red;
                    break;
            }
        }

        private void OnMouseDown()
        {
            if (EsNodo && !m_referenciaPopUp.activeSelf)
            {
                m_controladorLineas.IniciarNuevaLinea(Posicion);
            }
        }

        private void OnMouseEnter()
        {
            ControladorJuego.UltimaCeldaTocada = this;

            if (Input.GetMouseButton(0) && m_controladorLineas.DibujoHabilitado)
            {
                if (EsNodo)
                {
                    m_controladorLineas.TerminarLinea(this, true);
                }
                else if (m_controladorLineas.EsCeldaLibre(Posicion))
                {

                    PintarCelda(TipoColor.Pintado);
                    m_controladorLineas.AniadirALinea(Posicion);
                }
                else
                {
                    m_controladorLineas.TerminarLinea(this, true);
                }
            }
        }

        private void OnMouseUp()
        {
            var celdaFinal = ControladorJuego.UltimaCeldaTocada ?? this;

            m_controladorLineas.TerminarLinea(celdaFinal);
        }
    }
}

