using UnityEngine;
using UnityEngine.UI;

public class CalidadGrafica : MonoBehaviour
{
    public Button[] botones;
    private int calidadActual = 3; // Por defecto en "Medio"

    void Start()
    {
        // Asigna eventos a los botones
        for (int i = 0; i < botones.Length; i++)
        {
            int index = i; // Necesario para evitar problemas con closures
            botones[i].onClick.AddListener(() => CambiarCalidad(index));
        }

        // Aplica la calidad inicial
        CambiarCalidad(calidadActual);
    }

    void CambiarCalidad(int index)
    {
        Debug.Log("Índice seleccionado: " + index); // Depuración
        calidadActual = index;
        QualitySettings.SetQualityLevel(index);

        // Cambia la apariencia del botón seleccionado
        for (int i = 0; i < botones.Length; i++)
        {
            bool seleccionado = (i == index);
            botones[i].targetGraphic.color = seleccionado ? Color.white : Color.black;
            botones[i].targetGraphic.color = seleccionado ? Color.black : Color.white;

        }
    }
}
