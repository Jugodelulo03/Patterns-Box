using UnityEngine;

public class DeskAnimationManager : MonoBehaviour
{
    public Animator anim;

    public void OpenDeskFinished()
    {
        anim.SetLayerWeight(1, 1); // Activa capa de botones
    }

    public void CloseDesk()
    {
        anim.SetLayerWeight(1, 0); // Desactiva capa de botones
        anim.SetInteger("DeskState", 0); // Retorna animacion Escritorio
        anim.SetInteger("BtnPress", 0); // RetornaAnimacionBotones
    }

    public void ButtonPressed(int buttonID)
    {
        anim.SetInteger("BtnPress", buttonID);
    }
}
