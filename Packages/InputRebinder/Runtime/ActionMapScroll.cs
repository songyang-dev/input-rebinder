using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InputRebinder.Runtime
{
    /// <summary>
    /// Controls the list of buttons for action maps
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class ActionMapScroll : MonoBehaviour
    {
        /// <summary>
        /// Content container for the buttons,
        /// simply add buttons as children thanks to the horizontal layout group
        /// </summary>
        [Tooltip("Container of buttons")]
        public GameObject ButtonContainer;

        /// <summary>
        /// Scroll view for the display of map contents
        /// </summary>
        [Tooltip("Scroll view for the display of map contents")]
        public ActionMapDisplayScroll contentScrollview;

        /// <summary>
        /// Displays the action map on the screen
        /// </summary>
        /// <param name="map"></param>
        public void ShowMap(ActionMapButton map)
        {
            contentScrollview.SetActiveMap(map.Map);
        }
    }
}