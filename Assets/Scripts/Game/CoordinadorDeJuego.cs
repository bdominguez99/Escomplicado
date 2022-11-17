using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinadorDeJuego : MonoBehaviour
{
    private ManejadorGuardado manejadorGuardado;
    private InfoEntreEscenas infoEntreEscenas;
    private string nombreJugador;
    private float tiempoJuego;
    private float puntuacionFinal;
    private bool isPlaying = true;
    private float[] scores;

    void Start()
    {
        infoEntreEscenas = FindObjectOfType<InfoEntreEscenas>();
        if(infoEntreEscenas == null)
        {
            GameObject aux = Instantiate(new GameObject("InfoEntreEscenas"), Vector3.zero, Quaternion.identity);
            aux.AddComponent<InfoEntreEscenas>();
            infoEntreEscenas = aux.GetComponent<InfoEntreEscenas>();
            infoEntreEscenas.idPartida = 0;
            infoEntreEscenas.nombreJugador = "Test";
        }
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
        if(nombreJugador != null && !nombreJugador.Equals(""))
        {
            return nombreJugador;
        }
        return infoEntreEscenas.nombreJugador;
    }

    public float[] getScores()
    {
        if (scores == null)
        {
            scores = new float[6];
            for(int i = 0; i < 6; i++)
            {
                scores[i] = -1f;
            }
            return scores;
        }
        return scores;
    }

    public float getPuntuacionFinal()
    {
        return puntuacionFinal;
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
            if(minijuegoId == 5)
            {
                for (int i = 0; i < scores.Length; i++) {
                    puntuacionFinal += scores[i];
                }
                puntuacionFinal *= 1000;
            }
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
