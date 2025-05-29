using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorUIManager : MonoBehaviour
{
    public Texture2D cursorNormal;
    public Texture2D cursorHover;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private bool cursorEnBoton = false;

    void Update()
    {
        if (EstaSobreUIInteractiva())
        {
            if (!cursorEnBoton)
            {
                Cursor.SetCursor(cursorHover, hotSpot, cursorMode);
                cursorEnBoton = true;
            }
        }
        else
        {
            if (cursorEnBoton)
            {
                Cursor.SetCursor(cursorNormal, hotSpot, cursorMode);
                cursorEnBoton = false;
            }
        }
    }

    bool EstaSobreUIInteractiva()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null ||
                result.gameObject.GetComponent<Toggle>() != null ||
                result.gameObject.GetComponent<Slider>() != null ||
                result.gameObject.GetComponent<Dropdown>() != null)
            {
                return true;
            }
        }

        return false;
    }
}