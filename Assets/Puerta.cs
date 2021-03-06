using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField] private GameObject puertaDestino;

    public void accion()
    {
        Interfaz interfaz = FindObjectOfType<Interfaz>();
        interfaz.setNextDoor(puertaDestino);
        StartCoroutine(interfaz.transition(false));
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
