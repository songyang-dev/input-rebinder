using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputRebinder.Runtime
{
    /// <summary>
    /// Controls the highest level of the input rebinder canvas
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class InputRebinderCanvas : MonoBehaviour
    {
        /// <summary>
        /// Gameobject with the version number and name
        /// </summary>
        [Tooltip("Text with the version number and name")]
        public TMPro.TextMeshProUGUI PluginVersion;


        /// <summary>
        /// Listener to when the close button is clicked
        /// </summary>
        public void ClickClose()
        {
            this.gameObject.SetActive(false);
        }
    }
}