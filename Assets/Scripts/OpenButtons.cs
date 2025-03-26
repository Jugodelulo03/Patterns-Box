using UnityEngine;
using System.Collections;


public class OpenButtons : MonoBehaviour
{
    public MonitorManager monitorManager;

    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera raycastCamera;

    public Animator buttonAnimator;  // Asigna el Animator del botón en el Inspector
    public string animationParameter = "DeskState"; // Nombre del parámetro del Animator
    public float tiempoEspera = 2f; // Tiempo antes de volver a 0 (editable en el Inspector)

    public enum TipoBoton { Rojo, Verde } // Opción para seleccionar en el Inspector
    public TipoBoton tipoDeBoton; // Variable para elegir el tipo de botón

    [Header("Botones")]
    public GameObject[] boton;

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
                    if (tipoDeBoton == TipoBoton.Rojo)
                    {
                        buttonAnimator.SetInteger(animationParameter, 2); // Botón Rojo → DeskState = 2
                        gameObject.layer = LayerMask.NameToLayer("Default");
            }
                    else if (tipoDeBoton == TipoBoton.Verde)
                    {
                        buttonAnimator.SetInteger(animationParameter, 1); // Botón Verde → DeskState = 1
                        StartCoroutine(ResetAfterDelay()); // Inicia la espera para volver a 0
                        monitorManager.EvaluarRespuesta(PatronEnganoso.Ninguno);

            }
            foreach (GameObject b in boton)
            {
                b.layer = LayerMask.NameToLayer("Interactable");
            }
        } 

    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(tiempoEspera);
        buttonAnimator.SetInteger(animationParameter, 0); // Vuelve a 0 después de X segundos
    }
}
