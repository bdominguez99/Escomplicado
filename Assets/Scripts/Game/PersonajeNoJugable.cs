using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeNoJugable : MonoBehaviour
{
    [SerializeField] private int numeroPersonaje;
    [SerializeField] private Dialogo dialogoSinMinijuego, dialogoBase, dialogoReintento, dialogoGanado;

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
        float[] scores = coordinadorDeJuego.getScores();
        if (scores != null && isMinigamActive(scores))
        {
            if (scores != null && scores[numeroPersonaje] >= 0f)
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
        }
        else
        {
            manejadorDeDialogos.IniciarDialogo(dialogoSinMinijuego);
        }
        player.setCanMove(false);
        manejadorMinijuegos.setNextMinigame(numeroPersonaje);
    }

    private bool isMinigamActive(float[] scores)
    {
        if (numeroPersonaje == 0 || numeroPersonaje == 5)
            return true;
        return (scores[numeroPersonaje - 1] > 0.6f && (scores[numeroPersonaje] < 0.6f || scores[numeroPersonaje + 1] < 0.6f));
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
