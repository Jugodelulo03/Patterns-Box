using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGabinete : MonoBehaviour
{

    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera raycastCamera;

    public Animator buttonAnimator;  // Asigna el Animator del botón en el Inspector
    public float tiempoEspera = 2f; // Tiempo antes de volver a 0 (editable en el Inspector)

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
            buttonAnimator.SetBool("Open", true);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
