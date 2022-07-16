using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NuevaPartida : MonoBehaviour
{
    [SerializeField] private string badNameMsg;
    [SerializeField] private Text auxText;
    [SerializeField] private InputField nombreText;

    public void tryStartNewGame()
    {
        string nombre = nombreText.text;
        if(nombre == null || nombre == "")
        {
            auxText.text = badNameMsg;
        }
        else
        {
            InfoEntreEscenas info = FindObjectOfType<InfoEntreEscenas>();
            info.nombreJugador = nombre;
            StartCoroutine(FindObjectOfType<Interfaz>().loadScene("NewGame"));
        }
    }
}
