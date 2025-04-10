using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Timer")]
    public float tiempoTotal = 60f;
    private float tiempoRestante;
    public TextMeshProUGUI textoTimer;

    [Header("Contador Correctos")]
    public int monitoresCorrectos = 0;
    public TextMeshProUGUI textoContadorCorrectos;

    [Header("Errores")]
    public int errores = 0;
    public int erroresMaximos = 3;

    [Header("Estado del Juego")]
    public bool tiempoActivo = true;
    [HideInInspector] public bool juegoFinalizado = false;

    [Header("Panel Game Over")]
    public GameObject Resultados; // Asigna el panel de pausa en el Inspector
    public Animator animador; // Asigna el Animator en el Inspector

    [Header("Controlador de Jugador (MFPC)")]
    public MonoBehaviour controladorJugador; // Tu script de movimiento


    [Header("Cámaras")]
    public Camera cameraJuego;
    public Camera cameraGameOver;

    void Start()
    {
        tiempoRestante = tiempoTotal;
        ActualizarUI();

        if (cameraJuego != null) cameraJuego.enabled = true;
        if (cameraGameOver != null) cameraGameOver.enabled = false;

        juegoFinalizado = false;
        tiempoActivo = true;
    }

    void Update()
    {
        if (tiempoActivo && !juegoFinalizado)
        {
            tiempoRestante -= Time.deltaTime;
            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                tiempoActivo = false;
                MostrarGameOver();
            }

            ActualizarUI();
        }
    }

    public void SumarMonitorCorrecto()
    {
        if (juegoFinalizado) return;

        monitoresCorrectos++;
        GameStatsManager.Instance?.RegistrarMonitor(true, ""); // No hay patrón engañoso en aciertos
        ActualizarUI();
    }

    public void SumarError(string patronEnganoso)
    {
        if (juegoFinalizado) return;

        errores++;
        GameStatsManager.Instance?.RegistrarMonitor(false, patronEnganoso);

        if (errores >= erroresMaximos)
        {
            MostrarGameOver();
        }
    }

    void ActualizarUI()
    {
        if (textoTimer != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            textoTimer.text = $"{minutos:00}:{segundos:00}";
        }

        if (textoContadorCorrectos != null)
        {
            textoContadorCorrectos.text = $"{monitoresCorrectos}";
        }
    }

    void MostrarGameOver()
    {
        juegoFinalizado = true;
        tiempoActivo = false;

        GameStatsManager.Instance?.EnviarEstadisticasAFirebase(); // Enviar datos a Firebase

        Resultados.SetActive(true);
        Time.timeScale = 0f; // Detiene el juego
        controladorJugador.enabled = false; // Deshabilita el movimiento
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (animador != null)
        {
            animador.SetBool("isPause", true);
        }

        if (controladorJugador != null)
            controladorJugador.enabled = false;

        if (cameraJuego != null) cameraJuego.enabled = false;
        if (cameraGameOver != null) cameraGameOver.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReiniciarEscena()
    {
        // Asegurarte de que el juego vuelva al tiempo normal si estaba pausado
        Time.timeScale = 1f;

        // Recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SalirDelJuego()
    {
        // Salir del juego (funciona solo en build, no en el editor)
        Application.Quit();

        // Solo para pruebas en editor (opcional):
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
