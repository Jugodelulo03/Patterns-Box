using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

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
        // Mostrar video de carga
        if (panelVideoCarga != null) panelVideoCarga.SetActive(true);
        if (videoDeCarga != null) videoDeCarga.Play();

        // Determinar escena a cargar
        string escenaDestino = (puntajeFinal >= PuntajeParaPasar)
            ? "Nivel" + (nivelActual + 1)
            : "Nivel" + nivelActual;

        if (puntajeFinal >= PuntajeParaPasar)
        {
            PlayerPrefs.SetInt("numeroNivel", nivelActual + 1);
        }
        else
        {
            PlayerPrefs.SetInt("numeroNivel", nivelActual);
        }

            // Comenzar carga en segundo plano
            StartCoroutine(CargarEscenaAsync(escenaDestino));
    }

    System.Collections.IEnumerator CargarEscenaAsync(string nombreEscena)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nombreEscena);
        asyncLoad.allowSceneActivation = false;



        // Mientras la escena se está cargando, reproducir el video
        while (!asyncLoad.isDone)
        {
            // Si la carga está al menos al 90%, podemos activar la escena
            if (asyncLoad.progress >= 0.9f)
            {
                // Asegurarse de que la pantalla de carga dure al menos 1 segundo
                yield return new WaitForSeconds(1f);  // Esperar 1 segundo más, si ya se ha cargado al 90%

                // Detener el video justo antes de activar la escena
                if (videoDeCarga != null)
                {
                    videoDeCarga.Stop();
                }

                // Activar la escena cuando la carga está lista
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
