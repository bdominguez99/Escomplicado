using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManejadorMinijuegos : MonoBehaviour
{
    private Interfaz interfaz;
    private int nextMinigame;

    private void Start()
    {
        interfaz = FindObjectOfType<Interfaz>();
    }

    public void setNextMinigame(int minigame)
    {
        nextMinigame = minigame;
    }

    public void runMinigame()
    {
        StartCoroutine(runMinigame(nextMinigame));
    }

    private IEnumerator runMinigame(int id)
    {
        interfaz.enableLoadingScreen();
        FindObjectOfType<ManejadorGuardado>().guardarPartida(FindObjectOfType<InfoEntreEscenas>().idPartida);
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadSceneAsync(id + 3);
    }
}