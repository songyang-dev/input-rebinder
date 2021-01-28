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

        /// <summary>
        /// Input Rebinder Actions of this map
        /// </summary>
        [HideInInspector]
        public List<InputRebinderAction> Actions;

        /// <summary>
        /// Input Rebinder Binding Pairs for better display
        /// </summary>
        [HideInInspector]
        public List<GameObject> BindingPairs;

        public InputRebinderAction GetInputRebinderAction(InputAction action)
        {
            foreach (var a in Actions)
            {
                if (a.Action.id == action.id)
                {
                    return a;
                }
            }

            throw new System.ArgumentException($"Input rebinder action not found: {action.name}");
        }
    }
}