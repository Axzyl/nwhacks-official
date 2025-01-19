using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
    public GameObject menu; // The menu GameObject
    public InputActionReference toggleMenuAction; // InputActionReference for the button
    public Transform cam;
    public Vector3 defaultPos;
    public Vector3 defaultRot;

    private void OnEnable()
    {
        if (toggleMenuAction != null)
        {
            // Subscribe to the action's performed event
            toggleMenuAction.action.performed += ToggleMenu;
        }
    }

    private void OnDisable()
    {
        if (toggleMenuAction != null)
        {
            // Unsubscribe from the action's performed event
            toggleMenuAction.action.performed -= ToggleMenu;
        }
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        if (menu != null)
        {
            // Toggle the menu's active state
            menu.SetActive(!menu.activeSelf);
            menu.transform.localEulerAngles = Vector3.up * cam.transform.localEulerAngles.y;
        }
    }
}
