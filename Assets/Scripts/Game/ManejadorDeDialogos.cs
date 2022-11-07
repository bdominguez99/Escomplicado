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
    private Interfaz interfaz;

    void Start()
    {
        lineas = new Queue<string>();
        interfaz = FindObjectOfType<Interfaz>();
    }

    public void IniciarDialogo (Dialogo dialogo, bool esDialogoMinijuego = false)
    {
        this.esDialogoMinijuego = esDialogoMinijuego;
        dialogoGameObject.SetActive(true);
        interfaz.mostrarInterfaz(Interfaz.Interfaces.Default);
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
            interfaz.mostrarInterfaz(Interfaz.Interfaces.Confirmacion);
            esDialogoMinijuego = false;
        }
        else
        {
            interfaz.mostrarInterfaz(Interfaz.Interfaces.Mapa);
            interfaz.mostrarInterfaz(Interfaz.Interfaces.Intractuar);
            FindObjectOfType<PlayerController>().setCanMove(true);
        }
    }
}
