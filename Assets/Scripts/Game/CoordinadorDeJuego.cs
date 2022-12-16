using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinadorDeJuego : MonoBehaviour
{
    [SerializeField] private AudioClip main;

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

    public int getActualMinigame()
    {
        float[] scores = getScores();
        for(int i = 0; i < 6; i++)
        {
            if (scores[i] < 0.6f)
            {
                return i;
            }
        }
        return 6;
    }

    public float getPuntuacionFinal()
    {
        return puntuacionFinal;
    }

    public void stopMusic()
    {
        FindObjectOfType<InfoEntreEscenas>().gameObject.GetComponent<AudioSource>().Stop();
    }

    private void verifyMinigameScores()
    {
        AudioSource mainSource = infoEntreEscenas.gameObject.GetComponent<AudioSource>();
        Minijuego minijuego = FindObjectOfType<Minijuego>();
        if(mainSource.isPlaying) mainSource.Stop();
        mainSource.clip = main;
        mainSource.Play();
        if(minijuego != null)
        {
            float score = minijuego.getScore();
            int minijuegoId = minijuego.getMinigameId();
            scores[minijuegoId] = score;
            Destroy(minijuego.gameObject);
            FindObjectOfType<IndicadorDireccion>().setMinigame(minijuegoId, score);
            if(minijuegoId == 5)
            {
                for (int i = 0; i < scores.Length; i++) {
                    puntuacionFinal += scores[i];
                }
                puntuacionFinal *= 1000;
            }
            if(infoEntreEscenas != null)
                manejadorGuardado.guardarPartida(infoEntreEscenas.idPartida);
            if(minijuegoId == 5)
            {
                FindObjectOfType<Interfaz>().loadScene("CompleteGame");
            }
        }
    }

    private void setGameVariables()
    {
        if (infoEntreEscenas != null)
            manejadorGuardado.cargarPartida(infoEntreEscenas.idPartida);
    }
}
