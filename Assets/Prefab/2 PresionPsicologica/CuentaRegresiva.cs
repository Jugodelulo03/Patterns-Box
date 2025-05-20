using TMPro;
using UnityEngine;

public class CuentaRegresiva : MonoBehaviour
{
    public TextMeshProUGUI textoCuentaRegresiva;
    private float tiempoRestante = 10f;
    private bool enCuentaRegresiva = true;

    void Update()
    {
        if (enCuentaRegresiva)
        {
            tiempoRestante -= Time.deltaTime;
            int segundos = Mathf.CeilToInt(tiempoRestante);
            textoCuentaRegresiva.text = segundos.ToString();

            if (tiempoRestante <= 0f)
            {
                enCuentaRegresiva = false;
                textoCuentaRegresiva.text = "¡Oferta Finalizada!";
            }
        }
    }
}
