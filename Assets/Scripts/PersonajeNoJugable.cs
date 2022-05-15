using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeNoJugable : MonoBehaviour
{
    [SerializeField] public int numeroPersonaje;
    public Dialogo dialogo;

    public void accion ()
    {
        FindObjectOfType<ManejadorDeDialogos>().IniciarDialogo(dialogo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<Interfaz>().mostrarInterfaz(Interfaz.Interfaces.Intractuar);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<Interfaz>().mostrarInterfaz(Interfaz.Interfaces.Mapa);
        }
    }
}
