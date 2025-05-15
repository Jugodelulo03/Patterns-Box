using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Folders : MonoBehaviour
{
    public MonoBehaviour playerController;
    public MonoBehaviour playerController2;

    public GameObject Infografia;
    public Animator Anicarpeta;

    public MonitorManager monitorManager;

    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera raycastCamera;

    public Animator buttonAnimator;
    public float tiempoEspera = 2f;

    public enum Carpeta { Carpeta1, Carpeta2, Carpeta3, Carpeta4, Carpeta5, Carpeta6 }
    public Carpeta NumCarpeta;

    [Header("Carpetas")]
    public GameObject[] Carpetas;

    //private bool carpetaAbierta = false;

    void Update()
    {
        if (buttonAnimator.GetBool("Open") == true)
        {
            foreach (GameObject b in Carpetas)
            {
                b.layer = LayerMask.NameToLayer("Interactable");
            }
        }

        // Opcional: evita interacción si el cursor está sobre UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if(raycastCamera != null && PauseMenu.GameIsPaused == false)
        {
            Ray ray = raycastCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Click detectado en: " + gameObject.name);
                        CheckForButton();
                        abririnfo();
                    }
                }
            }
        }
        
    }

    void CheckForButton()
    {
        if (buttonAnimator.GetBool("Open") == true)
        {
            foreach (GameObject b in Carpetas)
            {
                b.layer = LayerMask.NameToLayer("Interactable");
            }

            buttonAnimator.SetInteger("Folder", (int)NumCarpeta + 1);
        }
    }

    IEnumerator Abrir()
    {
        Debug.Log("Abrir Carpeta: " + NumCarpeta);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
        playerController.enabled = false;
        playerController2.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Infografia.SetActive(true);
        Anicarpeta.SetBool("abierta", true);
    }

    public void abririnfo()
    {
        PauseMenu.IsInteracting = true;
        // if (carpetaAbierta) return; // Solo bloquea si ya estaba abierta
        //carpetaAbierta = true;
        StartCoroutine(Abrir());
    }

    public void cerrarinfo()
    {
        
        Debug.Log("Cerrando carpeta, carpetaAbierta = false");

        Time.timeScale = 1f;
        Infografia.SetActive(false);
        Anicarpeta.SetBool("abierta", false);
        buttonAnimator.SetInteger("Folder", 0);

        playerController.enabled = true;
        playerController2.enabled = true;

        PauseMenu.IsInteracting = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //carpetaAbierta = false;
    }
}
