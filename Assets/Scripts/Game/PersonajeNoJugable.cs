using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeNoJugable : MonoBehaviour
{
    [SerializeField] private int numeroPersonaje;
    [SerializeField] private Dialogo dialogoBase, dialogoReintento, dialogoGanado;

    private ManejadorDeDialogos manejadorDeDialogos;
    private ManejadorMinijuegos manejadorMinijuegos;
    private CoordinadorDeJuego coordinadorDeJuego;
    private PlayerController player;

    private void Start()
    {
        manejadorDeDialogos = FindObjectOfType<ManejadorDeDialogos>();
        manejadorMinijuegos = FindObjectOfType<ManejadorMinijuegos>();
        coordinadorDeJuego = FindObjectOfType<CoordinadorDeJuego>();
        player = FindObjectOfType<PlayerController>();
    }

    public void accion ()
    {
        if (coordinadorDeJuego.getScores()[numeroPersonaje] >= 0f)
        {
            if (coordinadorDeJuego.getScores()[numeroPersonaje] >= 6f)
            {
                manejadorDeDialogos.IniciarDialogo(dialogoGanado);
            }
            else
            {
                manejadorDeDialogos.IniciarDialogo(dialogoReintento, true);
            }
        }
        else
        {
            manejadorDeDialogos.IniciarDialogo(dialogoBase, true);
        }
        player.setCanMove(false);
        manejadorMinijuegos.setNextMinigame(numeroPersonaje);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<Interfaz>().mostrarInterfaz(Interfaz.Interfaces.Intractuar);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<Interfaz>().mostrarInterfaz(Interfaz.Interfaces.Mapa);
        }
    }
}
