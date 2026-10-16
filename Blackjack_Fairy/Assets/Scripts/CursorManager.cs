using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        UnlockCursor();
    }

    void Update()
    {
        // Keep unlocking if it gets locked again
        if (Cursor.lockState != CursorLockMode.None)
        {
            UnlockCursor();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            UnlockCursor();
        }
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
