using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

namespace InputRebinder.Runtime
{
    /// <summary>
    /// Input Rebinder runtime script for an action map button
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ActionMapButton : MonoBehaviour
    {
        [Tooltip("Reference to the text mesh pro text component of this button")]
        [SerializeField]
        private TextMeshProUGUI _text = default;

        /// <summary>
        /// Reference to the scroll view component for managing all map buttons
        /// </summary>
        [Tooltip("Reference to the scroll view component for managing all map buttons")]
        [SerializeField]
        public ActionMapScroll scrollView;

        /// <summary>
        /// Reference to the input system map the button represents
        /// </summary>
        [HideInInspector]
        public InputActionMap Map;

        /// <summary>
        /// Text of the button, usually the name of the action map
        /// </summary>
        /// <value></value>
        public string ButtonText
        {
            get => _text.text;
            set => _text.text = value;
        }

        /// <summary>
        /// Listener to the button click
        /// </summary>
        public void ClickMap()
        {
            // pass in the text of the button
            scrollView.ShowMap(this);
        }
    }
}
