using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{
    [SerializeField] public GameObject[] pisos;
    public GameObject puerta;    
    private int pisoActual, pisoSiguiente;

    public int PisoActual {get => pisoActual;}

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

}
