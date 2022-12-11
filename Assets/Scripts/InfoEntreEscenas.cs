using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoEntreEscenas : MonoBehaviour
{
    public static InfoEntreEscenas info;
    public int idPartida;
    public string nombreJugador;

    public bool EsModoLibre { get; set; } = false;

    public string MateriaModoLibre { get; set; } = "";

    void Start()
    {
        if(info == null)
        {
            info = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
