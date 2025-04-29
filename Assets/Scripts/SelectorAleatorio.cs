using UnityEngine;

public class SelectorAleatorio : MonoBehaviour
{
    [SerializeField] private GameObject[] opciones;

    void Start()
    {
        // Desactiva todos primero
        foreach (GameObject opcion in opciones)
        {
            opcion.SetActive(false);
        }

        // Activa una al azar
        int indiceAleatorio = Random.Range(0, opciones.Length);
        opciones[indiceAleatorio].SetActive(true);
    }
}
