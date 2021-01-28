using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        /// Reference to the input system component in Unity
        /// </summary>
        public InputActionAsset Asset;

        private InputActionReference _action;

        /// <summary>
        /// TMPro component of the text of the current binding
        /// </summary>
        public TMPro.TextMeshProUGUI CurrentBindingText;

        /// <summary>
        /// TMPro component of the text of the rebind button
        /// </summary>
        [SerializeField] private TMPro.TextMeshProUGUI ButtonText;

        /// <summary>
        /// Reference to the button
        /// </summary>
        [SerializeField] private Button rebindButton;
        
        /// <summary>
        /// Reference to the reset button
        /// </summary>
        [SerializeField] private Button resetButton;

        /// <summary>
        /// Index of this binding in the action's bindings array
        /// </summary>
        public int BindingIndex;

        /// <summary>
        /// Name of the input action map that this binding is in
        /// </summary>
        public string MapName;

        /// <summary>
        /// Name of the input action that this binding is in
        /// </summary>
        public string ActionName;

        /// <summary>
        /// Reference to the input action, will search if not set
        /// </summary>
        /// <value></value>
        public InputActionReference Action
        {
            get
            {
                if (_action == null)
                {
                    GetAction(); return this._action;
                }
                return _action;
            }
            set => _action = value;
        }

        /// <summary>
        /// Locates the action inside the input action asset
        /// </summary>
        private void GetAction()
        {
            this._action = InputActionReference.Create(
                this.Asset.FindActionMap(this.MapName).FindAction(this.ActionName)
            );
        }

        /// <summary>
        /// Event listener of when the rebind button is clicked
        /// </summary>
        public void ClickRebind()
        {
            // change the text of the rebind button
            this.ButtonText.text = "Listening...";

            // disable buttons
            this.rebindButton.enabled = false;
            this.resetButton.enabled = false;

            InitiateRebindOperation(Action);
        }

        /// <summary>
        /// Remaps the input binding using Unity's own extension.
        /// Code comes from https://docs.unity3d.com/Packages/com.unity.inputsystem@1.1/manual/HowDoI.html#create-a-ui-to-rebind-input-in-my-game
        /// </summary>
        /// <param name="actionToRebind"></param>
        internal void InitiateRebindOperation(InputAction actionToRebind)
        {
            actionToRebind.Disable();
            var rebindOperation = actionToRebind.PerformInteractiveRebinding(this.BindingIndex)                

                // To avoid accidental input from mouse motion
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)

                // timeout
                .WithTimeout(2f)

                // cancel through
                .WithCancelingThrough("<Keyboard>/escape")

                // Dispose the operation on completion.
                .OnComplete(operation => 
                {
                    operation.Dispose(); 
                    actionToRebind.Enable();
                    ResetTextAndButtons();
                })
                .OnCancel(operation => 
                {
                    operation.Dispose(); 
                    actionToRebind.Enable();
                    ResetTextAndButtons();
                })
                .Start();
        }

        /// <summary>
        /// Sets the UI back to ready state
        /// </summary>
        private void ResetTextAndButtons()
        {
            this.ButtonText.text = "Rebind";
            this.CurrentBindingText.text =
                this.Action.action.bindings[this.BindingIndex].ToDisplayString(InputBinding.DisplayStringOptions.DontOmitDevice);
            this.rebindButton.enabled = true;
            this.resetButton.enabled = true;
        }

        /// <summary>
        /// Event listener of when the reset button is clicked
        /// </summary>
        public void ClickReset()
        {
            this.Action.action.RemoveBindingOverride(this.BindingIndex);
            ResetTextAndButtons();
        }
    }
}