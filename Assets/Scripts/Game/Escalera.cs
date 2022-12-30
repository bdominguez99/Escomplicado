using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escalera : MonoBehaviour
{
    [SerializeField] private bool sonSubir;

    public void accion()
    {
        FindObjectOfType<AudioMenu>().playClimbStairs();
        if (sonSubir) FindObjectOfType<Mapa>().subir();
        else FindObjectOfType<Mapa>().bajar();
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
