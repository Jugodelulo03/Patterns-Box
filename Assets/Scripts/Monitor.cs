using UnityEngine;

public class Monitor : MonoBehaviour
{
    public PatronEnganoso patronAsignado;

    public TMPro.TextMeshProUGUI texto1;
    public TMPro.TextMeshProUGUI texto2;
    public TMPro.TextMeshProUGUI texto3;
    public TMPro.TextMeshProUGUI texto4;
    public TMPro.TextMeshProUGUI texto5;

    public void SetTextos(TextoMonitor data)
    {
        texto1.text = data.texto1;
        texto2.text = data.texto2;
        texto3.text = data.texto3;
        texto4.text = data.texto4;
        texto5.text = data.texto5;
    }
}
