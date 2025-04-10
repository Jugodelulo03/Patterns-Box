using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;

public class PantallaCarga : MonoBehaviour
{
    public string escenaASiguiente = "NombreDeLaSiguienteEscena";
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI textoCargando;

    private bool escenaCargada = false;

    void Start()
    {
        // Empieza a cargar la escena en segundo plano
        StartCoroutine(CargarEscenaAsync());

        // Empieza a reproducir el video
        videoPlayer.Play();

        // Escuchamos si el video termina
        videoPlayer.loopPointReached += OnVideoTerminado;
    }

    IEnumerator CargarEscenaAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(escenaASiguiente);
        asyncLoad.allowSceneActivation = false;

        // Opcional: animación de texto
        while (!asyncLoad.isDone)
        {
            // Por ejemplo: "...", "....", etc.
            textoCargando.text = "Cargando" + new string('.', Mathf.FloorToInt(Time.time % 4));

            // Esperamos a que esté casi completa
            if (asyncLoad.progress >= 0.9f && !escenaCargada)
            {
                escenaCargada = true;

                // Si el video ya terminó, activamos la escena
                if (!videoPlayer.isPlaying)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    void OnVideoTerminado(VideoPlayer vp)
    {
        if (escenaCargada)
        {
            // Si ya está cargada, pasamos
            SceneManager.LoadScene(escenaASiguiente);
        }
    }
}
