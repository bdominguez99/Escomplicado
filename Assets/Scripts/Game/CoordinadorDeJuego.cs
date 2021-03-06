using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinadorDeJuego : MonoBehaviour
{
    private string nombreJugador;
    private float tiempoJuego;
    private bool isPlaying = true;

    void Start()
    {
        setGameVariables();
    }

    void Update()
    {
        if (isPlaying)
            tiempoJuego += Time.deltaTime;
    }

    public void setTiempoJuego(float tiempoJuego)
    {
        this.tiempoJuego = tiempoJuego;
    }

    public void setNombreJugador(string nombreJugador)
    {
        this.nombreJugador = nombreJugador;
    }

    public void setPlayingState(bool isPlaying)
    {
        this.isPlaying = isPlaying;
    }

    public float getTiempoJuego()
    {
        return tiempoJuego;
    }

    public string getNombreJugador()
    {
        return nombreJugador;
    }

    private void setGameVariables()
    {
        InfoEntreEscenas infoEntreEscenas = FindObjectOfType<InfoEntreEscenas>();
        if (infoEntreEscenas != null)
            FindObjectOfType<ManejadorGuardado>().cargarPartida(infoEntreEscenas.idPartida);
    }
}
