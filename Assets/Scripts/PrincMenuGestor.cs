using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PrincMenuGestor : MonoBehaviour
{

    public Animator animador;
    public bool Salir, Iniciar, Ajustes;
    public int NumEscena;
    public GameObject FondoAjustes;

    [Header("Video de carga")]
    public GameObject panelVideoCarga;
    public VideoPlayer videoDeCarga;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Lista de resultados del raycast
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            bool clicEnBoton = false;

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<Button>() != null)
                {
                    clicEnBoton = true;
                    break;
                }
            }

            // Si no se hizo clic en un botón
            if (!clicEnBoton)
            {
                AnimInicio(); // Reproduce animación de retorno
            }
        }
    }

    public void IniciarF()
    {
        if (!Iniciar)
        {
            ResetEstados();
            Iniciar = true;
            animador.SetInteger("Estado", 1);
        }
        else
        {
            CambiarEscena();
        }
    }

    public void SalirF()
    {
        if (!Salir)
        {
            ResetEstados();
            Salir = true;
            animador.SetInteger("Estado", 3);
        }
        else
        {
            CerrarJuego();
        }
    }

    public void AjustesF()
    {
        if (!Ajustes)
        {
            ResetEstados();
            Ajustes = true;
            animador.SetInteger("Estado", 2);
        }
        else
        {
            if (FondoAjustes != null)
            {
                FondoAjustes.SetActive(true);
            }
            else
            {
                Debug.Log("AsignarUnPanelDeAjustes");
            }
        }
    }

    public void CambiarEscena()
    {
        // Mostrar video de carga
        if (panelVideoCarga != null)
        {
            panelVideoCarga.SetActive(true);  // Asegurarse de que el panel de video esté visible
        }

        if (videoDeCarga != null)
        {
            videoDeCarga.Play(); // Reproducir el video de carga
        }

        // Iniciar la carga de la escena con retraso
        StartCoroutine(CargarEscenaAsync(NumEscena));
    }

    System.Collections.IEnumerator CargarEscenaAsync(int escenaID)
    {
        // Esperamos al menos 1 segundo para mostrar el video (si la carga es rápida)
        yield return new WaitForSeconds(1f);

        // Comienza la carga de la escena de manera asíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(escenaID);
        asyncLoad.allowSceneActivation = false;

        // Mientras la escena se está cargando, reproducir el video
        while (!asyncLoad.isDone)
        {
            // Si la carga está al menos al 90%, podemos activar la escena
            if (asyncLoad.progress >= 0.9f)
            {
                // Esperamos un poco más para asegurar que el video se haya mostrado lo suficiente
                yield return new WaitForSeconds(1f); // Puedes ajustar el tiempo que necesites

                // Detener el video
                if (videoDeCarga != null)
                {
                    videoDeCarga.Stop();
                }

                // Activar la escena
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void AnimInicio()
    {
        ResetEstados();
        animador.SetInteger("Estado", 0);

    }

    public void CerrarJuego()
    {
        Application.Quit();
        Debug.Log("Aplicación cerrada");
    }

    private void ResetEstados()
    {
        Iniciar = false;
        Salir = false;
        Ajustes = false;
    }
}
