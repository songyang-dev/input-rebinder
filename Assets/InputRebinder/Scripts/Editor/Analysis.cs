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
        #region Analysis results
        /// <summary>
        /// Paired asset
        /// </summary>
        private InputActionAsset asset;

        /// <summary>
        /// Whether the map is to be ignored in generation.
        /// Changes depending on user input
        /// </summary>
        /// <typeparam name="InputActionMap">Action map from Unity</typeparam>
        /// <typeparam name="bool">Whether to generate the prefab for this map</typeparam>
        /// <returns>Empty dictionary</returns>
        private Dictionary<InputActionMap, bool> maps = new Dictionary<InputActionMap, bool>();

        #endregion

        #region UI parameters
        /// <summary>
        /// Used to display the window UI
        /// </summary>
        /// <typeparam name="InputActionMap">Action map from Unity</typeparam>
        /// <typeparam name="bool">Whether the UI is folding this map or not</typeparam>
        /// <returns>Empty dictionary</returns>
        private Dictionary<InputActionMap, bool> mapFoldout = new Dictionary<InputActionMap, bool>();

        #endregion

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
            // foldout
            if (mapFoldout.ContainsKey(map))
                mapFoldout[map] = EditorGUILayout.BeginFoldoutHeaderGroup(mapFoldout[map], map.name);
            else
                mapFoldout.Add(map, EditorGUILayout.BeginFoldoutHeaderGroup(true, map.name));

            // indent
            EditorGUI.indentLevel++;

            if (mapFoldout[map])
            {
                // generation option
                if (maps.ContainsKey(map))
                    maps[map] = EditorGUILayout.ToggleLeft("Generate", maps[map]);
                else
                    maps.Add(map, EditorGUILayout.ToggleLeft("Generate", true));
            }
        };

        internal Action AnalyzeMapOnExit(InputActionMap map) => () =>
        {
            // un-indent
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();
        };
    }
}