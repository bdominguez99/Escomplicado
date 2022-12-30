using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    [SerializeField] private bool isDoorOutside;
    [SerializeField] private int targetFloor = -1;
    [SerializeField] private GameObject puertaDestino;

    private Mapa mapa;

    private void Start()
    {
        mapa = FindObjectOfType<Mapa>();
    }

    public void accion()
    {
        Interfaz interfaz = FindObjectOfType<Interfaz>();
        interfaz.setNextDoor(puertaDestino);
        StartCoroutine(interfaz.transition(false, !isDoorOutside));
        if(isDoorOutside) FindObjectOfType<IndicadorDireccion>().setArrowVisibility(false);
        FindObjectOfType<AudioMenu>().playDoor();
    }

    public int getTargetFloor()
    {
        return targetFloor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (targetFloor == -1 || mapa.getActiveFloor() == targetFloor))
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
