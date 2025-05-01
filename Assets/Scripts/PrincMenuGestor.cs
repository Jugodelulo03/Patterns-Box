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
    public bool Salir, Iniciar, Ajustes,CambioDePantalla=false;
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
                Debug.Log("Boton");
                if (result.gameObject.GetComponent<Button>() != null)
                {
                    clicEnBoton = true;
                    break;
                }
            }

            // Si no se hizo clic en un botón
            if (!clicEnBoton && CambioDePantalla==false)
            {
                Debug.Log("NoBoton");
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
            CambioDePantalla = true;
            animador.SetBool("Ingresar", CambioDePantalla);
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
        Debug.Log("Cargando nivel");
        SceneLoader.Instance.ConfigurarVideo(panelVideoCarga, videoDeCarga);
        SceneLoader.Instance.CargarEscenaGuardada();
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
        CambioDePantalla = false;
        animador.SetBool("Ingresar", false);
        Iniciar = false;
        Salir = false;
        Ajustes = false;
    }

    public void ResetearNivel()
    {
        if (GameStatsManager.Instance != null)
        {
            GameStatsManager.Instance.ResetearNivel();
        }
    }


}
