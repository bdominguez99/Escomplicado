using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Threading;

namespace TripasDeGato
{
    public class ControladorJuego : MonoBehaviour
    {
        [SerializeField] private GameObject m_celdaPrefab;
        [SerializeField] private GameObject m_elementoListaPrefab;
        [SerializeField] private GameObject m_popUp;
        [SerializeField] private GameObject m_listaConceptos;
        [SerializeField] private GameObject m_listaDefiniciones;
        [SerializeField] private GameObject m_referenciaCanvasGameObject;
        [SerializeField] private GameObject m_textoPrefab;
        [SerializeField] private GameObject m_pantallaSiguienteNivel;
        [SerializeField] private GameObject m_referenciaPopUp;
        [SerializeField] private GameObject m_pantallaCarga;
        [SerializeField] private GameObject m_pantallaFinDeJuego;
        [SerializeField] private GameObject m_padreCeldas;
        [SerializeField] private Text m_textoPuntuacionFinal;
        [SerializeField] private int m_ancho;
        [SerializeField] private int m_alto;
        [SerializeField] private Transform m_camara;
        [SerializeField] private float m_tamanioCelda;
        [SerializeField] private Vector2 m_inicioAreaJuego;
        [SerializeField] private Vector2 m_finAreaJuego;
        [SerializeField] private Reloj m_reloj;
        [SerializeField] private int m_maximoPreguntasPorFase;
        [SerializeField] private int m_maximoPreguntas;


        public static Dictionary<Vector2, Celda> Celdas { get; private set; }

        public static HashSet<Vector2> CeldasOcupadas { get; set; }

        public static Dictionary<int, Nodo> Nodos { get; private set; }

        public bool PopUpActivo { get; set; }

        public static Celda UltimaCeldaTocada;

        private ManualResetEvent m_lock;
        private ControladorPopUp m_controladorPopUp;
        private ControladorLineas m_controladorLineas;
        private Reloj m_manejadorReloj;
        private DataManager m_dataManager;
        private List<GameObject> m_textosNodos;
        private List<List<RelationQuestion>> m_preguntasFases;
        private int m_preguntasResueltas;
        private int m_totalPreguntas;
        private int m_faseActual;

        private async void Start()
        {
            m_dataManager = FindObjectOfType<CargadorDeDB>().DataManager;
            m_controladorPopUp = new ControladorPopUp(m_popUp, m_padreCeldas);
            m_manejadorReloj = FindObjectOfType<Reloj>();
            m_faseActual = 0;
            await ObtenerPreguntas();
            InstanciarCeldas();
            ConfigurarJuego();
        }

        private async Task ObtenerPreguntas()
        {
            m_pantallaCarga.SetActive(true);
            m_manejadorReloj.DetenerReloj();
            List<RelationQuestion> preguntas = await m_dataManager.GetRelationQuestionsAsync("Arquitectura de Computadoras");

            m_preguntasFases = new List<List<RelationQuestion>>();
            m_totalPreguntas = Mathf.Min(preguntas.Count, m_maximoPreguntas);
            var totalPreguntasAux = m_totalPreguntas;
            int totalFases = preguntas.Count / m_maximoPreguntasPorFase + (preguntas.Count % m_maximoPreguntasPorFase > 0 ? 1 : 0);

            for (int i = 0; i < totalFases && totalPreguntasAux > 0; i++)
            {
                var listaFase = new List<RelationQuestion>();
                for (int j = 0; j < m_maximoPreguntasPorFase && totalPreguntasAux > 0; j++)
                {
                    totalPreguntasAux--;
                    listaFase.Add(preguntas[i * m_maximoPreguntasPorFase + j]);
                }
                m_preguntasFases.Add(listaFase);
            }
            m_pantallaCarga.SetActive(false);
            m_manejadorReloj.IniciarReloj();
        }

        public void ActualizarEstado()
        {
            m_preguntasResueltas++;
            FindObjectOfType<ManejadorMarcador>().SetPreguntasResueltas(m_preguntasResueltas);
            if (m_preguntasResueltas == m_totalPreguntas)
            {
                FinalizarJuego();
            }
            else if (m_preguntasResueltas % m_maximoPreguntasPorFase == 0)
            {
                ReiniciarJuego();
            }
        }

        public void FinalizarJuego()
        {
            m_textoPuntuacionFinal.text = "Puntuacion: " + m_preguntasResueltas + "/" + m_totalPreguntas;
            m_pantallaFinDeJuego.SetActive(true);

            m_reloj.DetenerReloj();
            m_padreCeldas.SetActive(false);

            FindObjectOfType<ManejadorMarcador>().Desactivar();
        }

        private void ReiniciarJuego()
        {
            StartCoroutine(TransicionNivel());
        }

        private IEnumerator TransicionNivel()
        {
            m_reloj.DetenerReloj();
            m_padreCeldas.SetActive(false);
            m_pantallaSiguienteNivel.SetActive(true);
            m_pantallaSiguienteNivel.GetComponentInChildren<Text>().text = "Siguiente Nivel...";
            m_faseActual++;
            // Reiniciar las celdas
            foreach (Celda celda in Celdas.Values)
            {
                celda.Init(false, celda.Posicion, m_tamanioCelda, m_controladorLineas, m_referenciaPopUp);

                if (celda.Posicion.x == m_inicioAreaJuego.x || celda.Posicion.x == m_finAreaJuego.x - m_tamanioCelda ||
                    celda.Posicion.y == m_inicioAreaJuego.y || celda.Posicion.y == m_finAreaJuego.y - m_tamanioCelda)
                {
                    celda.InitNodo(true, Color.clear, "", -1, -2);
                    CeldasOcupadas.Add(celda.Posicion);
                }
            }

            // Remover los textos
            foreach (var texto in m_textosNodos)
            {
                Destroy(texto);
            }

            // Reiniciar celdas marcadas
            m_controladorLineas.ReiniciarJuego();
            CeldasOcupadas.Clear();

            m_controladorPopUp.RemoverElementos(m_listaConceptos);
            m_controladorPopUp.RemoverElementos(m_listaDefiniciones);

            ConfigurarJuego();

            yield return new WaitForSeconds(3);

            m_pantallaSiguienteNivel.SetActive(false);
            m_padreCeldas.SetActive(true);

            // Reanudar reloj
            m_reloj.IniciarReloj();
        }

        private void InstanciarCeldas()
        {
            Celdas = new Dictionary<Vector2, Celda>();
            Nodos = new Dictionary<int, Nodo>();
            CeldasOcupadas = new HashSet<Vector2>();
            m_controladorLineas = new ControladorLineas(m_controladorPopUp);
            m_textosNodos = new List<GameObject>();

            for (float x = m_inicioAreaJuego.x; x < m_finAreaJuego.x; x += m_tamanioCelda)
            {
                for (float y = m_inicioAreaJuego.y; y < m_finAreaJuego.y; y += m_tamanioCelda)
                {
                    var posicionCelda = new Vector3(x, y);
                    var celdaCreadaGameObject = Instantiate(m_celdaPrefab, posicionCelda, Quaternion.identity);

                    var celdaCreada = celdaCreadaGameObject.GetComponent<Celda>();
                    celdaCreada.transform.SetParent(m_padreCeldas.transform, false);

                    celdaCreada.Init(false, posicionCelda, m_tamanioCelda, m_controladorLineas, m_referenciaPopUp);
                    Celdas[new Vector2(x, y)] = celdaCreada;

                    if (x == m_inicioAreaJuego.x || x == m_finAreaJuego.x - m_tamanioCelda ||
                        y == m_inicioAreaJuego.y || y == m_finAreaJuego.y - m_tamanioCelda)
                    {
                        celdaCreada.InitNodo(true, Color.clear, "", -1, -2);
                        CeldasOcupadas.Add(celdaCreada.Posicion);
                    }
                }
            }
        }

        private void ConfigurarJuego()
        {
            // Inicializa las celdas nodo
            var preguntasFaseActual = m_preguntasFases[m_faseActual];

            List<int> idNodos = new List<int>();
            List<string> textoNodosConceptos = new List<string>();
            List<string> textoNodosDefiniciones = new List<string>();

            FindObjectOfType<ManejadorMarcador>().SetTotalPreguntas(m_totalPreguntas);

            for (int i = 1; i <= preguntasFaseActual.Count * 2; i++)
            {
                idNodos.Add(i);
            }
            for (int i = 0; i < preguntasFaseActual.Count; i++)
            {
                textoNodosConceptos.Add(preguntasFaseActual[i].Concept);
            }
            for (int i = 0; i < preguntasFaseActual.Count; i++)
            {
                textoNodosDefiniciones.Add(preguntasFaseActual[i].Definition);
            }

            ExtensionMethods.Shuffle<int>(idNodos);
            ExtensionMethods.Shuffle<string>(textoNodosConceptos);
            ExtensionMethods.Shuffle<string>(textoNodosDefiniciones);
            var idTexto = new Dictionary<string, int>();

            for (int i = 0; i < textoNodosConceptos.Count; i++)
            {
                idTexto[textoNodosConceptos[i]] = idNodos[i];
                var elementoLista = Instantiate(m_elementoListaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                elementoLista.transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().text = idTexto[textoNodosConceptos[i]].ToString();
                elementoLista.transform.GetChild(1).GetChild(0).GetComponentInChildren<Text>().text = textoNodosConceptos[i];
                m_controladorPopUp.AniadirElemento(elementoLista, m_listaConceptos);
            }

            for (int i = 0; i < textoNodosDefiniciones.Count; i++)
            {
                idTexto[textoNodosDefiniciones[i]] = idNodos[i + textoNodosDefiniciones.Count];
                var elementoLista = Instantiate(m_elementoListaPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                elementoLista.transform.GetChild(0).GetChild(0).GetComponentInChildren<Text>().text = idTexto[textoNodosDefiniciones[i]].ToString();
                elementoLista.transform.GetChild(1).GetChild(0).GetComponentInChildren<Text>().text = textoNodosDefiniciones[i];
                m_controladorPopUp.AniadirElemento(elementoLista, m_listaDefiniciones);
            }

            // Se elijen celdas al azar y se inicializan como concepto-definicion
            for (int i = 0; i < preguntasFaseActual.Count; i++)
            {
                Celda celdaConcepto = null;
                Celda celdaDefinicion = null;
                while (celdaDefinicion == null || celdaConcepto == null || !ValidarPosicionCeldas(celdaConcepto, celdaDefinicion))
                {
                    celdaConcepto = Celdas.ElementAt(Random.Range(0, Celdas.Count)).Value;
                    celdaDefinicion = Celdas.ElementAt(Random.Range(0, Celdas.Count)).Value;
                }

                var color = Color.clear;

                Vector2 offset = new Vector2(m_tamanioCelda, m_tamanioCelda);
                var contenedorTexto = GameObject.FindWithTag("ContenedorTexto");

                var celdasAdyacentesConcepto = ObtenerCeldasAdyacentes(celdaConcepto);
                foreach (Celda celda in celdasAdyacentesConcepto)
                {
                    celda.InitNodo(true, color, preguntasFaseActual[i].Concept, idTexto[preguntasFaseActual[i].Concept], idTexto[preguntasFaseActual[i].Definition]);
                    CeldasOcupadas.Add(celda.Posicion);
                }
                Nodos[idTexto[preguntasFaseActual[i].Concept]] = new Nodo(celdasAdyacentesConcepto, celdaConcepto);


                var celdasAdyacentesDefinicion = ObtenerCeldasAdyacentes(celdaDefinicion);
                foreach (Celda celda in celdasAdyacentesDefinicion)
                {
                    celda.InitNodo(true, color, preguntasFaseActual[i].Definition, idTexto[preguntasFaseActual[i].Definition], idTexto[preguntasFaseActual[i].Concept]);
                    CeldasOcupadas.Add(celda.Posicion);
                }
                Nodos[idTexto[preguntasFaseActual[i].Definition]] = new Nodo(celdasAdyacentesDefinicion, celdaDefinicion);

                var textoConcepto = Instantiate(m_textoPrefab, celdaConcepto.Posicion + offset / 2, Quaternion.identity, contenedorTexto.transform);
                textoConcepto.GetComponent<Text>().text = idTexto[preguntasFaseActual[i].Concept].ToString();

                var textoDefinicion = Instantiate(m_textoPrefab, celdaDefinicion.Posicion + offset / 2, Quaternion.identity, contenedorTexto.transform);
                textoDefinicion.GetComponent<Text>().text = idTexto[preguntasFaseActual[i].Definition].ToString();

                m_textosNodos.Add(textoConcepto);
                m_textosNodos.Add(textoDefinicion);
            }

            // Centrar la camara
            m_camara.transform.position = new Vector3((float)m_ancho / 2 - (m_tamanioCelda / 2), (float)m_alto / 2 - (m_tamanioCelda / 2), -10);
        }

        private bool ValidarPosicionCeldas(Celda celda1, Celda celda2)
        {
            List<Vector2> posicionesCelda1 = new List<Vector2>();
            List<Vector2> posicionesCelda2 = new List<Vector2>();

            var celdasAdyacentesCelda1 = ObtenerCeldasAdyacentes(celda1);
            var celdasAdyacentesCelda2 = ObtenerCeldasAdyacentes(celda2);

            if (celdasAdyacentesCelda1 == null || celdasAdyacentesCelda2 == null)
            {
                return false;
            }

            foreach (Celda celda in celdasAdyacentesCelda1)
            {
                if (CeldasOcupadas.Contains(celda.Posicion))
                {
                    return false;
                }
                posicionesCelda1.Add(celda.Posicion);
            }

            foreach (Celda celda in celdasAdyacentesCelda2)
            {
                if (CeldasOcupadas.Contains(celda.Posicion))
                {
                    return false;
                }
                posicionesCelda2.Add(celda.Posicion);
            }

            return !posicionesCelda1.Intersect(posicionesCelda2).Any();
        }

        public List<Celda> ObtenerCeldasAdyacentes(Celda celda)
        {
            List<Celda> celdasAdyacentes = new List<Celda>() { celda };

            float x = m_tamanioCelda;
            List<Vector2> movimientos = new List<Vector2>()
        {

            new Vector2(-x, -x), new Vector2(0, -x), new Vector2(x, -x),
            new Vector2(-x, 0),                       new Vector2(x, 0),
            new Vector2(-x, x), new Vector2(0, x), new Vector2(x, x)
        };

            foreach (var movimiento in movimientos)
            {
                var nuevaPosicion1 = celda.Posicion + movimiento;

                var celdaValida1 = nuevaPosicion1.x >= m_inicioAreaJuego.x && nuevaPosicion1.x < m_finAreaJuego.x &&
                    nuevaPosicion1.y >= m_inicioAreaJuego.y && nuevaPosicion1.y < m_finAreaJuego.y &&
                    !CeldasOcupadas.Contains(nuevaPosicion1);

                if (!celdaValida1)
                {
                    return null;
                }

                celdasAdyacentes.Add(Celdas[nuevaPosicion1]);
            }

            return celdasAdyacentes;
        }
        public void ReiniciarMinijuego()
        {
            m_pantallaFinDeJuego.SetActive(false);
            m_pantallaCarga.SetActive(true);
            SceneManager.LoadSceneAsync("TripasDeGato");
        }

        public void TerminarMiniuego()
        {
            FindObjectOfType<Minijuego>().setScore(((float)m_preguntasResueltas / m_totalPreguntas) * 10f);
            m_pantallaFinDeJuego.SetActive(false);
            m_pantallaCarga.SetActive(true);
            SceneManager.LoadSceneAsync("Main");
        }
    }
}