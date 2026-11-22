using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem.UI;
#endif

public class ControladorMenu : MonoBehaviour
{
    [Header("Opcional: dejar vacías, se buscan solas")]
    public GameObject panelMenu;
    public Button botonIniciar;
    public Button botonSalir;

    void Awake()
    {
        ArreglarEventSystem();
        BuscarReferencias();
    }

    void Start()
    {
        if (botonIniciar != null)
        {
            botonIniciar.onClick.RemoveAllListeners();
            botonIniciar.onClick.AddListener(IniciarJuego);
        }
        else
        {
            Debug.LogError("No se encontró el botón Iniciar.");
        }

        if (botonSalir != null)
        {
            botonSalir.onClick.RemoveAllListeners();
            botonSalir.onClick.AddListener(Salir);
        }
        else
        {
            Debug.LogError("No se encontró el botón Salir.");
        }

        if (panelMenu == null)
        {
            Debug.LogError("No se encontró el Panel del menú.");
        }

        MostrarMenu(true);
        Time.timeScale = 0f;
    }

    public void IniciarJuego()
    {
        Debug.Log("Iniciar juego");
        MostrarMenu(false);
        Time.timeScale = 1f;
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Oculta o muestra el panel (imagen de fondo) y los dos botones
    void MostrarMenu(bool visible)
    {
        if (panelMenu != null) panelMenu.SetActive(visible);
        if (botonIniciar != null) botonIniciar.gameObject.SetActive(visible);
        if (botonSalir != null) botonSalir.gameObject.SetActive(visible);
    }

    void BuscarReferencias()
    {
        // Si en la casilla del Panel hay un botón por error, se descarta
        if (panelMenu != null && panelMenu.GetComponent<Button>() != null)
        {
            panelMenu = null;
        }

        Button[] botones = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Button b in botones)
        {
            string nombre = b.gameObject.name.Trim().ToLower();
            string texto = "";

            TMP_Text tmp = b.GetComponentInChildren<TMP_Text>(true);
            if (tmp != null) texto = tmp.text.Trim().ToLower();

            bool esSalir = nombre.Contains("salir") || texto.Contains("salir");
            bool esIniciar = nombre.Contains("inici") || texto.Contains("inici") || texto.Contains("jugar");

            if (esSalir && botonSalir == null)
            {
                botonSalir = b;
            }
            else if (esIniciar && !esSalir && botonIniciar == null)
            {
                botonIniciar = b;
            }
        }

        // El Panel es el padre de los botones
        if (panelMenu == null && botonIniciar != null)
        {
            Transform padre = botonIniciar.transform.parent;
            if (padre != null && padre.GetComponent<Canvas>() == null)
            {
                panelMenu = padre.gameObject;
            }
        }
    }

    void ArreglarEventSystem()
    {
        EventSystem eventSystem = FindFirstObjectByType<EventSystem>();

        if (eventSystem == null)
        {
            GameObject go = new GameObject("EventSystem");
            eventSystem = go.AddComponent<EventSystem>();
        }

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        StandaloneInputModule moduloViejo = eventSystem.GetComponent<StandaloneInputModule>();
        if (moduloViejo != null)
        {
            Destroy(moduloViejo);
        }

        if (eventSystem.GetComponent<InputSystemUIInputModule>() == null)
        {
            InputSystemUIInputModule moduloNuevo = eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            moduloNuevo.AssignDefaultActions();
        }
#else
        if (eventSystem.GetComponent<StandaloneInputModule>() == null)
        {
            eventSystem.gameObject.AddComponent<StandaloneInputModule>();
        }
#endif
    }
}