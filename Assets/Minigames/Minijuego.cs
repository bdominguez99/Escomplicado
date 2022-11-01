using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minijuego : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] private int minigameId;
    private float score;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Este m�todo se usa para pasar la puntuaci�n obtenida en un minijuego a la escena principal
    /// </summary>
    /// <param name="score">El score es la calificaci�n que sac� el jugador, debe de ser un numero entre 0 y 10</param>
    public void setScore(float score)
    {
        this.score = score;
    }

    public float getScore()
    {
        return score;
    }

    public int getMinigameId()
    {
        return minigameId;
    }
}
