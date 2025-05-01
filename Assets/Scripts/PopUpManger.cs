using UnityEngine;

public class CambiarEstadoAnimator : MonoBehaviour
{
    // Referencia al Animator
    public Animator animador;

    public void CambiarEstado(int nuevoEstado)
    {
        if (animador != null)
        {
            animador.SetInteger("Estado", nuevoEstado);
        }
        else
        {
            Debug.LogWarning("No se asignó un Animator.");
        }
    }
}
