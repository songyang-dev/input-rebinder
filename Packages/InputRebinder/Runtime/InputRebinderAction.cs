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
        /// Reference to the parent where bindings are added
        /// </summary>
        public GameObject BindingsParent;

        /// <summary>
        /// List of bindings for this action
        /// </summary>
        [HideInInspector]
        public List<InputRebinderBinding> InputRebinderBindings;

        /// <summary>
        /// The last pair is used to populate the window
        /// </summary>
        [HideInInspector]
        public BindingPair LastPair;
    }
}