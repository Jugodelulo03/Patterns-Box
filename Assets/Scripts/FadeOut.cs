using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 2f;
    public GameObject PausaGestor;
    private Image fadeImage;
    private float timer = 0f;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;
        StartCoroutine(FadeOutRoutine());
    }

    private System.Collections.IEnumerator FadeOutRoutine()
    {
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - (timer / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        PausaGestor.SetActive(true);
        fadeImage.gameObject.SetActive(false); // Opcional: desactiva la imagen cuando termina el fade
    }
}
