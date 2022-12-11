using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : MonoBehaviour
{
    public static int partidaAEliminar;
    [SerializeField] private GameObject player;
    private Interfaz interfaz;

    private void Start()
    {
        interfaz = FindObjectOfType<Interfaz>();
    }

    public void menuInicio()
    {
        interfaz.mostrarInterfaz(Interfaz.Interfaces.Inicio);
    }

    public void seleccionarPartida()
    {
        interfaz.mostrarInterfaz(Interfaz.Interfaces.SleccionPartida);
    }

    public void prepararEliminarPartida(int idPartida)
    {
        partidaAEliminar = idPartida;
        interfaz.mostrarInterfaz(Interfaz.Interfaces.ConfirmacionEliminarPartida);
    }

    public void verPuntuaciones()
    {
        interfaz.mostrarInterfaz(Interfaz.Interfaces.VerPuntuaciones);
    }

    public void modoLibre()
    {
        interfaz.mostrarInterfaz(Interfaz.Interfaces.ModoLibre);
    }

    public void salirDelJuego()
    {
        Application.Quit();
    }

    public void verificarPartidaNueva(int idPartida)
    {
        SeleccionPartida seleccionPartida = FindObjectOfType<SeleccionPartida>();
        if (seleccionPartida.isNewGame(idPartida))
        {
            FindObjectOfType<InfoEntreEscenas>().idPartida = idPartida;
            interfaz.mostrarInterfaz(Interfaz.Interfaces.NuevaPartida);
        }
        else
        {
            FindObjectOfType<Interfaz>().loadScene("Main");
        }
    }

    public void iniciarNuevaPartida()
    {
        FindObjectOfType<NuevaPartida>().tryStartNewGame();
    }

    public void puasarJuego()
    {
        interfaz.mostrarInterfaz(Interfaz.Interfaces.Pausa);
        FindObjectOfType<CoordinadorDeJuego>().setPlayingState(false);
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.setCanMove(false);
            playerController.enabled = false;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void reanudarPartida()
    {
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.enabled = true;
            playerController.setCanMove(true);
        }
        interfaz.mostrarInterfaz(Interfaz.Interfaces.Mapa);
        FindObjectOfType<CoordinadorDeJuego>().setPlayingState(true);
    }

    public void eliminarPartida()
    {
        var manager = new GuardadoGenerico<ArchivoGuardado>();
        var archivo = manager.Load(ManejadorGuardado.fileName);
        archivo.partidas[partidaAEliminar] = null;
        manager.Save(archivo, ManejadorGuardado.fileName);
        FindObjectOfType<SeleccionPartida>().instantiateGamesTexts();
        interfaz.mostrarInterfaz(Interfaz.Interfaces.SleccionPartida);
    }

    public void guardarPartida()
    {
        FindObjectOfType<ManejadorGuardado>().guardarPartida(FindObjectOfType<InfoEntreEscenas>().idPartida);
    }

    public void cargarMenu()
    {
        FindObjectOfType<Interfaz>().loadScene("MainMenu");
    }

    public void interactuar()
    {
        GameObject interactivo = FindObjectOfType<PlayerController>().ultimaColision;
        if (interactivo != null)
        {
            interactivo.SendMessage("accion");
        }
    }
}
