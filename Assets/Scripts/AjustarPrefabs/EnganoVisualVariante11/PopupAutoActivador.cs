using UnityEngine;

public class PopupAutoActivador : MonoBehaviour
{
    public GameObject popupPanel;
    public float Cooldown;

    void Start()
    {
        // Activa el popup cada 3 segundos, empezando luego de 3 segundos
        InvokeRepeating("MostrarPopup", Cooldown, Cooldown);
    }

    void MostrarPopup()
    {
        popupPanel.SetActive(true);
    }
}