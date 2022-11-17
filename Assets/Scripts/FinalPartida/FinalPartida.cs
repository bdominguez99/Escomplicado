using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalPartida : MonoBehaviour
{
    [SerializeField] private Text score, time;
    [SerializeField] private GameObject topScreen, loadingScreen;
    [SerializeField] private float fadeTime;

    private GuardadoGenerico<ArchivoGuardado> guardado;
    private ArchivoGuardado archivo;
    private InfoEntreEscenas infoEntreEscenas;
    private Image top;

    void Start()
    {
        guardado = new GuardadoGenerico<ArchivoGuardado>();
        archivo = guardado.Load(ManejadorGuardado.fileName);
        infoEntreEscenas = FindObjectOfType<InfoEntreEscenas>();
        if (infoEntreEscenas == null)
        {
            GameObject aux = Instantiate(new GameObject("InfoEntreEscenas"), Vector3.zero, Quaternion.identity);
            aux.AddComponent<InfoEntreEscenas>();
            infoEntreEscenas = aux.GetComponent<InfoEntreEscenas>();
            infoEntreEscenas.idPartida = 0;
            infoEntreEscenas.nombreJugador = "Test";
        }
        if (archivo != default)
        {
            Partida partida = archivo.partidas[infoEntreEscenas.idPartida];
            score.text = "" + partida.puntuacionFinal;
            time.text = "" + (int)(partida.tiempoJuego / 60) + " minutos";
        }
        top = topScreen.GetComponent<Image>();
        StartCoroutine(fade());
    }
    public void continuar()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private IEnumerator fade()
    {
        float progress = 0f;
        while(progress < fadeTime)
        {
            progress += Time.deltaTime;
            top.color = new Color(1, 1, 1, 1 - (progress / fadeTime));
            yield return null;
        }
        top.color = new Color(1, 1, 1, 0);
        topScreen.SetActive(false);
    }
}
