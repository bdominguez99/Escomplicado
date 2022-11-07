using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Partida
{
    public float posx;
    public float posy;
    public int pisoActual;
    public string nombre;
    public float tiempoJuego;
    public float[] scores;
    public float puntuacionFinal;
    public string fechaTermino;

    public Partida()
    {
        posx = 0f;
        posy = 0f;
        pisoActual = 0;
        nombre = "";
        tiempoJuego = 0f;
        scores = new float[6];
        for(int i = 0; i < 6; i++)
        {
            scores[i] = -1f;
        }
        puntuacionFinal = 0f;
        fechaTermino = "";
    }

    public string toString()
    {
        return "{pos: (" + posx + ", " + posy + "), piso: " + pisoActual + ", nombre: " + nombre + ", tiempo: " + tiempoJuego + ", scores: [" + scores[0] + ", " + scores[1] + ", " + scores[2] + ", " + scores[3] + ", " + scores[4] + ", " + scores[5] + "], fecha: " + fechaTermino + "}";
    }
}
