using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargadorDeDB : MonoBehaviour
{
    public static CargadorDeDB m_cargadorDB;

    public DataManager DataManager { get; private set; }

    private async void Awake()
    {
        if (m_cargadorDB == null)
        {
            var manejadorBaseDatos = new DataManager();
            await manejadorBaseDatos.GetAllQuestionsAsync();
            DataManager = manejadorBaseDatos;
        }
    }

    private void Start()
    {
        if (m_cargadorDB == null)
        {
            m_cargadorDB = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
