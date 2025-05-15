using UnityEngine;

public class MonitorButton : MonoBehaviour
{

    public MonitorManager monitorManager;
    public PatronEnganoso patronDelBoton;
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera raycastCamera;

    void Update()
    {
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
                        
                        monitorManager.EvaluarRespuesta(patronDelBoton);
                    }
                }
            }
        }
    }

    public void cerrar()
    {
        
        monitorManager.EvaluarRespuesta(patronDelBoton);
    }
}