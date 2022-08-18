using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionPartida : MonoBehaviour
{
    [SerializeField] private GameObject[] eraseButtons;
    [SerializeField] private Text[] slots;
    private ArchivoGuardado archivoGuardado;

    private void OnEnable()
    {
        instantiateGamesTexts();
    }

    public bool isNewGame(int idPartida)
    {
        archivoGuardado = new GuardadoGenerico<ArchivoGuardado>().Load(ManejadorGuardado.fileName);
        return archivoGuardado == null || archivoGuardado.partidas == null || archivoGuardado.partidas[idPartida] == null;
    }

    public void instantiateGamesTexts()
    {
        archivoGuardado = new GuardadoGenerico<ArchivoGuardado>().Load(ManejadorGuardado.fileName);
        if (archivoGuardado != default && archivoGuardado.partidas != null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (archivoGuardado.partidas[i] != null && archivoGuardado.partidas[i].nombre != null && !archivoGuardado.partidas[i].nombre.Equals(""))
                {
                    slots[i].text = (i + 1) + ": " + archivoGuardado.partidas[i].nombre;
                    eraseButtons[i].SetActive(true);
                }
                else
                {
                    slots[i].text = (i + 1) + ": Iniciar nueva partida.";
                    eraseButtons[i].SetActive(false);
                }
            }
        }
    }
}
