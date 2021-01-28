using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

namespace InputRebinder.Editor
{

    /// <summary>
    /// Results of the analysis, structured in pairs
    /// </summary>
    internal class Analysis : IParsingAction
    {
        #region Analysis results

        /// <summary>
        /// Whether the map is to be ignored in generation.
        /// Changes depending on user input
        /// </summary>
        /// <typeparam name="InputActionMap">Action map from Unity</typeparam>
        /// <typeparam name="bool">Whether to generate the prefab for this map</typeparam>
        /// <returns>Whether to generate the prefab for this map</returns>
        internal Dictionary<InputActionMap, bool> maps = new Dictionary<InputActionMap, bool>();

        /// <summary>
        /// Generation options relating to actions
        /// </summary>
        /// <typeparam name="InputAction">Action from Unity</typeparam>
        /// <typeparam name="bool">Whether to generate</typeparam>
        /// <returns>Whether to generate</returns>
        internal Dictionary<InputAction, bool> actions = new Dictionary<InputAction, bool>();

        #endregion

        #region UI parameters

        /// <summary>
        /// GUI code to display the results
        /// </summary>
        internal List<Action> Results;

        /// <summary>
        /// Used to display the window UI for maps
        /// </summary>
        /// <typeparam name="InputActionMap">Action map from Unity</typeparam>
        /// <typeparam name="bool">Whether the UI is folding this map or not</typeparam>
        /// <returns>Whether the UI is folding this map or not</returns>
        private Dictionary<InputActionMap, bool> mapFoldout = new Dictionary<InputActionMap, bool>();

        /// <summary>
        /// Used to display the window UI for actions
        /// </summary>
        /// <typeparam name="InputAction">Action from Unity</typeparam>
        /// <typeparam name="bool">Whether the UI is folding this action or not</typeparam>
        /// <returns>Analysis information about the action</returns>
        private Dictionary<InputAction, bool> actionFoldout = new Dictionary<InputAction, bool>();

        #endregion

        /// <summary>
        /// Initialize an analysis for the paired asset
        /// </summary>
        internal Analysis()
        {
            this.Results = new List<Action>();
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
                var checkMark = new GUIContent("Generate action map", "Check if you want this action map to be in the generated prefab");
                if (maps.ContainsKey(map))
                    maps[map] = EditorGUILayout.ToggleLeft(checkMark, maps[map]);
                else
                    maps.Add(map, EditorGUILayout.ToggleLeft(checkMark, true));
            }
        };

        /// <summary>
        /// Closes UI groups
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        internal Action AnalyzeMapOnExit(InputActionMap map) => () =>
        {
            // un-indent
            EditorGUI.indentLevel--;
            EditorGUILayout.EndFoldoutHeaderGroup();
        };

        /// <summary>
        /// Generates GUI code for actions and links analysis data
        /// </summary>
        /// <param name="action">Input systemaction</param>
        /// <returns>GUI code</returns>
        internal Action AnalyzeActionOnEnter(InputAction action) => () =>
        {
            // do not display when the map is not folded
            if (!mapFoldout[action.actionMap]) return;
            // grey out the action when the map is not generated
            if (!maps[action.actionMap]) EditorGUI.BeginDisabledGroup(true);

            // indent
            EditorGUI.indentLevel++;

            // generation option
            var checkMark = new GUIContent(action.name, "Check if you want to include the action in the prefab");
            if (actions.ContainsKey(action))
                actions[action] = EditorGUILayout.ToggleLeft(checkMark, actions[action]);
            else
                actions.Add(action, EditorGUILayout.ToggleLeft(checkMark, true));

        };

        /// <summary>
        /// Remove indentation and closes UI groups
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        internal Action AnalyzeActionOnExit(InputAction action) => () =>
        {
            if (!mapFoldout[action.actionMap]) return;
            if (!maps[action.actionMap]) EditorGUI.EndDisabledGroup();
            // un-indent
            EditorGUI.indentLevel--;
        };

        #region Interface implementation
        public bool ActOnEnter(InputActionAsset asset)
        {
            return true;
        }

        public bool ActOnEnter(InputActionMap map)
        {
            this.Results.Add(AnalyzeMapOnEnter(map));
            return true;
        }

        public bool ActOnEnter(InputAction action)
        {
            this.Results.Add(AnalyzeActionOnEnter(action));
            return true;
        }

        public void Act(InputBinding b, InputAction action)
        {
        }

        public void ActOnExit(InputActionAsset asset)
        {
        }

        public void ActOnExit(InputActionMap map)
        {
            this.Results.Add(AnalyzeMapOnExit(map));
        }

        public void ActOnExit(InputAction action)
        {
            this.Results.Add(AnalyzeActionOnExit(action));
        }

        #endregion
    }
}