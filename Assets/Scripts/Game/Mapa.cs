using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{
    [SerializeField] private GameObject[] pisos;
    private int pisoActual, pisoSiguiente;

    public void cambiarPiso()
    {
        pisos[pisoActual].SetActive(false);
        pisos[pisoSiguiente].SetActive(true);
        pisoActual = pisoSiguiente;
    }

    public void subir()
    {
        pisoSiguiente = pisoActual + 1;
        StartCoroutine(FindObjectOfType<Interfaz>().transition(true));
    }

    public void bajar()
    {
        pisoSiguiente = pisoActual - 1;
        StartCoroutine(FindObjectOfType<Interfaz>().transition(true));
    }

    public int getActiveFloor()
    {
        return pisoActual;
    }

    public void setActiveFloor(int activeFloor)
    {
        foreach (var piso in pisos)
        {
            piso.SetActive(false);
        }
        pisos[activeFloor].SetActive(true);
        pisoActual = activeFloor;
    }
}
