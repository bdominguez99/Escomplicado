using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorModoLibre : MonoBehaviour
{
    [SerializeField] private GameObject m_elementoMateriaPrefab;
    [SerializeField] private GameObject m_minigamePrefab;
    [SerializeField] private GameObject m_referenciaListaMaterias;
    [SerializeField] private GameObject m_referenciaListaMinijuegos;
    [SerializeField] private GameObject m_textoDescription;
    
    [SerializeField] private List<string> m_nombreMinijuegos;
    [SerializeField] private List<string> m_descripcionMinijuegos;
    [SerializeField] private List<TipoMinijuego> m_tipoMinijuego;
    [SerializeField] private List<Sprite> m_miniaturas;

    private DataManager m_dataManager;
    private List<GameObject> m_minijuegos;
    private Dictionary<string, HashSet<TipoMinijuego>> m_materias;
    private int m_minijuegoSeleccionado;
    private string m_materiaSeleccionada;


    // Start is called before the first frame update
    async void Start()
    {
        m_minijuegos = new List<GameObject>();
        m_materias = new Dictionary<string, HashSet<TipoMinijuego>>();
        m_minijuegoSeleccionado = -1;

        await ObtenerMaterias();
        MostrarMaterias();
        CrearMiniaturas();
    }

    private async Task ObtenerMaterias()
    {
        m_dataManager = new DataManager();
        List<JsonQuestion> preguntas = await m_dataManager.GetAllQuestionsAsync();

        foreach (var pregunta in preguntas)
        {
            if (!m_materias.ContainsKey(pregunta.subject))
            {
                m_materias.Add(pregunta.subject, new HashSet<TipoMinijuego>());
            }
            m_materias[pregunta.subject].Add(ToTipoMinijuego(pregunta.type));
        }
    }

    public TipoMinijuego ToTipoMinijuego(string tipo)
    {
        switch (tipo)
        {
            case "multiple_option":
                return TipoMinijuego.OpcionMultiple;
            case "ordered_answer":
                return TipoMinijuego.Ordenamiento;
            case "relation_question":
                return TipoMinijuego.Relacion;
            case "multiple_answer":
                return TipoMinijuego.MultipleIncisos;
            default: return TipoMinijuego.OpcionMultiple;
        }
    }

    private void MostrarMaterias()
    {
        foreach (var materia in m_materias)
        {
            var nuevoElemento = Instantiate(m_elementoMateriaPrefab, m_referenciaListaMaterias.transform);
            nuevoElemento.GetComponent<ElementoListaMateria>().Init(materia.Key, materia.Value);
        }
    }

    public void CrearMiniaturas()
    {
        for (int i = 0; i < m_nombreMinijuegos.Count; i++)
        {
            var miniatura = Instantiate(m_minigamePrefab, m_referenciaListaMinijuegos.transform);
            miniatura.GetComponent<MiniaturaMinijuego>()
                .Init(m_miniaturas[i], m_nombreMinijuegos[i], m_tipoMinijuego[i], i);
            m_minijuegos.Add(miniatura);
            miniatura.SetActive(false);
        }
    }

    public void MostrarMinijuegos(string nombreMateria, HashSet<TipoMinijuego> tipo)
    {
        m_materiaSeleccionada = nombreMateria;

        foreach (var minijuego in m_minijuegos)
        {
            if (tipo.Contains(minijuego.GetComponent<MiniaturaMinijuego>().TipoMinijuego))
            {
                minijuego.SetActive(true);
            }
            else
            {
                minijuego.SetActive(false);
            }
        }
    }

    public void SeleccionarMinijuego(int numeroMinijuego)
    {
        m_textoDescription.GetComponent<Text>().text = m_descripcionMinijuegos[numeroMinijuego];
        m_minijuegoSeleccionado = numeroMinijuego;
    }

    public void IniciarMinijuego()
    {
        if (m_minijuegoSeleccionado == -1)
        {
            return;
        }

        FindObjectOfType<InfoEntreEscenas>().EsModoLibre = true;
        FindObjectOfType<InfoEntreEscenas>().MateriaModoLibre = m_materiaSeleccionada;

        switch (m_minijuegoSeleccionado)
        {
            case 0:

                SceneManager.LoadSceneAsync("SpaceInvaders");
                break;
            case 1:
                SceneManager.LoadSceneAsync("SnakeScene");
                break;
            case 2:
                SceneManager.LoadSceneAsync("TripasDeGato");
                break;
            case 3:
                SceneManager.LoadSceneAsync("FroggerScene");
                break;
            case 4:
                SceneManager.LoadSceneAsync("AngryKids");
                break;
            case 5:
                SceneManager.LoadSceneAsync("Pipes");
                break;
        }
    }
}
