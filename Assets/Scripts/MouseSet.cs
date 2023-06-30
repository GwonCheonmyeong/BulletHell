using UnityEngine;
using System.Runtime.InteropServices;

public class MouseSet : MonoBehaviour
{
    [DllImport("user32.dll")] static extern bool SetCursorPos(int X, int Y);

    void Start()
    {
        Vector2 offset = Screen.mainWindowPosition;
        SetCursorPos(Screen.width / 2 + (int)offset.x, Screen.height / 2 + (int)offset.y);
    }
}