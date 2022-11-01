using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinadorDeJuego : MonoBehaviour
{
    private ManejadorGuardado manejadorGuardado;
    private InfoEntreEscenas infoEntreEscenas;
    private string nombreJugador;
    private float tiempoJuego;
    private bool isPlaying = true;
    private float[] scores;

    void Start()
    {
        infoEntreEscenas = FindObjectOfType<InfoEntreEscenas>();
        manejadorGuardado = FindObjectOfType<ManejadorGuardado>();
        setGameVariables();
        verifyMinigameScores();
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

    public void setScores(float[] scores)
    {
        this.scores = scores;
    }

    public float getTiempoJuego()
    {
        return tiempoJuego;
    }

    public string getNombreJugador()
    {
        return nombreJugador;
    }

    public float[] getScores()
    {
        return scores;
    }

    private void verifyMinigameScores()
    {
        Minijuego minijuego = FindObjectOfType<Minijuego>();
        if(minijuego != null)
        {
            float score = minijuego.getScore();
            int minijuegoId = minijuego.getMinigameId();
            scores[minijuegoId] = score;
            Destroy(minijuego.gameObject);
            if(infoEntreEscenas != null)
                manejadorGuardado.guardarPartida(infoEntreEscenas.idPartida);
        }
    }

    private void setGameVariables()
    {
        if (infoEntreEscenas != null)
            manejadorGuardado.cargarPartida(infoEntreEscenas.idPartida);
    }
}
