using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCatcher : MonoBehaviour
{
    public void Move(InputAction.CallbackContext callback)
    {
        var result = callback.ReadValue<Vector2>();
        // Debug.Log(result);
    }
}
