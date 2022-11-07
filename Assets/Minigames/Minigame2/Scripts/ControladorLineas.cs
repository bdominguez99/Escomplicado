using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TripasDeGato
{
    public class ControladorLineas
    {
        public bool DibujoHabilitado { get; set; }

        private HashSet<Vector2> m_celdasPintadas;
        private Linea m_lineaActual;
        private ControladorPopUp m_controladorPopUp;

        public ControladorLineas(ControladorPopUp referenciaControladorPopUp)
        {
            m_celdasPintadas = new HashSet<Vector2>();
            m_controladorPopUp = referenciaControladorPopUp;
        }

        public void IniciarNuevaLinea(Vector2 posicionInicialCelda)
        {
            m_lineaActual = new Linea(posicionInicialCelda);
            DibujoHabilitado = true;
        }

        public void AniadirALinea(Vector2 posicionCeldaNueva)
        {
            if (m_lineaActual != null && EsCeldaLibre(posicionCeldaNueva))
            {
                m_lineaActual.AniadirCelda(posicionCeldaNueva);
            }
        }

        public void TerminarLinea(Celda celdaFinal, bool esOnMouseEnter = false)
        {
            if (m_lineaActual != null)
            {
                var celdaInicial = ControladorJuego.Celdas[m_lineaActual.PosicionCeldaInicial];
                var idCeldaFinal = celdaFinal.Id;
                if (
                    celdaFinal.EsNodo &&
                    celdaInicial.Id == idCeldaFinal &&
                    m_lineaActual.Celdas.Count == 1 &&
                    esOnMouseEnter)
                { // Para poder mantener dibujo sobre nodo
                    return;
                }
                else if (
                  celdaFinal.EsNodo &&
                  celdaInicial.Id != idCeldaFinal &&
                  celdaInicial.IdNodoPar == idCeldaFinal)
                { // Terminó de forma correcta
                    foreach (var posicionCelda in m_lineaActual.Celdas)
                    {
                        m_celdasPintadas.Add(posicionCelda);
                    }
                    DesactivarCeldasNodos(celdaInicial, celdaFinal);
                    GameObject.FindObjectOfType<ControladorJuego>().ActualizarEstado();
                }
                else if (
                    celdaFinal.EsNodo &&
                    celdaInicial.Id == idCeldaFinal &&
                    m_lineaActual.Celdas.Count == 1 &&
                    !esOnMouseEnter)
                { // Activa el pop-up

                    m_controladorPopUp.ActivarPopUp();
                }
                else
                { // Se equivocó
                    foreach (var posicionCelda in m_lineaActual.Celdas)
                    {
                        var celda = ControladorJuego.Celdas[posicionCelda];
                        celda.PintarCelda(TipoColor.Default);
                    }

                    var reloj = GameObject.FindObjectOfType<Reloj>();
                    reloj.IncrementarTiempo();
                }
            }

            m_lineaActual = null;
            DibujoHabilitado = false;
        }

        private void DesactivarCeldasNodos(Celda celdaInicial, Celda celdaFinal)
        {
            var celdasNodoInicial = ControladorJuego.Nodos[celdaInicial.Id].Celdas;
            foreach (var celda in celdasNodoInicial)
            {
                celda.EsNodo = false;
                celda.PintarCelda(TipoColor.Pintado);
                ControladorJuego.CeldasOcupadas.Add(celda.Posicion);
            }

            var celdasNodoFinal = ControladorJuego.Nodos[celdaFinal.Id].Celdas;
            foreach (var celda in celdasNodoFinal)
            {
                celda.PintarCelda(TipoColor.Pintado);
                ControladorJuego.CeldasOcupadas.Add(celda.Posicion);
                celda.EsNodo = false;
            }
        }

        public bool EsCeldaLibre(Vector2 posicionCelda)
        {
            return !ControladorJuego.CeldasOcupadas.Contains(posicionCelda) && !m_celdasPintadas.Contains(posicionCelda);
        }

        public void ReiniciarJuego()
        {
            m_celdasPintadas.Clear();
            m_lineaActual = null;
        }
    }

    public class Linea
    {
        public HashSet<Vector2> Celdas { get; private set; }
        public Vector2 PosicionCeldaInicial;

        public Linea(Vector2 posicionCeldaInicial)
        {
            Celdas = new HashSet<Vector2>() { posicionCeldaInicial };
            PosicionCeldaInicial = posicionCeldaInicial;
        }

        public void AniadirCelda(Vector2 posicionCelda)
        {
            Celdas.Add(posicionCelda);
        }
    }
}