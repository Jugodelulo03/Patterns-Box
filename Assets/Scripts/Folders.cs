using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Folders : MonoBehaviour
{
    public MonoBehaviour playerController; // Asigna el script de MFPC en el Inspector
    public MonoBehaviour playerController2; // Asigna el script de MFPC en el Inspector

    public GameObject Infografia;
    public Animator Anicarpeta;

    public MonitorManager monitorManager;

    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera raycastCamera;

    public Animator buttonAnimator;  // Asigna el Animator del botón en el Inspector
    public float tiempoEspera = 2f; // Tiempo antes de volver a 0 (editable en el Inspector)

    public enum Carpeta { Carpeta1, Carpeta2, Carpeta3, Carpeta4, Carpeta5, Carpeta6 } // Opciones para el Inspector
    public Carpeta NumCarpeta; // Variable para elegir el tipo de botón

    [Header("Carpetas")]
    public GameObject[] Carpetas;

    private bool carpetaAbierta = false; // Controla si la carpeta ya está abierta

    void Update()
    {
        if (buttonAnimator.GetBool("Open") == true)
        {
            foreach (GameObject b in Carpetas)
            {
                b.layer = LayerMask.NameToLayer("Interactable");
            }
        }

        if (carpetaAbierta) return; // Si ya está abierta, no hace nada



        // Opcional: evita interacción si el cursor está sobre UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

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

    void CheckForButton()
    {
        if (buttonAnimator.GetBool("Open") == true)
        {
            foreach (GameObject b in Carpetas)
            {
                b.layer = LayerMask.NameToLayer("Interactable");
            }

            buttonAnimator.SetInteger("Folder", (int)NumCarpeta + 1); // Simplificado
        }
    }

    IEnumerator Abrir()
    {
        Debug.Log("Abrir Carpeta: " + NumCarpeta);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f; // Detiene el juego
        playerController.enabled = false; // Deshabilita el movimiento
        playerController2.enabled = false; // Deshabilita el movimiento

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Infografia.SetActive(true);
        Anicarpeta.SetBool("abierta", true);
    }

    public void abririnfo()
    {
        carpetaAbierta = true;
        StartCoroutine(Abrir());
    }

    public void cerrarinfo()
    {
        Time.timeScale = 1f; // Reanuda el juego
        Infografia.SetActive(false);
        Anicarpeta.SetBool("abierta", false);
        buttonAnimator.SetInteger("Folder", 0);

        playerController.enabled = true; // Habilita el movimiento
        playerController2.enabled = true; // Habilita el movimiento

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        carpetaAbierta = false; // Ahora puede volver a abrirse
    }
}
