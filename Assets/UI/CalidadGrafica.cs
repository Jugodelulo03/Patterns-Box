using UnityEngine;
using UnityEngine.UI;

public class CalidadGrafica : MonoBehaviour
{
    public Button[] botones;
    private int calidadActual = 3; // Por defecto en "Medio"

    void OnEnable()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            int index = i;
            botones[i].onClick.AddListener(() => CambiarCalidad(index));
        }

        // Carga valor guardado si existe
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            Debug.Log("Calidad guardada " + PlayerPrefs.GetInt("QualityLevel"));
            calidadActual = PlayerPrefs.GetInt("QualityLevel");
        }

        CambiarCalidad(calidadActual);
    }

    void OnDisable()
    {
        calidadActual = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        AplicarCalidadGuardada(calidadActual); // Restaura la selección visual
        QualitySettings.SetQualityLevel(calidadActual); // Restaura la calidad real
    }

    public void CambiarCalidad(int index)
    {
        Debug.Log("la calidad es " + index);
        calidadActual = index;
        QualitySettings.SetQualityLevel(index);
        ActualizarBotones();
    }

    private void ActualizarBotones()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            bool seleccionado = (i == calidadActual);
            botones[i].targetGraphic.color = seleccionado ? Color.black : Color.white;
        }
    }

    // 🔁 Exponer nivel actual
    public int ObtenerCalidadActual()
    {
        Debug.Log("Calidad Acutal es: " + calidadActual);
        return calidadActual;
    }

    // ⬅️ Aplicar desde fuera
    public void AplicarCalidadGuardada(int calidad)
    {
        calidadActual = calidad;
        QualitySettings.SetQualityLevel(calidad);

        // Actualiza visualmente los botones
        for (int i = 0; i < botones.Length; i++)
        {
            bool seleccionado = (i == calidad);
            botones[i].targetGraphic.color = seleccionado ? Color.black : Color.white;
        }
    }

}
