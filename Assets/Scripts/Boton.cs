using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : MonoBehaviour
{
    private Interfaz interfaz;
    [SerializeField] private GameObject player;

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

    public void verPuntuaciones()
    {
        interfaz.mostrarInterfaz(Interfaz.Interfaces.VerPuntuaciones);
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
            StartCoroutine(FindObjectOfType<Interfaz>().loadScene("Main"));
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

    public void guardarPartida()
    {
        FindObjectOfType<ManejadorGuardado>().guardarPartida(FindObjectOfType<InfoEntreEscenas>().idPartida);
    }

    public void cargarMenu()
    {
        StartCoroutine(FindObjectOfType<Interfaz>().loadScene("MainMenu"));
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
