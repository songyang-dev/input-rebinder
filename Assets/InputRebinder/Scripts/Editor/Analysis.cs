using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

namespace InputRebinder
{

    /// <summary>
    /// Results of the analysis, structured in pairs
    /// </summary>
    internal class Analysis
    {
        /// <summary>
        /// Paired asset
        /// </summary>
        private InputActionAsset asset;
        
        /// <summary>
        /// Whether the map is to be ignored in generation
        /// </summary>
        private Dictionary<InputActionMap, bool> maps = new Dictionary<InputActionMap, bool>();

        /// <summary>
        /// Initialize an analysis for the paired asset
        /// </summary>
        /// <param name="asset"></param>
        internal Analysis(InputActionAsset asset)
        {
            this.asset = asset;
        }

        /// <summary>
        /// Generates GUI code and links analysis data upon
        /// entering an action map
        /// </summary>
        /// <param name="map">Input system map</param>
        /// <returns>A closure with GUI code</returns>
        internal Action AnalyzeMapOnEnter(InputActionMap map) => () => 
        {
            EditorGUILayout.BeginFoldoutHeaderGroup(true, map.name);
            GUILayout.Toggle(true, "Generate");
        };

        internal Action AnalyzeMapOnExit(InputActionMap map) => () =>
        {
            EditorGUILayout.EndFoldoutHeaderGroup();
        };
    }
}