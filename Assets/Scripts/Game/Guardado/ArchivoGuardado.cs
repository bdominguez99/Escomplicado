using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArchivoGuardado
{
    public Partida[] partidas;

    public ArchivoGuardado()
    {
        partidas = new Partida[3];
    }

    public string toString()
    {
        string s = "";
        for (int i = 0; i < partidas.Length; i++)
        {
            if (partidas[i] != null)
            {
                s += "Partida " + i + ": " + partidas[i].toString() + ". ";
            }
        }
        return s;
    }
}
