using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejadorGuardado : MonoBehaviour
{
    private GuardadoGenerico<ArchivoGuardado> guardado = new GuardadoGenerico<ArchivoGuardado>();
    private ArchivoGuardado archivoGuardado;
    public static readonly string fileName = "SaveFile.dat";

    public void guardarPartida(int idPartida)
    {
        Partida partida = new Partida();
        Vector2 playerPos = FindObjectOfType<PlayerController>().transform.position;
        partida.posx = playerPos.x;
        partida.posy = playerPos.y;
        CoordinadorDeJuego coordinador = FindObjectOfType<CoordinadorDeJuego>();
        partida.tiempoJuego = coordinador.getTiempoJuego();
        partida.nombre = coordinador.getNombreJugador();
        archivoGuardado = guardado.Load(fileName);
        if(archivoGuardado == default)
        {
            archivoGuardado = new ArchivoGuardado();
        }
        archivoGuardado.partidas[idPartida] = partida;
        guardado.Save(archivoGuardado, fileName);
    }

    public void cargarPartida(int idPartida)
    {
        archivoGuardado = guardado.Load(fileName);
        if (archivoGuardado != default)
        {
            Partida partida = archivoGuardado.partidas[idPartida];
            CoordinadorDeJuego coordinador = FindObjectOfType<CoordinadorDeJuego>();
            if (partida != default)
            {
                coordinador.setTiempoJuego(partida.tiempoJuego);
                if (partida.nombre != null)
                    coordinador.setNombreJugador(partida.nombre);
                else
                    coordinador.setNombreJugador(FindObjectOfType<InfoEntreEscenas>().nombreJugador);
                FindObjectOfType<PlayerController>().transform.position = new Vector2(partida.posx, partida.posy);
            }
            else
            {
                coordinador.setTiempoJuego(0);
                coordinador.setNombreJugador(FindObjectOfType<InfoEntreEscenas>().nombreJugador);
            }
        }
    }
}
