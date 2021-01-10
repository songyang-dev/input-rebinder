using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputRebinder.Runtime
{
    /// <summary>
    /// Component attached to an input system action in the menu
    /// </summary>
    public class InputRebinderAction : MonoBehaviour
    {
        /// <summary>
        /// Reference to the input action inside Unity
        /// </summary>
        [HideInInspector]
        public InputAction Action;

        /// <summary>
        /// TMPro of the action's name
        /// </summary>
        [Tooltip("Text of the action's name")]
        public TMPro.TextMeshProUGUI ActionName;

        /// <summary>
        /// Reference to the empty holding the input rebinder bindings
        /// </summary>
        public GameObject BindingsEmpty;

        /// <summary>
        /// List of bindings for this action
        /// </summary>
        [HideInInspector]
        public List<InputRebinderBinding> InputRebinderBindings;
    }
}