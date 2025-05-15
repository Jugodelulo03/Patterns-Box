using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEditor;

public class PantallaResultados : MonoBehaviour
{
    [Header("Puntaje necesario")]
    public float PuntajeParaPasar;

    [Header("Referencias UI")]
    public TextMeshProUGUI txtPuntajeParaPasar;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoCorrectos;
    public TextMeshProUGUI textoIncorrectos;
    public TextMeshProUGUI textoTotal;
    public TextMeshProUGUI textoPuntosCorrectos;
    public TextMeshProUGUI textoPuntosIncorrectos;
    public TextMeshProUGUI textoPuntosTotales;
    public Slider sliderRendimiento;

    [Header("Botón continuar")]
    public Button botonContinuar;

    [Header("Video de carga")]
    public GameObject panelVideoCarga;
    public VideoPlayer videoDeCarga;

    private int puntajeFinal;
    private int nivelActual;

    void Start()
    {
        PauseMenu.IsInteracting = true;
        MostrarEstadisticas();
        botonContinuar.onClick.AddListener(VerificarResultado);
    }

    void MostrarEstadisticas()
    {
        var stats = GameStatsManager.Instance;

        txtPuntajeParaPasar.text = PuntajeParaPasar.ToString() + " pts";

        textoCorrectos.text = stats.GetMonitoresCorrectos().ToString();
        textoIncorrectos.text = stats.GetMonitoresIncorrectos().ToString();
        textoTotal.text = stats.GetTotalRevisados().ToString();

        int puntosCorrectos = stats.GetMonitoresCorrectos() * 1000;
        int puntosIncorrectos = stats.GetMonitoresIncorrectos() * 500;
        puntajeFinal = puntosCorrectos - puntosIncorrectos;

        textoPuntosCorrectos.text = puntosCorrectos.ToString() + " pts";
        textoPuntosIncorrectos.text = "-" + puntosIncorrectos.ToString() + " pts";
        textoPuntosTotales.text = puntajeFinal.ToString() + " pts";

        sliderRendimiento.maxValue = PuntajeParaPasar + (PuntajeParaPasar / 4);
        sliderRendimiento.value = puntajeFinal;

        nivelActual = stats.numeroNivel;
        textoNivel.text = "JORNADA " + nivelActual;
    }

    void VerificarResultado()
    {
        Time.timeScale = 1f;

        nivelActual = PlayerPrefs.GetInt("numeroNivel");
        //cambie la forma en la que se Guarda el nivel para que use la funcion de Guardar
        if (puntajeFinal >= PuntajeParaPasar)
            GameStatsManager.Instance.GuardarNivel(nivelActual + 1);
        else
            GameStatsManager.Instance.GuardarNivel(nivelActual);

        //Lineas para el cambio de escenas
        SceneLoader.Instance.ConfigurarVideo(panelVideoCarga, videoDeCarga);
        SceneLoader.Instance.CargarEscenaGuardada();
        Debug.Log("Cargando escena");

        /*Nota:
            Hice un cambio en este codigo para que separa la pantalla de carga con
            la carga de los niveles asi con llamar las dos funciones dentro del script se pueda 
            hacer exactamente lo mismo de las corrutinas y asi centralizar esta accion 
            en la clase "ScenceLoader" en vez de tener una por cada codigo "Resulados" y "PrincMenuGestor".

            Nomas me parecio mas comodo para poder editar despues.
        */
    }


}
