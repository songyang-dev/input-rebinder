using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// Namespace for the input rebinder plugin
/// </summary>
namespace InputRebinder
{

    /// <summary>
    /// Script controlling the user GUI
    /// </summary>
    public class UserGUI : EditorWindow
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
        public bool HasAnalyzed = false;

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
        public static void ShowWindow()
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
            
            GUILayout.Label("Analysis Results", EditorStyles.boldLabel);
        }

        /// <summary>
        /// Creates a button depending on the context
        /// </summary>
        private void DisplayButton()
        {
            string buttonText = HasAnalyzed ? "Generate Prefab" : "Analyze Asset";

            // on click
            if (GUILayout.Button(buttonText))
                ClickButton();
        }

        /// <summary>
        /// When the button is clicked
        /// </summary>
        private void ClickButton()
        {
            // asset is not null
            if (asset != null)
            {
                // analysis is required before generation
                if (HasAnalyzed)
                    parser.Mode = Parser.ParserMode.Generate;
                else
                    parser.Mode = Parser.ParserMode.Analyze;
                parser.Parse(asset);
            }
            // null asset
            else
                Debug.LogError("No input action asset has been chosen. Select an .inputactions file.");
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
    }

}