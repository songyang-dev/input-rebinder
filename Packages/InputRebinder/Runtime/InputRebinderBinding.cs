using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputRebinder.Runtime
{
    /// <summary>
    /// Component of a input system binding
    /// </summary>
    public class InputRebinderBinding : MonoBehaviour
    {
        /// <summary>
        /// Reference to the binding in Unity
        /// </summary>
        [HideInInspector]
        public InputBinding OriginalBinding;

        /// <summary>
        /// Reference to the action in Unity
        /// </summary>
        [HideInInspector]
        public InputAction Action;

        /// <summary>
        /// TMPro component of the text of the current binding
        /// </summary>
        public TMPro.TextMeshProUGUI CurrentBindingText;

        /// <summary>
        /// TMPro component of the text of the rebind button
        /// </summary>
        public TMPro.TextMeshProUGUI ButtonText;

        /// <summary>
        /// Event when click the rebind button
        /// </summary>
        public void ClickRebind()
        {
            RemapButtonClicked(this.Action);

            // change the text of the rebind button
            this.ButtonText.text = "Listening...";
        }

        /// <summary>
        /// Remaps the input binding using Unity's own extension.
        /// Code comes from https://docs.unity3d.com/Packages/com.unity.inputsystem@1.1/manual/HowDoI.html#create-a-ui-to-rebind-input-in-my-game
        /// </summary>
        /// <param name="actionToRebind"></param>
        internal void RemapButtonClicked(InputAction actionToRebind)
        {
            var rebindOperation = actionToRebind.PerformInteractiveRebinding()
                        // To avoid accidental input from mouse motion
                        .WithControlsExcluding("Mouse")
                        .OnMatchWaitForAnother(0.1f)
                        ;

            // Dispose the operation on completion.
            rebindOperation.OnComplete(
               operation =>
               {
                   operation.Dispose();
                   this.ButtonText.text = "Rebind";
               });

            rebindOperation.Start();
        }
    }
}