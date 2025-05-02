using UnityEngine;

public class GabitAnimator : MonoBehaviour
{
    public Animator anim;

    public void OpenGabit()
    {
        anim.SetLayerWeight(1, 1); // Activa capa de botones
        anim.SetBool("Open", true); // Retorna animacion Escritorio
    }

    public void CloseGabit()
    {
        anim.SetLayerWeight(1, 0); // Desactiva capa de botones
        anim.SetBool("Open", false); // Retorna animacion Escritorio
        anim.SetInteger("Folder", 0); // RetornaAnimacionBotones
    }

    public void FolderSelect(int FolderID)
    {
        anim.SetInteger("Folder", FolderID);
    }
}
