using UnityEngine;
using System.Collections;

public class NonePattern : MonoBehaviour
{

    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera raycastCamera;

    public Animator buttonAnimator;  // Asigna el Animator del botón en el Inspector
    public string animationParameter = "DeskState"; // Nombre del parámetro del Animator
    public float tiempoEspera = 2f; // Tiempo antes de volver a 0 (editable en el Inspector)

    void Update()
    {
        Ray ray = raycastCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (Input.GetKeyDown(KeyCode.E))
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
                buttonAnimator.SetInteger(animationParameter, 1); // Cambia a 1
                StartCoroutine(ResetAfterDelay()); // Inicia la espera para volver a 0
            }
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(tiempoEspera);
        buttonAnimator.SetInteger(animationParameter, 0); // Vuelve a 0 después de X segundos
    }
}

