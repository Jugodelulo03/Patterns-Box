using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoTrigger : MonoBehaviour
{
    public SistemaDialogo sistemaDialogo;
    public float tiempoInicio = 2f; // Tiempo de espera antes de iniciar el diálogo

    [Header("Contenido del diálogo")]
    public string personaje;
    [TextArea(2, 5)]
    public string texto;
    public AudioClip clipDeAudio;

    void Start()
    {
        StartCoroutine(IniciarTrasEspera());
    }

    IEnumerator IniciarTrasEspera()
    {
        yield return new WaitForSeconds(tiempoInicio);

        List<LineaDialogo> lineas = new List<LineaDialogo>
        {
            new LineaDialogo
            {
                personaje = personaje,
                texto = texto,
                clipDeAudio = clipDeAudio
            }
        };

        sistemaDialogo.IniciarDialogo(lineas);
    }
}
