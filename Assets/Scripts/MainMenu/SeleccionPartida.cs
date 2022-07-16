using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionPartida : MonoBehaviour
{
    [SerializeField] private Text[] slots;
    private ArchivoGuardado archivoGuardado;

    private void OnEnable()
    {
        archivoGuardado = new GuardadoGenerico<ArchivoGuardado>().Load(ManejadorGuardado.fileName);
        if(archivoGuardado != default && archivoGuardado.partidas != null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (archivoGuardado.partidas[i] != null && archivoGuardado.partidas[i].nombre != null && !archivoGuardado.partidas[i].nombre.Equals(""))
                    slots[i].text = (i + 1) + ": " + archivoGuardado.partidas[i].nombre;
            }
        }
    }

    public bool isNewGame(int idPartida)
    {
        return archivoGuardado.partidas == null || archivoGuardado.partidas[idPartida] == null;
    }
}
