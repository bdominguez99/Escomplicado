using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorDeDialogos : MonoBehaviour
{
    public Text textoDeDialogo;
    [SerializeField] private GameObject dialogoGameObject;
    [SerializeField] private float velocidadDeEscritura = 5;
    private Queue<string> lineas;
    private string siguienteLinea;
    private bool escribiendo, esDialogoMinijuego;

    void Start()
    {
        lineas = new Queue<string>();
    }

    public void IniciarDialogo (Dialogo dialogo, bool esDialogoMinijuego = false)
    {
        this.esDialogoMinijuego = esDialogoMinijuego;
        dialogoGameObject.SetActive(true);
        lineas.Clear();

        foreach (string linea in dialogo.lineas)
        {
            lineas.Enqueue(linea);
        }

        MostrarLineaSiguiente();
    }

    public void MostrarLineaSiguiente()
    {
        if (escribiendo && siguienteLinea != null)
        {
            StopAllCoroutines();
            textoDeDialogo.text = siguienteLinea;
            escribiendo = false;
        }
        else
        {
            if (lineas.Count == 0)
            {
                FinalizarDialogo();
                return;
            }
            siguienteLinea = lineas.Dequeue();
            StopAllCoroutines();
            StartCoroutine(EscribirLinea(siguienteLinea));
        }
    }

    IEnumerator EscribirLinea(string linea)
    {
        escribiendo = true;
        textoDeDialogo.text = "";
        foreach (char caracter in linea.ToCharArray())
        {
            textoDeDialogo.text += caracter;
            yield return new WaitForSeconds(1 / velocidadDeEscritura);
        }
        escribiendo = false;
    }

    public void FinalizarDialogo()
    {
        dialogoGameObject.SetActive(false);
        if (esDialogoMinijuego)
        {
            FindObjectOfType<Interfaz>().mostrarInterfaz(Interfaz.Interfaces.Confirmacion);
            esDialogoMinijuego = false;
        }
        else
        {
            FindObjectOfType<PlayerController>().setCanMove(true);
        }
    }
}
