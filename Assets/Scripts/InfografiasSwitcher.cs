using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfografiasSwitcher : MonoBehaviour
{
    [Header("Lista de objetos a mostrar")]
    public List<GameObject> objetos;

    private int indiceActual = 0;

    void Start()
    {
        ActualizarVisibilidad();
    }

    public void MostrarSiguiente()
    {
        if (objetos.Count == 0) return;

        indiceActual = (indiceActual + 1) % objetos.Count;
        ActualizarVisibilidad();
    }

    public void MostrarAnterior()
    {
        if (objetos.Count == 0) return;

        indiceActual = (indiceActual - 1 + objetos.Count) % objetos.Count;
        ActualizarVisibilidad();
    }

    private void ActualizarVisibilidad()
    {
        for (int i = 0; i < objetos.Count; i++)
        {
            objetos[i].SetActive(i == indiceActual);
        }
    }
}
