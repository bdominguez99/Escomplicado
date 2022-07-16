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
}
