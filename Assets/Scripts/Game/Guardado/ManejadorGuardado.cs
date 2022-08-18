using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejadorGuardado : MonoBehaviour
{
    private GuardadoGenerico<ArchivoGuardado> guardado = new GuardadoGenerico<ArchivoGuardado>();
    private ArchivoGuardado archivoGuardado;
    public static readonly string fileName = "SaveFile.dat";

    public void Start()
    {
        var archivoGuardado = guardado.Load(fileName);
        if (archivoGuardado == null)
        {
            Debug.LogError("El archivo no pudo ser cargado");
            return;
        }

        int idPartida = FindObjectOfType<InfoEntreEscenas>().idPartida;
        if (archivoGuardado.partidas[idPartida] == null)
        {
            var entradas = ControladorSalones.asignarSalones();
            FindObjectOfType<ManejadorGuardado>().cargarEntradasSalones(entradas);
        }
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
        partida.entradasSalones = obtenerEntradasSalones();
        if (partida.entradasSalones == null || partida.entradasSalones.Length == 0)
        {
            Debug.Log("Es null al guardar");
        }
        Debug.Log("Salones guardados" + partida.entradasSalones.Length);

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
                cargarEntradasSalones(partida.entradasSalones);
            }
            else
            {
                coordinador.setTiempoJuego(0);
                coordinador.setNombreJugador(FindObjectOfType<InfoEntreEscenas>().nombreJugador);
            }
        }
    }

    public ArchivoGuardado cargarArchivoGuardado()
    {
        return (new GuardadoGenerico<ArchivoGuardado>()).Load(fileName);
    }

    private EntradaSalon[] obtenerEntradasSalones()
    {
        List<EntradaSalon> entradasSalones = new List<EntradaSalon>();
        CoordinadorDeJuego.acivarTodosLosPisos();
        for (int i = 0; i <= 2; i++)
        {
            GameObject pisoGameObject = GameObject.Find("Piso" + i).transform.gameObject;
            Puerta[] puertas = pisoGameObject.transform.GetComponentsInChildren<Puerta>();
            foreach (Puerta puerta in puertas)
            {
                float[] posicion = new float[] { puerta.transform.position.x, puerta.transform.position.y };
                entradasSalones.Add(new EntradaSalon(posicion, puerta.esIzquierda, i));
            }
        }
        CoordinadorDeJuego.desacivarPisosNoUsados();

        return entradasSalones.ToArray();
    }

    public void cargarEntradasSalones(EntradaSalon[] entradas)
    {
        // Activamos los pisos temporalmente para poder asignar las puertas
        CoordinadorDeJuego.acivarTodosLosPisos();

        foreach (EntradaSalon entrada in entradas)
        {
            GameObject puertas = new GameObject();
            puertas.name = "Puertas";
            puertas.transform.parent = GameObject.Find("Piso" + entrada.piso).transform;

            GameObject puertaPrefab = FindObjectOfType<Mapa>().puerta;

            var newPuerta = Instantiate(puertaPrefab, new Vector3(entrada.posicionX, entrada.posicionY), Quaternion.identity);
            newPuerta.GetComponent<Puerta>().esIzquierda = entrada.esIzquierda;
            newPuerta.GetComponent<Puerta>().esEntradaASalon = true;
            if (entrada.esIzquierda)
            {
                newPuerta.GetComponent<Puerta>().puertaDestino = GameObject.Find("PuertaSalonDeIzquierda");
            }
            else
            {
                newPuerta.GetComponent<Puerta>().puertaDestino = GameObject.Find("PuertaSalonDeDerecha");
            }

            newPuerta.transform.parent = puertas.transform;
        }

        CoordinadorDeJuego.desacivarPisosNoUsados();
    }
}
