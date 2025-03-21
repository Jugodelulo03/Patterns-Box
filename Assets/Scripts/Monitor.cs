using UnityEngine;

public class Monitor : MonoBehaviour
{
    [Header("Configuración Manual del Patrón")]
    public PatronEnganoso patronAsignado;

    [Tooltip("Nombre exacto de la variante en Firebase (Ej: variante1, variante2...)")]
    public string varianteSeleccionada;

    [Header("Referencias de los textos UI")]
    public TMPro.TextMeshProUGUI texto1;
    public TMPro.TextMeshProUGUI texto2;
    public TMPro.TextMeshProUGUI texto3;
    public TMPro.TextMeshProUGUI texto4;
    public TMPro.TextMeshProUGUI texto5;

    public void SetTextos(TextoMonitor data)
    {
        if (texto1 != null) texto1.text = data.texto1;
        if (texto2 != null) texto2.text = data.texto2;
        if (texto3 != null) texto3.text = data.texto3;
        if (texto4 != null) texto4.text = data.texto4;
        if (texto5 != null) texto5.text = data.texto5;
    }
}
