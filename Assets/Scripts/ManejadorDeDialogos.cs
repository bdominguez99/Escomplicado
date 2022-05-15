using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorDeDialogos : MonoBehaviour
{
    public Text textoDeDialogo;
    public Animator animator;
    [SerializeField] public float velocidadDeEscritura = 5;
    private Queue<string> lineas;

    void Start()
    {
        lineas = new Queue<string>();
    }

    public void IniciarDialogo (Dialogo dialogo)
    {
        animator.SetBool("Activo", true);

        lineas.Clear();

        foreach (string linea in dialogo.lineas)
        {
            lineas.Enqueue(linea);
        }

        MostrarLineaSiguiente();
    }


    public void MostrarLineaSiguiente()
    {
        if (lineas.Count == 0)
        {
            FinalizarDialogo();
            return;
        }

        string siguienteLinea = lineas.Dequeue();
        StopAllCoroutines();
        StartCoroutine(EscribirLinea(siguienteLinea));
    }

    IEnumerator EscribirLinea(string linea)
    {
        textoDeDialogo.text = "";
        foreach (char caracter in linea.ToCharArray())
        {
            textoDeDialogo.text += caracter;
            yield return new WaitForSeconds(1 / velocidadDeEscritura);
        }
    }

    public void FinalizarDialogo()
    {
        animator.SetBool("Activo", false);
    }
}
