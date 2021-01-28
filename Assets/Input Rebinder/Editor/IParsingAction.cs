using UnityEngine.InputSystem;
using System;
using UnityEditor;
using System.IO;

/// <summary>
/// Contains editor code for generating the prefabs from an inputaction file
/// </summary>
namespace InputRebinder.Editor
{
    /// <summary>
    /// Class that acts on the input action parsed data
    /// </summary>
    internal interface IParsingAction
    {
        /// <summary>
        /// In analyze mode, initiates a paired analysis instance and flushes the GUI code object.
        /// In generate mode, does nothing.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns>Whether to continue parsing or not</returns>
        bool ActOnEnter(InputActionAsset asset);

        /// <summary>
        /// In analyze mode, present the map to the GUI.
        /// In generate mode, execute the prefab creator for action maps.
        /// </summary>
        /// <param name="map"></param>
        /// <returns>Whether to continue parsing or not</returns>
        bool ActOnEnter(InputActionMap map);

        /// <summary>
        /// In analyze mode, present the action to the GUI.
        /// In generate mode, execute the prefab creator for actions.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>Whether to continue parsing or not</returns>
        bool ActOnEnter(InputAction action);

        /// <summary>
        /// In analyze mode, ask what kind of bindings should be excluded.
        /// In generate mode, execute the prefab creator for bindings.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="action">Action the binding belongs to</param>
        void Act(InputBinding b, InputAction action);

        void ActOnExit(InputActionAsset asset);

        void ActOnExit(InputActionMap map);

        void ActOnExit(InputAction action);
    }
}

