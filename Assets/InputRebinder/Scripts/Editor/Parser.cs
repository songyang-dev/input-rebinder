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
        internal ParserMode Mode = ParserMode.Analyze;

        /// <summary>
        /// Inner class for what to do during parsing
        /// </summary>
        private ParsingAction parsingAction;

        /// <summary>
        /// Reference to the GUI window populator
        /// </summary>
        private UserGUI userGUI;

        internal Parser(ParserMode mode, UserGUI userGUI)
        {
            Mode = mode;
            this.parsingAction = new ParsingAction(mode, userGUI);
            this.userGUI = userGUI;
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
            parsingAction.ActOnEnter(asset);

            var maps = asset.actionMaps;

            // recurse
            foreach (var map in maps)
            {
                Parse(map);
            }

            // parsing actions: exit
            parsingAction.ActOnExit(asset);

            this.userGUI.HasAnalyzed = true;
        }

        /// <summary>
        /// Parses the action map
        /// </summary>
        /// <param name="map"></param>
        private void Parse(InputActionMap map)
        {
            // parsing actions
            parsingAction.ActOnEnter(map);

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
            parsingAction.ActOnEnter(action);

            foreach (var b in action.bindings)
            {
                Parse(b);
            }

            parsingAction.ActOnExit(action);
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

            private UserGUI userGUI;

            private Analysis analysis = null;

            internal ParsingAction(ParserMode mode, UserGUI userGUI)
            {
                this.mode = mode;
                this.userGUI = userGUI;
            }

            /// <summary>
            /// In analyze mode, initiates a paired analysis instance and flushes the GUI code object.
            /// In generate mode, does nothing.
            /// </summary>
            /// <param name="asset"></param>
            internal void ActOnEnter(InputActionAsset asset)
            {
                switch (mode)
                {
                    case ParserMode.Analyze:
                        this.analysis = new Analysis(asset);
                        this.userGUI.AnalysisDisplay.Clear();
                        break;

                    case ParserMode.Generate:
                        throw new NotImplementedException();
                    //break;

                    default:
                        throw new Exception($"Unknown parser mode: {mode}");
                }

            }

            /// <summary>
            /// In analyze mode, present the map to the GUI.
            /// In generate mode, execute the prefab creator for action maps.
            /// </summary>
            /// <param name="map"></param>
            internal void ActOnEnter(InputActionMap map)
            {
                switch (mode)
                {
                    case ParserMode.Analyze:
                        this.userGUI.AnalysisDisplay.Add(this.analysis.AnalyzeMapOnEnter(map));
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
            internal void ActOnEnter(InputAction action)
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

            internal void ActOnExit(InputActionAsset asset)
            {
                //throw new NotImplementedException();
            }

            internal void ActOnExit(InputActionMap map)
            {
                switch (mode)
                {
                    case ParserMode.Analyze:
                        this.userGUI.AnalysisDisplay.Add(this.analysis.AnalyzeMapOnExit(map));
                        break;

                    case ParserMode.Generate:
                        break;

                    default:
                        break;
                }
            }

            internal void ActOnExit(InputAction action)
            {
                //throw new NotImplementedException();
            }
        }
    }

}
