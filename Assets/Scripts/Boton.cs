using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void inciarNueaPartida()
    {
        SceneManager.LoadScene("Main");
    }

    public void puasarJuego()
    {
        interfaz.mostrarInterfaz(Interfaz.Interfaces.Pausa);
        if (player != null) player.SetActive(false);
    }

    public void reanudarPartida()
    {
        if (player != null) player.SetActive(true);
        interfaz.mostrarInterfaz(Interfaz.Interfaces.Mapa);
    }

    public void guardarPartida()
    {
        Debug.Log("Aqui se debería guardar la partida pero todavía no está implementado jaja.");
    }

    public void cargarMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
