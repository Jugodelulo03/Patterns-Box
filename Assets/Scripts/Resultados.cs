using UnityEngine;
using TMPro;

public class PantallaResultados : MonoBehaviour
{
    public TextMeshProUGUI textoCorrectos;
    public TextMeshProUGUI textoIncorrectos;
    public TextMeshProUGUI textoTotal;
    public TextMeshProUGUI textoPromedioTiempo;
    public TextMeshProUGUI textoPorcentaje;
    public TextMeshProUGUI textoMaxErrores;
    public TextMeshProUGUI textoPatronMasFallado;
    public TextMeshProUGUI textoNivel;

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

        textoCorrectos.text = stats.GetMonitoresCorrectos().ToString();
        textoIncorrectos.text = stats.GetMonitoresIncorrectos().ToString();
        textoTotal.text = stats.GetTotalRevisados().ToString();
        textoPromedioTiempo.text = promedio.ToString("F2") + "s";
        textoPorcentaje.text = porcentaje.ToString("F2") + "%";
        textoMaxErrores.text = stats.GetMaxErroresConsecutivos().ToString();
        textoPatronMasFallado.text = (string.IsNullOrEmpty(patronFallado) ? "Ninguno" : patronFallado);
        textoNivel.text = "JORNADA: " + stats.numeroNivel.ToString(); ;
    }
}
