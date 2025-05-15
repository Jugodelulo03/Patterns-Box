using UnityEngine;
using UnityEngine.UI;

public class CharacterStateController : MonoBehaviour
{
    // Cada estado tiene 2 elementos: [0] = Cara, [1] = Brazos
    public Image[] Idle = new Image[2];
    public Image[] Salir = new Image[2];
    public Image[] Iniciar = new Image[2];
    public Image[] Ajustes = new Image[2];
    public BodyLookControllerUI BodyScript;
    // Array con los 4 estados
    private Image[][] estados;

    void Start()
    {
        // Agrupar todos los estados
        estados = new Image[][] { Idle, Salir, Iniciar, Ajustes };

        // Activar el primer estado por defecto
        CambiarEstado(0);
    }

    public void CambiarEstado(int estadoIndex)
    {
        if(estadoIndex == 1 || estadoIndex == 3)
        {
            BodyScript.flip = false;
        }
        else
        {
            BodyScript.flip = true;
        }

        if (estadoIndex < 0 || estadoIndex >= estados.Length)
        {
            Debug.LogWarning("Índice de estado inválido.");
            return;
        }

        // Desactivar todos los estados
        for (int i = 0; i < estados.Length; i++)
        {
            estados[i][0].gameObject.SetActive(false); // Cara
            estados[i][1].gameObject.SetActive(false); // Brazos
        }

        // Activar el estado seleccionado
        estados[estadoIndex][0].gameObject.SetActive(true);  // Cara
        estados[estadoIndex][1].gameObject.SetActive(true);  // Brazos
    }
}
