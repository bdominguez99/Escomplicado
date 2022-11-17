using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejadorGuardado : MonoBehaviour
{
    private GuardadoGenerico<ArchivoGuardado> guardado;
    private ArchivoGuardado archivoGuardado;
    public static readonly string fileName = "SaveFile.dat";

    private void Start()
    {
        guardado = new GuardadoGenerico<ArchivoGuardado>();
    }

    public void guardarPartida(int idPartida)
    {
        Partida partida = new Partida();
        Vector2 playerPos = FindObjectOfType<PlayerController>().transform.position;
        partida.posx = playerPos.x;
        partida.posy = playerPos.y;
        CoordinadorDeJuego coordinador = FindObjectOfType<CoordinadorDeJuego>();
        partida.tiempoJuego = coordinador.getTiempoJuego();
        partida.nombre = coordinador.getNombreJugador();
        partida.scores = coordinador.getScores();
        partida.puntuacionFinal = coordinador.getPuntuacionFinal();
        partida.pisoActual = FindObjectOfType<Mapa>().getActiveFloor();
        archivoGuardado = guardado.Load(fileName);
        if(archivoGuardado == default)
        {
            archivoGuardado = new ArchivoGuardado();
        }
        if (partida.puntuacionFinal > 0)
        {
            archivoGuardado.historialPuntuaciones.Add(partida.nombre + "\t" + partida.puntuacionFinal + "\t" + partida.tiempoJuego / 60);
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
                coordinador.setScores(partida.scores);
                FindObjectOfType<Mapa>().setActiveFloor(partida.pisoActual);
                if (partida.nombre != null)
                    coordinador.setNombreJugador(partida.nombre);
                else
                    coordinador.setNombreJugador(FindObjectOfType<InfoEntreEscenas>().nombreJugador);
                FindObjectOfType<PlayerController>().transform.position = new Vector2(partida.posx, partida.posy);
                int minijuegoActual = 6;
                for(int i = 0; i < partida.scores.Length; i++)
                {
                    if(partida.scores[i] < 0.6f)
                    {
                        minijuegoActual = i;
                        break;
                    }
                }
                FindObjectOfType<IndicadorDireccion>().setMinijuego(minijuegoActual);
            }
            else
            {
                coordinador.setTiempoJuego(0);
                coordinador.setNombreJugador(FindObjectOfType<InfoEntreEscenas>().nombreJugador);
                float[] scores = new float[6];
                for (int i = 0; i < scores.Length; i++)
                {
                    scores[i] = -1f;
                }
                coordinador.setScores(scores);
                FindObjectOfType<IndicadorDireccion>().setMinijuego(0);
            }
        }
    }
}
