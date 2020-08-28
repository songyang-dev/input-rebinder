using UnityEngine.InputSystem;
using System;

/// <summary>
/// Contains editor code for generating the prefabs from an inputaction file
/// </summary>
namespace InputRebinder
{

    internal partial class Parser
    {
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
                switch (mode)
                {
                    case ParserMode.Analyze:
                        this.userGUI.AnalysisDisplay.Add(this.analysis.AnalyzeActionOnEnter(action));
                        break;

                    case ParserMode.Generate:
                        break;

                    default:
                        throw new Exception($"Unknown parser mode: {mode}");
                }
            }

            /// <summary>
            /// In analyze mode, ask what kind of bindings should be excluded.
            /// In generate mode, execute the prefab creator for bindings.
            /// </summary>
            /// <param name="b"></param>
            internal void Act(InputBinding b)
            {
            }

            internal void ActOnExit(InputActionAsset asset)
            {
                switch (mode)
                {
                    case ParserMode.Analyze:
                        break;

                    case ParserMode.Generate:
                        break;

                    default:
                        break;
                }
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
                switch (mode)
                {
                    case ParserMode.Analyze:
                        this.userGUI.AnalysisDisplay.Add(this.analysis.AnalyzeActionOnExit(action));
                        break;

                    case ParserMode.Generate:
                        break;

                    default:
                        break;
                }
            }
        }
    }

}
