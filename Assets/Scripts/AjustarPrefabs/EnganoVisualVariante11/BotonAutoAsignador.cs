using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotonAutoAsignador : MonoBehaviour
{
    IEnumerator Start()
    {
        // Esperar 1 frame para asegurarse de que Camera.main est� disponible
        yield return null;

        Button btn = GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogWarning("BotonAutoAsignador: No se encontr� el componente Button.");
            yield break;
        }

        if (Camera.main == null)
        {
            Debug.LogWarning("BotonAutoAsignador: No se encontr� Camera.main.");
            yield break;
        }

        MonitorLookInteraction interaction = Camera.main.GetComponent<MonitorLookInteraction>();
        if (interaction == null)
        {
            Debug.LogWarning("BotonAutoAsignador: No se encontr� MonitorLookInteraction en la c�mara.");
            yield break;
        }

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(interaction.ExitUIInteraction);

        Debug.Log("BotonAutoAsignador: Evento onClick asignado correctamente.");
    }
}

