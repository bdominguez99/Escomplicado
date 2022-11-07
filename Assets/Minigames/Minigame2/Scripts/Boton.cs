using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TripasDeGato
{
    public class Boton : MonoBehaviour
    {
        private void OnMouseDown()
        {
            Debug.Log("Click");
            FindObjectOfType<ControladorJuego>().PopUpActivo = false;
        }
    }
}
