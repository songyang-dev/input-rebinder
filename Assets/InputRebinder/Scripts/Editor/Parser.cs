using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Contains editor code for generating the prefabs from an inputaction file
/// </summary>
namespace InputRebinder
{

    /// <summary>
    /// Reads the input action asset (.inputactions)
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Decides what extra action to perform when parsing
        /// </summary>
        public enum ParserMode
        {
            /// Only analyze
            Analyze,
            /// Generate prefab
            Generate,
        }

        /// <summary>
        /// Must be set when parser is enabled
        /// </summary>
        public ParserMode Mode = ParserMode.Analyze;

        /// <summary>
        /// Inner class for what to do during parsing
        /// </summary>
        private ParsingAction parsingAction;

        /// <summary>
        /// Reference to the GUI window populator
        /// </summary>
        private UserGUI userGUI;

        /// <summary>
        /// Object holding the results of the analysis
        /// </summary>
        /// <returns></returns>
        private Analysis analysis = new Analysis();

        public Parser(ParserMode mode, UserGUI userGUI)
        {
            Mode = mode;
            this.parsingAction = new ParsingAction(mode, this.analysis);
            this.userGUI = userGUI;
        }

        #region Parsing recursions

        /// <summary>
        /// Main parsing function to be called by the GUI
        /// </summary>
        /// <param name="asset">Reference to the input action asset</param>
        public void Parse(InputActionAsset asset)
        {
            // initiate parsing actions
            parsingAction.Act(asset);

            var maps = asset.actionMaps;

            // recurse
            foreach (var map in maps)
            {
                Parse(map);
            }

            // parsing completed
            this.userGUI.HasAnalyzed = true;
        }

        /// <summary>
        /// Parses the action map
        /// </summary>
        /// <param name="map"></param>
        private void Parse(InputActionMap map)
        {
            // parsing actions
            parsingAction.Act(map);

            var actions = map.actions;

            foreach (var action in actions)
            {
                Parse(action);
            }
        }

        /// <summary>
        /// Parses the action
        /// </summary>
        /// <param name="action"></param>
        private void Parse(InputAction action)
        {
            // parsing actions
            parsingAction.Act(action);

            foreach (var b in action.bindings)
            {
                Parse(b);
            }
        }

        /// <summary>
        /// Parses the binding, which can be composite
        /// </summary>
        /// <param name="b"></param>
        private void Parse(InputBinding b)
        {
            parsingAction.Act(b);
        }
#endregion

        /// <summary>
        /// Class that acts on the input action parsed data
        /// </summary>
        private class ParsingAction
        {
            private ParserMode mode;

            private Analysis analysis;

            public ParsingAction(ParserMode mode, Analysis analysis)
            {
                this.mode = mode;
                this.analysis = analysis;
            }

            /// <summary>
            /// Does nothing
            /// </summary>
            /// <param name="asset"></param>
            internal void Act(InputActionAsset asset)
            {

            }

            /// <summary>
            /// In analyze mode, present the map to the GUI.
            /// In generate mode, execute the prefab creator for action maps.
            /// </summary>
            /// <param name="map"></param>
            internal void Act(InputActionMap map)
            {
                switch (mode)
                {
                    case ParserMode.Analyze:
                        
                        break;

                    case ParserMode.Generate:
                        break;

                    default:
                        throw new Exception($"Unknown parser mode: {mode}");
                }
            }

            /// <summary>
            /// In analyze mode, present the action to the GUI.
            /// In generate mode, execute the prefab creator for actions.
            /// </summary>
            /// <param name="action"></param>
            internal void Act(InputAction action)
            {
            }

            /// <summary>
            /// In analyze mode, present the binding to the GUI.
            /// In generate mode, execute the prefab creator for bindings.
            /// </summary>
            /// <param name="b"></param>
            internal void Act(InputBinding b)
            {
            }
        }
    }

}
