using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using InputRebinder.Runtime;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("com.songyang.inputrebinder.Editor.Tests")]

/// <summary>
/// Namespace for the input rebinder plugin
/// </summary>
namespace InputRebinder.Editor
{

    /// <summary>
    /// Script controlling the user GUI
    /// </summary>
    internal class UserGUI : EditorWindow
    {
        /// <summary>
        /// Reference to the active window, if any
        /// </summary>
        internal static UserGUI window;

        /// <summary>
        /// Reference to the parser script
        /// </summary>
        private Parser parser;

        /// <summary>
        /// Input action asset to process
        /// </summary>
        private InputActionAsset _asset;

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
        private Analysis analysis = null;

        /// <summary>
        /// For allowing the analysis part to to scroll
        /// </summary>
        private Vector2 scrollPosAnalysis = default;

        private InputActionAsset asset
        {
            get => _asset;
            set
            {
                // remove previous analysis
                if (_asset != value) this.analysis = null;

                _asset = value;
            }
        }

        /// <summary>
        /// Makes the plugin show up in unity menu
        /// </summary>
        [MenuItem("Tools/Input Rebinder")]
        internal static void ShowWindow()
        {
            window = EditorWindow.GetWindow(typeof(UserGUI)) as UserGUI;
        }

        /// <summary>
        /// Populate the GUI window
        /// </summary>
        void OnGUI()
        {

            // things above the buttons
            ShowPreamble();

            // make the generation and analysis buttons
            DisplayButton();

            using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosAnalysis))
            {
                scrollPosAnalysis = scrollView.scrollPosition;
                // analysis results
                if (analysis != null) ShowAnalysis();
            }
        }

        private void ShowAnalysis()
        {
            // section title
            GUILayout.Label("Analysis Results", EditorStyles.boldLabel);

            // the rest is shown after analysis
            foreach (var item in this.analysis.Results)
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
                if (GUILayout.Button(analysis != null ? "Re-analyze" : "Analyze"))
                    ClickAnalyze();
            }

            // Disable the generation button if no analysis was done
            using (new EditorGUI.DisabledScope(analysis == null))
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
            this.parser = new Parser(new PrefabCreator(this.analysis, this.path, this.prefabName));
            parser.Parse(asset);
        }

        /// <summary>
        /// When 'analyze' is clicked
        /// </summary>
        private void ClickAnalyze()
        {
            this.analysis = new Analysis();
            this.parser = new Parser(analysis);
            parser.Parse(asset);
        }



        /// <summary>
        /// GUI to display before the generate button
        /// </summary>
        private void ShowPreamble()
        {
            titleContent = new GUIContent("Input Rebinder"); // tab title

            // label
            EditorGUILayout.LabelField("Input Action Asset", EditorStyles.boldLabel);

            // asset reference field
            GUIContent assetTooltip = new GUIContent(".inputactions file", "Unity's new input system asset file that contains your control bindings");
            asset = EditorGUILayout.ObjectField(assetTooltip, asset, typeof(InputActionAsset), false)
                as InputActionAsset;

            // generated prefab location
            GUIContent locationTooltip = new GUIContent("Generated Prefab Folder", "Where to store the generated prefab");
            path = EditorGUILayout.TextField(locationTooltip, string.IsNullOrEmpty(path) ? "Assets/Prefabs": path);

            // generated prefab name
            GUIContent nameTooltip = new GUIContent("Generated Prefab Name", "Name of the generated prefab. Conflicting file names will be overwritten.");
            prefabName = EditorGUILayout.TextField(nameTooltip, string.IsNullOrEmpty(prefabName) ? "Input Rebinder Controls" : prefabName);
        }
        #endregion
    }

}