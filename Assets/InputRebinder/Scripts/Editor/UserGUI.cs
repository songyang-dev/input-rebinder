﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

/// <summary>
/// Namespace for the input rebinder plugin
/// </summary>
namespace InputRebinder
{

    /// <summary>
    /// Script controlling the user GUI
    /// </summary>
    internal class UserGUI : EditorWindow
    {
        /// <summary>
        /// Reference to the parser script
        /// </summary>
        private Parser parser;

        /// <summary>
        /// Input action asset to process
        /// </summary>
        private InputActionAsset asset;

        /// <summary>
        /// Path for the generated prefab 
        /// </summary>
        private string path;

        /// <summary>
        /// Name of the generated prefab
        /// </summary>
        private string prefabName;

        /// <summary>
        /// Whether the asset was analyzed
        /// </summary>
        internal bool HasAnalyzed = false;

        /// <summary>
        /// GUI code in linear order or execution
        /// </summary>
        internal List<Action> AnalysisDisplay = new List<Action>();

        /// <summary>
        /// Instantiate a parser
        /// </summary>
        private void OnEnable()
        {
            this.parser = new Parser(Parser.ParserMode.Analyze, this);
        }

        /// <summary>
        /// Makes the plugin show up in unity menu
        /// </summary>
        [MenuItem("Window/Input Rebinder")]
        internal static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(UserGUI));
        }

        /// <summary>
        /// Populate the GUI window
        /// </summary>
        void OnGUI()
        {
            // things above the button
            ShowPreamble();

            // make a button
            DisplayButton();

            // analysis results
            if (HasAnalyzed) ShowAnalysis();
        }

        private void ShowAnalysis()
        {
            // section title
            GUILayout.Label("Analysis Results", EditorStyles.boldLabel);

            // the rest is shown after analysis
            foreach (var item in this.AnalysisDisplay)
            {
                item();
            }
        }

        // Information that is always displayed
        #region Static information
        /// <summary>
        /// Creates a button depending on the context
        /// </summary>
        private void DisplayButton()
        {
            GUILayout.BeginHorizontal();
            using (new EditorGUI.DisabledGroupScope(asset == null))
            {
                if (GUILayout.Button("Analyze")) ClickAnalyze();
            }

            // Disable the generation button if no analysis was done
            using (new EditorGUI.DisabledScope(HasAnalyzed == false))
            {
                if (GUILayout.Button("Generate")) ClickGenerate();
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// When 'generate' is clicked
        /// </summary>
        private void ClickGenerate()
        {
            parser.Mode = Parser.ParserMode.Generate;
            parser.Parse(asset);
        }

        /// <summary>
        /// When 'analyze' is clicked
        /// </summary>
        private void ClickAnalyze()
        {
            parser.Mode = Parser.ParserMode.Analyze;
            parser.Parse(asset);

        }



        /// <summary>
        /// GUI to display before the generate button
        /// </summary>
        private void ShowPreamble()
        {
            titleContent = new GUIContent("Input Rebinder"); // tab title

            // label
            GUILayout.Label("Input Action Asset", EditorStyles.boldLabel);

            // asset reference field
            GUIContent assetTooltip = new GUIContent(".inputactions file", "Unity's new input system asset file that contains your control bindings");
            asset = EditorGUILayout.ObjectField(assetTooltip, asset, typeof(InputActionAsset), false)
                as InputActionAsset;

            // generated prefab location
            GUIContent locationTooltip = new GUIContent("Generated Prefab Location", "Where to store the generated prefab");
            path = EditorGUILayout.TextField(locationTooltip, "Assets/Prefabs");

            // generated prefab name
            GUIContent nameTooltip = new GUIContent("Generated Prefab Name", "Name of the generated prefab. Conflicting file names will be overwritten.");
            prefabName = EditorGUILayout.TextField(nameTooltip, "Input Rebinder Controls");
        }
        #endregion
    }

}