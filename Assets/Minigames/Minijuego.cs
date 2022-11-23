using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minijuego : MonoBehaviour
{
    public static Minijuego minijuego;
    [SerializeField] [Range(0, 5)] private int minigameId;
    [SerializeField] private float score;
    [SerializeField] private bool setScoreSteable;

    void Start()
    {
        if(minijuego == null)
        {
            minijuego = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Este método se usa para pasar la puntuación obtenida en un minijuego a la escena principal
    /// </summary>
    /// <param name="score">El score es la calificación que sacó el jugador, debe de ser un numero entre 0 y 10</param>
    public void setScore(float score)
    {
        if(setScoreSteable)
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
