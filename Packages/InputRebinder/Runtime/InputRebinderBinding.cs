using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputRebinder.Runtime
{
    /// <summary>
    /// Component of a input system binding
    /// </summary>
    public class InputRebinderBinding : MonoBehaviour
    {
        /// <summary>
        /// Reference to the binding in Unity
        /// </summary>
        [HideInInspector]
        public InputBinding Binding;

        /// <summary>
        /// TMPro component of the text of the current binding
        /// </summary>
        public TMPro.TextMeshProUGUI CurrentBindingText;
    }
}