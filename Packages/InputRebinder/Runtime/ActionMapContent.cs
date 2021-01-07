using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace InputRebinder.Runtime
{
    /// <summary>
    /// Content of the viewport for a particular action map
    /// </summary>
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class ActionMapContent : MonoBehaviour
    {
        /// <summary>
        /// Map associated with this content
        /// </summary>
        [HideInInspector]
        public InputActionMap Map;
    }
}