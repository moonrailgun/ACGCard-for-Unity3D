using UnityEngine;
using System.Collections;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursor;
    public Vector2 hotspot = new Vector2(0,5);

    private void Awake()
    {
        Cursor.SetCursor(cursor, hotspot, CursorMode.ForceSoftware);
    }
}