using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SistemaDialogo : MonoBehaviour
{
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;
    public TextMeshProUGUI nombrePersonaje;
    public AudioSource audioSource;

    private CanvasGroup canvasGroup;
    private Queue<LineaDialogo> colaLineas = new Queue<LineaDialogo>();
    private Coroutine dialogoActivo;

    public float VelocidadEscritura;

    private void Awake()
    {
        canvasGroup = panelDialogo.GetComponent<CanvasGroup>();
        panelDialogo.SetActive(false);
    }

    public void IniciarDialogo(List<LineaDialogo> lineas)
    {
        colaLineas.Clear();
        if (dialogoActivo != null)
        {
            StopCoroutine(dialogoActivo);
        }

        foreach (LineaDialogo linea in lineas)
        {
            colaLineas.Enqueue(linea);
        }

        dialogoActivo = StartCoroutine(MostrarDialogoSecuencial());
        panelDialogo.SetActive(true);
    }

    private IEnumerator MostrarDialogoSecuencial()
    {
        while (colaLineas.Count > 0)
        {
            LineaDialogo lineaActual = colaLineas.Dequeue();

            nombrePersonaje.text = lineaActual.personaje;
            textoDialogo.text = "";  // Limpiamos el texto antes de cada nuevo diálogo

            // Activar panel y hacer fade in solo una vez si no está visible aún
            if (!panelDialogo.activeSelf)
            {
                panelDialogo.SetActive(true);
                yield return FadeCanvasGroup(canvasGroup, 0f, 1f, 0.5f);
            }

            // Reproduce el audio y escribe al mismo tiempo
            if (lineaActual.clipDeAudio != null)
            {
                audioSource.clip = lineaActual.clipDeAudio;
                audioSource.Play();
            }

            // Comienza a escribir el texto sin esperar a que termine antes de continuar
            yield return StartCoroutine(EfectoEscritura(lineaActual.texto));

            //yield return new WaitForSeconds(lineaActual.clipDeAudio.length); // Extra tiempo después del audio
            yield return new WaitForSeconds(2f); // Extra tiempo después del audio

        }

        // Fade Out después de mostrar todos los diálogos
        yield return FadeCanvasGroup(canvasGroup, 1f, 0f, 0.5f);
        panelDialogo.SetActive(false);
    }


    private IEnumerator EfectoEscritura(string texto)
    {
        // Se va mostrando el texto carácter por carácter
        for (int i = 0; i < texto.Length; i++)
        {
            textoDialogo.text += texto[i]; // Agrega un carácter a la vez
            yield return new WaitForSeconds(VelocidadEscritura); // Ajusta la velocidad aquí
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
    {
        float elapsed = 0f;
        group.alpha = from;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            group.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        group.alpha = to;
    }
}
