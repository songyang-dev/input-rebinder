using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputRebinder.Runtime
{
    /// <summary>
    /// Component attached to an input system action in the menu
    /// </summary>
    public class InputRebinderAction : MonoBehaviour
    {
        /// <summary>
        /// TMPro of the action's name
        /// </summary>
        [Tooltip("Text of the action's name")]
        public TMPro.TextMeshProUGUI ActionName;

        /// <summary>
        /// TMPro of the current binding
        /// </summary>
        [Tooltip("Text of the current binding")]
        public TMPro.TextMeshProUGUI CurrentBinding;
    }
}