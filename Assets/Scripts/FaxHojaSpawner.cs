using UnityEngine;
using System.Collections;

public class FaxHojaSpawner : MonoBehaviour
{
    [Header("Hoja y Movimiento")]
    public GameObject hojaPrefab;
    public Transform puntoSalidaHoja;
    public Material[] texturasErrores; // 3 materiales diferentes para los errores
    public float distanciaMovimiento = 1.5f;
    public float separacionVerticalEntreHojas = 0.1f; // Separación en eje Y
    public float duracionMovimiento = 1f;

    [Header("Sonido")]
    public AudioClip sonidoError;
    public AudioSource audioSource;

    private int errores = 0;

    public void DispararHojaError()
    {
        if (errores >= 3) return;

        errores++;

        // Reproducir sonido
        if (audioSource && sonidoError)
        {
            audioSource.PlayOneShot(sonidoError);
        }

        // Instanciar hoja
        GameObject hoja = Instantiate(hojaPrefab, puntoSalidaHoja.position, puntoSalidaHoja.rotation);

        // Asignar textura
        Renderer rend = hoja.GetComponent<Renderer>();
        int texturaIndex = errores - 1;
        if (rend != null && texturaIndex >= 0 && texturaIndex < texturasErrores.Length)
        {
            rend.material = texturasErrores[texturaIndex];
        }

        // Calcular destino con separación vertical en Y
        Vector3 destino = puntoSalidaHoja.position
                        + puntoSalidaHoja.forward * distanciaMovimiento
                        + Vector3.up * separacionVerticalEntreHojas * (errores - 1);

        // Mover hoja suavemente
        StartCoroutine(MoverHojaSuavemente(hoja, destino, duracionMovimiento));

        // Verificar fin de nivel
        if (errores >= 3)
        {
            Debug.Log("¡Nivel perdido por 3 errores!");
            // Aquí podés mostrar UI, cargar escena, etc.
        }
    }

    private IEnumerator MoverHojaSuavemente(GameObject hoja, Vector3 destino, float duracion)
    {
        float tiempo = 0f;
        Vector3 origen = hoja.transform.position;

        while (tiempo < duracion && hoja != null)
        {
            tiempo += Time.deltaTime;
            hoja.transform.position = Vector3.Lerp(origen, destino, tiempo / duracion);
            yield return null;
        }

        if (hoja != null)
        {
            hoja.transform.position = destino;
        }
    }
}
