using UnityEngine;

public class BodyLookControllerUI : MonoBehaviour
{
    public RectTransform cabeza, caras, brazos;
    public float maxOffset = 20f; // En píxeles


    public float DesCara, DesBrazos, DesCabeza, VelDes;
    public bool flip=true;
    private Vector3 cabezaInicial, carasInicial, BrazosInicial;
    private RectTransform personajeRect;
    private Vector2 mousePosCanvas;
    private Vector2 mouseDir;

    void Start()
    {
        personajeRect = GetComponent<RectTransform>();
        cabezaInicial = cabeza.anchoredPosition;
        BrazosInicial = brazos.anchoredPosition;
        carasInicial = caras.anchoredPosition;
    }

    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            personajeRect.parent as RectTransform,
            Input.mousePosition,
            null,
            out mousePosCanvas
        );

        Vector2 personajePosCanvas = personajeRect.anchoredPosition;
        mouseDir = (mousePosCanvas - personajePosCanvas).normalized;

        // Flip horizontal solo si es necesario
        if(flip == true)
        {
            Vector3 scale = personajeRect.localScale;
            if (mousePosCanvas.x < personajePosCanvas.x && scale.x > 0)
            {
                scale.x *= -1;
                personajeRect.localScale = scale;
            }
            else if (mousePosCanvas.x > personajePosCanvas.x && scale.x < 0)
            {
                scale.x *= -1;
                personajeRect.localScale = scale;
            }
        }
        

        bool isFlipped = personajeRect.localScale.x < 0;

        // Mover partes
        MoverParte(cabeza, cabezaInicial, DesCabeza, true, false);   // solo Y, sin inversión
        MoverParte(brazos, cabezaInicial, DesCabeza, true, false);   // solo Y, sin inversión
        MoverParte(caras, carasInicial, DesCara, false, isFlipped); // XY, con inversión en X si flip
    }

    void MoverParte(RectTransform parte, Vector3 posInicial, float intensidad, bool soloY = false, bool invertirX = false)
    {
        Vector2 dir = mouseDir;

        if (soloY)
            dir.x = 0f;

        if (invertirX)
            dir.x *= -1f;

        Vector3 offset = (Vector3)(dir * maxOffset * intensidad);
        parte.anchoredPosition = Vector3.Lerp(parte.anchoredPosition, posInicial + offset, Time.deltaTime * VelDes);
    }

    public void StateFlip(bool Flip)
    {
        flip = Flip;
    }
}
