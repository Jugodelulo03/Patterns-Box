using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folders : MonoBehaviour
{
    public MonitorManager monitorManager;

    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera raycastCamera;

    public Animator buttonAnimator;  // Asigna el Animator del botón en el Inspector
    public float tiempoEspera = 2f; // Tiempo antes de volver a 0 (editable en el Inspector)

    public enum Carpeta { Carpeta1, Carpeta2, Carpeta3, Carpeta4, Carpeta5, Carpeta6 } // Opción para seleccionar en el Inspector
    public Carpeta NumCarpeta; // Variable para elegir el tipo de botón

    [Header("Carpetas")]
    public GameObject[] Carpetas;

    void Update()
    {
        Ray ray = raycastCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    CheckForButton();
                }
            }
        }
    }

    void CheckForButton()
    {
        if (buttonAnimator != null)
        {
            foreach (GameObject b in Carpetas)
            {
                b.layer = LayerMask.NameToLayer("Interactable");
            }

            if (NumCarpeta == Carpeta.Carpeta1)
            {
                buttonAnimator.SetInteger("Folder", 1); 
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else if (NumCarpeta == Carpeta.Carpeta2)
            {
                buttonAnimator.SetInteger("Folder", 2); 
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else if (NumCarpeta == Carpeta.Carpeta3)
            {
                buttonAnimator.SetInteger("Folder", 3);
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else if (NumCarpeta == Carpeta.Carpeta4)
            {
                buttonAnimator.SetInteger("Folder", 4);
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else if (NumCarpeta == Carpeta.Carpeta5)
            {
                buttonAnimator.SetInteger("Folder", 5);
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else if (NumCarpeta == Carpeta.Carpeta6)
            {
                buttonAnimator.SetInteger("Folder", 6);
                gameObject.layer = LayerMask.NameToLayer("Default");
            }


        }

    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(tiempoEspera);
        buttonAnimator.SetInteger("DeskState", 0); // Vuelve a 0 después de X segundos
    }
}

