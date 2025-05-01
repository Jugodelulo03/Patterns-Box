using TMPro;
using UnityEngine;

public class NivelTextoUI : MonoBehaviour
{
    public GameObject ResetButtom;
    private TextMeshProUGUI textoNivel;
    public TextMeshProUGUI Cargar, PopUpCargar;


    void Start()
    {
        textoNivel = GetComponent<TextMeshProUGUI>();
        ActualizarTextoNivel();
    }

    public void ActualizarTextoNivel()
    {
        int nivelActual = GameStatsManager.Instance.numeroNivel;
            if (nivelActual > 0)
            {
                ResetButtom.SetActive(true);
                Debug.Log("Texto actual: " + "Nivel: " + nivelActual);
                textoNivel.text = "Nivel: " + nivelActual;
                Cargar.text = "CARGAR";
                PopUpCargar.text = "¿Continuar la jornada laboral?";
                
            }
            else
            {
                ResetButtom.SetActive(false);
                Debug.Log("Texto actual: " + "Nivel: " + nivelActual);
                textoNivel.text = "BUSCAR EMPLEO";
                Cargar.text = "BUSCAR";
                PopUpCargar.text = "¿Buscar una jornada laboral?";
            }
    }

}
