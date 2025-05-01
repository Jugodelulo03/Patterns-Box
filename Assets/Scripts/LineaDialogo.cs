using UnityEngine;

[System.Serializable]
public class LineaDialogo
{
    public string personaje;

    [TextArea(2, 5)]
    public string texto;

    public AudioClip clipDeAudio;
}
