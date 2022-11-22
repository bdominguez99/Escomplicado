using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interfaz : MonoBehaviour
{
    public enum Interfaces { Inicio, SleccionPartida, NuevaPartida, VerPuntuaciones, Pausa, Mapa, Intractuar, Confirmacion, ConfirmacionEliminarPartida, Default };

    [Header("Inicio")]
    [SerializeField] private GameObject confirmacionSalida;
    [SerializeField] private GameObject listaPuntuaciones;
    [SerializeField] private GameObject elementoPuntuacionPrefab;

    [Header("Pantallas")]
    [SerializeField] private GameObject[] layers;
    [SerializeField] private GameObject pantallaNegraGO, loadingScreen;

    private Image pantallaNegra;
    private PlayerController player;
    private GameObject nextDoor;
    private IndicadorDireccion indicador;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        indicador = FindObjectOfType<IndicadorDireccion>();
        if (pantallaNegraGO != null) pantallaNegra = pantallaNegraGO.GetComponent<Image>();
        else cargarPuntuaciones();
    }

    private void cargarPuntuaciones()
    {
        var file = ManejadorGuardado.fileName;
        GuardadoGenerico<ArchivoGuardado> guardado = new GuardadoGenerico<ArchivoGuardado>();
        var archivoGuardado = guardado.Load(file);
        var puntuaciones = new List<List<string>>();
        if (archivoGuardado != default)
        {
            if (archivoGuardado.historialPuntuaciones == null)
            {
                Debug.Log("Null list");
            }
            else
            {
                foreach (var puntuacion in archivoGuardado.historialPuntuaciones)
                {
                    var aux = new List<string>(puntuacion.Split(' '));
                    puntuaciones.Add(aux);
                }
            }
        }

        foreach (var entrada in puntuaciones)
        {
            var nuevaEntrada = Instantiate(elementoPuntuacionPrefab, listaPuntuaciones.transform);
            nuevaEntrada.GetComponent<ElementoPuntuacion>().Init(entrada[0],entrada[1], entrada[2]);
        }
    }

    public void mostrarInterfaz(Interfaces interfaz)
    {
        switch (interfaz)
        {
            case Interfaces.Inicio:
                mostrarLayers(new bool[] { true, false, false, false, false });
                break;
            case Interfaces.SleccionPartida:
                mostrarLayers(new bool[] { false, true, false, false, false });
                break;
            case Interfaces.NuevaPartida:
                mostrarLayers(new bool[] { false, false, false, true, false });
                break;
            case Interfaces.VerPuntuaciones:
                mostrarLayers(new bool[] { false, false, true, false, false });
                break;
            case Interfaces.ConfirmacionEliminarPartida:
                mostrarLayers(new bool[] { false, true, false, false, true });
                break;
            case Interfaces.Pausa:
                mostrarLayers(new bool[] { true, false, false, false });
                break;
            case Interfaces.Mapa:
                mostrarLayers(new bool[] { false, true, false, false });
                break;
            case Interfaces.Intractuar:
                mostrarLayers(new bool[] { layers[0].activeSelf, layers[1].activeSelf, true, false });
                break;
            case Interfaces.Confirmacion:
                mostrarLayers(new bool[] { false, false, false, true });
                break;
            case Interfaces.Default:
                mostrarLayers(new bool[] { false, false, false, false });
                break;
        }
    }

    public void apagarConfirmacion()
    {
        mostrarLayers(new bool[] { false, true, true, false });
        player.setCanMove(true);
    }

    private void mostrarLayers(bool[] activeLayers)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].SetActive(activeLayers[i]);
        }
    }

    public void setNextDoor(GameObject door)
    {
        nextDoor = door;
    }

    public void enableLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }

    public IEnumerator transition(bool cambiarPiso, bool showArrow = false)
    {
        if (pantallaNegraGO != null)
        {
            float progress = 0;
            player.setCanMove(false);
            pantallaNegraGO.SetActive(true);
            while (progress < 1)
            {
                progress += Time.deltaTime;
                pantallaNegra.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, progress);
                yield return null;
            }
            pantallaNegra.color = Color.black;
            yield return new WaitForSecondsRealtime(0.1f);

            if (cambiarPiso)
            {
                FindObjectOfType<Mapa>().cambiarPiso();
                indicador.verifyFloorlayersVisivility();
            }
            else player.transform.position = nextDoor.transform.position;

            yield return new WaitForSecondsRealtime(0.1f);
            progress = 1;
            while (progress > 0)
            {
                progress -= Time.deltaTime;
                pantallaNegra.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, progress);
                yield return null;
            }
            pantallaNegra.color = new Color(0, 0, 0, 0);
            pantallaNegraGO.SetActive(false);
            player.setCanMove(true);
            if(showArrow && indicador != null) indicador.setArrowVisibility(true);
        }
    }

    public void loadScene(string scene)
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadSceneAsync(scene);
    }

    public void mostrarConfirmacionSalida()
    {
        confirmacionSalida.SetActive(true);
    }

    public void esconderConfirmacionSalida()
    {
        confirmacionSalida.SetActive(false);
    }
}
