using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InputRebinder.Runtime
{
    /// <summary>
    /// Content window of the selected action map
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class ActionMapDisplayScroll : MonoBehaviour
    {
        /// <summary>
        /// Reference to the viewport
        /// </summary>
        [Tooltip("Reference to the viewport")]
        public GameObject Viewport;

        /// <summary>
        /// List of contents display in the scrollview
        /// </summary>
        [HideInInspector]
        public List<ActionMapContent> Contents = new List<ActionMapContent>();

        /// <summary>
        /// Finds the action map content component of a input action map in this
        /// scrollview
        /// </summary>
        /// <param name="map">Input system map to look for</param>
        /// <returns>Associated ActionMapContent</returns>
        public ActionMapContent GetActionMapContent(InputActionMap map)
        {
            foreach (var mapContent in Contents)
            {
                if (mapContent.Map.id == map.id)
                {
                    return mapContent;
                }
            }
            throw new ArgumentException($"Map content not found: {map.name}");
        }

        /// <summary>
        /// Displays the content for the given map and hides all other maps
        /// </summary>
        /// <param name="mapContent"></param>
        public void SetActiveMap(InputActionMap map)
        {
            foreach (var mapContent in Contents)
            {
                if (mapContent.Map.id == map.id)
                {
                    mapContent.gameObject.SetActive(true);
                    GetComponent<ScrollRect>().content = mapContent.GetComponent<RectTransform>();
                }
                else mapContent.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Displays the first map, used after generating the prefab
        /// </summary>
        public void ActivateFirstMapContent()
        {
            if (Contents.Count != 0)
                SetActiveMap(Contents[0].Map);
        }
    }
}