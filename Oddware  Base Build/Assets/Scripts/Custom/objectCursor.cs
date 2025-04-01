using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectCursor : MonoBehaviour
{
    public Texture2D cursorArrow;
    public Texture2D cursorHover;

    void Start()
    {
        //Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorHover, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);
    }
}
