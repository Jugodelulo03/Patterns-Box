using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PantallaResultados : MonoBehaviour
{
    public int PuntajeParaPasar;

    public TextMeshProUGUI txtPuntajeParaPasar;

    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoCorrectos;
    public TextMeshProUGUI textoIncorrectos;
    public TextMeshProUGUI textoTotal;
    public TextMeshProUGUI textoPuntosCorrectos;
    public TextMeshProUGUI textoPuntosIncorrectos;
    public TextMeshProUGUI textoPuntosTotales;

    //public TextMeshProUGUI textoPatronMasFallado;

    [Header("Referencias UI")]
    public Slider sliderRendimiento;

    void Start()
    {
        MostrarEstadisticas();
    }

    void MostrarEstadisticas()
    {
        var stats = GameStatsManager.Instance;

        float promedio = stats.GetTiempoPromedioPorMonitor();
        float porcentaje = stats.GetPorcentajeAciertos();
        string patronFallado = stats.GetPatronMasFallado();

        txtPuntajeParaPasar.text = PuntajeParaPasar.ToString()+ " pts";

        //izq
        textoCorrectos.text = stats.GetMonitoresCorrectos().ToString();
        textoIncorrectos.text = stats.GetMonitoresIncorrectos().ToString();
        textoTotal.text = stats.GetTotalRevisados().ToString();

        //der
        textoPuntosCorrectos.text = (stats.GetMonitoresCorrectos() * 1000).ToString() + " pts";
        textoPuntosIncorrectos.text = "-" + (stats.GetMonitoresIncorrectos() * 500).ToString() + " pts";
        textoPuntosTotales.text = ((stats.GetMonitoresCorrectos() * 1000)- (stats.GetMonitoresIncorrectos() * 500)).ToString() + " pts";

        //Slider

        sliderRendimiento.maxValue = PuntajeParaPasar*2;
        sliderRendimiento.value = ((stats.GetMonitoresCorrectos() * 1000) - (stats.GetMonitoresIncorrectos() * 500));


        //textoPromedioTiempo.text = promedio.ToString("F2") + "s";
        //textoPorcentaje.text = porcentaje.ToString("F2") + "%";
        //textoMaxErrores.text = stats.GetMaxErroresConsecutivos().ToString();
        //textoPatronMasFallado.text = (string.IsNullOrEmpty(patronFallado) ? "Ninguno" : patronFallado);
        textoNivel.text = "JORNADA " + stats.numeroNivel.ToString(); ;
    }
}
