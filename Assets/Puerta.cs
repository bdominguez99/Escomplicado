using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField] public GameObject puertaDestino;
    [SerializeField] public bool esEntradaASalon = true;
    public bool esIzquierda;

    public Puerta(bool esIzquierda, bool esEntradaASalon)
    {
        this.esIzquierda = esIzquierda;
        this.esEntradaASalon = esEntradaASalon;
    }

    public void accion()
    {
        // Ponemos como destino de la puerta a la que vamos, la puerta
        // desde la que se activa la accion para poder regresar.
        if (esEntradaASalon)
        {
            if (esIzquierda)
            {
                var puertaSalonIzquierda = GameObject.Find("PuertaSalonDeIzquierda");
                puertaSalonIzquierda.GetComponent<Puerta>().puertaDestino = this.gameObject;
            }
            else
            {
                var puertaSalonDerecha = GameObject.Find("PuertaSalonDeDerecha");
                puertaSalonDerecha.GetComponent<Puerta>().puertaDestino = this.gameObject;
            }
        }
        
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
