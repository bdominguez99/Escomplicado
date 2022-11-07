using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TripasDeGato
{
    public class Nodo
    {
        public List<Celda> Celdas { get; private set; }

        public Celda CeldaCentral { get; set; }

        public Nodo(List<Celda> celdas, Celda celdaCentral)
        {
            Celdas = celdas;
            CeldaCentral = CeldaCentral;
        }

        public void DesactivarNodo()
        {
            foreach (var celda in Celdas)
            {
                celda.EsNodo = false;
                celda.PintarCelda(TipoColor.Pintado);
            }
        }
    }
}