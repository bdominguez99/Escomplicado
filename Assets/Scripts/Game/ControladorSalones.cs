using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class ControladorSalones
    {
        private const int salonesPorAsignar = 6;

        private static float[][] posicionesPuertasDerechas = new float[][] 
        {
            new float[]{ 170.3f , 25 }, 
            new float[]{ 170.3f , 20 }, 
            new float[]{ 170.3f , 15 }, 
            new float[]{ 170.3f , 10 }, 
            new float[]{ 170.3f , 1 },  
            new float[]{ 170.3f , -4 }, 
            new float[]{ 170.3f , -9 }, 
            new float[]{ 170.3f , -19 },
            new float[]{ 170.3f , -24 },
            new float[]{ 170.3f , -29 },
            new float[]{ 170.3f , -34 },
            new float[]{ 170.3f , -44 },
            new float[]{ 170.3f , -49 },
            new float[]{ 170.3f , -54 },
            new float[]{191.3f , -17},
            new float[]{191.3f , -22},
            new float[]{191.3f , -27},
            new float[]{191.3f , -32},
            new float[]{191.3f , -37},
            new float[]{191.3f , -47},
            new float[]{191.3f , -51},
            new float[]{191.3f , -56},
            new float[]{191.3f , -61},
            new float[]{191.3f , -66},
        };

        private static float[][] posicionesPuertasIzquierdas = new float[][]
        {
            new float[]{ 201.9f , 25 },
            new float[]{ 201.9f , 20 },
            new float[]{ 201.9f , 15 },
            new float[]{ 201.9f , 10 },
            new float[]{ 201.9f , 1 },
            new float[]{ 201.9f , -4 },
            new float[]{ 201.9f , -9 },
            new float[]{ 201.9f , -19 },
            new float[]{ 201.9f , -24 },
            new float[]{ 201.9f , -29 },
            new float[]{ 201.9f , -34 },
            new float[]{ 201.9f , -44 },
            new float[]{ 201.9f , -49 },
            new float[]{ 201.9f , -54 },
            new float[]{180.7f , -17},
            new float[]{180.7f , -22},
            new float[]{180.7f , -27},
            new float[]{180.7f , -32},
            new float[]{180.7f , -37},
            new float[]{180.7f , -47},
            new float[]{180.7f , -51},
            new float[]{180.7f , -56},
            new float[]{180.7f , -61},
            new float[]{180.7f , -66},
        };

        public static EntradaSalon[] asignarSalones ()
        {
            List<EntradaSalon> entradasAsignadas = new List<EntradaSalon>();

            for (int i = 0; i < salonesPorAsignar; i++)
            {
                // Entradas hacia la izquierda
                if (Random.value <= 0.5)
                {
                    int posicionEntrada = Random.Range(0, posicionesPuertasIzquierdas.Length);
                    entradasAsignadas.Add(new EntradaSalon(posicionesPuertasIzquierdas[posicionEntrada], true, Random.Range(0, 3)));
                } 
                else
                {
                    int posicionEntrada = Random.Range(0, posicionesPuertasDerechas.Length);
                    entradasAsignadas.Add(new EntradaSalon(posicionesPuertasDerechas[posicionEntrada], false, Random.Range(0, 3)));
                }
            }

            return entradasAsignadas.ToArray();
        }
    }

    [System.Serializable]
    public class EntradaSalon
    {
        public bool esIzquierda;
        public float posicionX;
        public float posicionY;
        public int piso;

        public EntradaSalon(float[] posicion, bool esIzquierda, int piso)
        {
            posicionX = posicion[0];
            posicionY = posicion[1];
            this.esIzquierda = esIzquierda;
            this.piso = piso;
        }
    }
}