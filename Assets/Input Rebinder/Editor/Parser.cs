using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Contains editor code for generating the prefabs from an inputaction file
/// </summary>
namespace InputRebinder.Editor
{

    /// <summary>
    /// Reads the input action asset (.inputactions)
    /// </summary>
    internal class Parser
    {
        /// <summary>
        /// Decides what extra action to perform when parsing
        /// </summary>
        internal enum ParserMode
        {
            /// Only analyze
            Analyze,
            /// Generate prefab
            Generate,
        }

        /// <summary>
        /// Must be set when parser is enabled
        /// </summary>
        private ParserMode mode;

        /// <summary>
        /// Inner class for what to do during parsing
        /// </summary>
        private IParsingAction parsingAction;

        /// <summary>
        /// Reference to the GUI window populator
        /// </summary>
        private UserGUI userGUI;

        /// <summary>
        /// Creates a parser
        /// </summary>
        /// <param name="action">Action done during parsing</param>
        internal Parser(IParsingAction action)
        {
            this.parsingAction = action;
        }

        // Change when Unity changes their input system structure
        #region Parsing recursions

        /// <summary>
        /// Main parsing function to be called by the GUI
        /// </summary>
        /// <param name="asset">Reference to the input action asset</param>
        internal void Parse(InputActionAsset asset)
        {
            // parsing actions: enter
            if (!parsingAction.ActOnEnter(asset)) return;

            var maps = asset.actionMaps;

            // recurse
            foreach (var map in maps)
            {
                Parse(map);
            }

            // parsing actions: exit
            parsingAction.ActOnExit(asset);
        }

        /// <summary>
        /// Parses the action map
        /// </summary>
        /// <param name="map"></param>
        private void Parse(InputActionMap map)
        {
            // parsing actions
            if (!parsingAction.ActOnEnter(map)) return;

            var actions = map.actions;

            foreach (var action in actions)
            {
                Parse(action);
            }

            parsingAction.ActOnExit(map);
        }

        /// <summary>
        /// Parses the action
        /// </summary>
        /// <param name="action"></param>
        private void Parse(InputAction action)
        {
            // parsing actions
            if (!parsingAction.ActOnEnter(action)) return;

            foreach (var b in action.bindings)
            {
                Parse(b, action);
            }

            parsingAction.ActOnExit(action);
        }

        /// <summary>
        /// Parses the binding, which can be composite
        /// </summary>
        /// <param name="b"></param>
        /// <param name="action"></param>
        private void Parse(InputBinding b, InputAction action)
        {
            parsingAction.Act(b, action);
        }
        #endregion

    }

}
