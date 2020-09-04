using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using InputRebinder.Runtime;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace InputRebinder
{
    internal class PrefabCreator : IParsingAction
    {
        /// <summary>
        /// Reference to the analysis object
        /// </summary>
        private Analysis analysis;

        /// <summary>
        /// Preview scene for creating the prefab
        /// </summary>
        private Scene previewScene;

        #region Generation prefab instance objects
        /// <summary>
        /// Generation template prefab instance, will become a prefab variant
        /// </summary>
        private GameObject generatedPrefab;

        /// <summary>
        /// Input Rebinder Canvas script, located at the prefab root
        /// </summary>
        private InputRebinderCanvas canvas;

        /// <summary>
        /// Scroll view of the action map buttons
        /// </summary>
        private ActionMapScroll scrollView;

        /// <summary>
        /// A map button script attached to an asset loaded from disk,
        /// not instantiated yet
        /// </summary>
        private ActionMapButton mapButton;
        #endregion

        /// <summary>
        /// Path to the folder of the new prefab asset
        /// </summary>
        private readonly string newPrefabFolder;

        /// <summary>
        /// Name of the new prefab
        /// </summary>
        private readonly string newPrefabName;

        /// <summary>
        /// Path to the generation template prefab
        /// </summary>
        private const string pathToGenerationTemplate = "Packages/com.songyang.inputrebinder/Runtime/Prefabs/Input Rebinder Template.prefab";

        /// <summary>
        /// Path to the action map button prefab
        /// </summary>
        private const string pathToActionMapButton = "Packages/com.songyang.inputrebinder/Runtime/Prefabs/Map Button.prefab";

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="analysis">Analysis results</param>
        /// <param name="newPrefabFolder">Path of the folder for the new prefab</param>
        /// <param name="newPrefabName">Name of the new prefab</param>
        internal PrefabCreator(Analysis analysis, string newPrefabFolder, string newPrefabName)
        {
            this.analysis = analysis ?? throw new ArgumentNullException(nameof(analysis));
            this.newPrefabFolder = newPrefabFolder;
            this.newPrefabName = newPrefabName;

            // create folders
            CreateFolders(newPrefabFolder);

            // load prefabs
            LoadPrefabs();
        }

        /// <summary>
        /// Saves the instantiated prefab as a completely new prefab
        /// and cleans up the preview scene
        /// </summary>
        private void PrefabSaveAndCleanUp()
        {
            // save prefab and clean up the editing environment
            var newPrefab = $"{this.newPrefabFolder}/{this.newPrefabName}.prefab";
            bool success;
            PrefabUtility.SaveAsPrefabAsset(this.generatedPrefab, newPrefab, out success);
            if (!success) throw new Exception($"New prefab did not generate at {this.newPrefabFolder}");

            EditorSceneManager.ClosePreviewScene(previewScene);
        }

        /// <summary>
        /// Prepares the prefabs for editing
        /// </summary>
        private void LoadPrefabs()
        {
            this.previewScene = EditorSceneManager.NewPreviewScene();

            // generation template
            var rootGameObject = AssetDatabase.LoadMainAssetAtPath(pathToGenerationTemplate);
            this.generatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(rootGameObject, previewScene);

            // canvas
            this.canvas = this.generatedPrefab.GetComponentInChildren<InputRebinderCanvas>();

            // scroll view
            this.scrollView = this.generatedPrefab.GetComponentInChildren<ActionMapScroll>();

            // map button
            this.mapButton = AssetDatabase.LoadAssetAtPath<GameObject>(pathToActionMapButton)
                .GetComponent<ActionMapButton>();
        }

        /// <summary>
        /// Creates all the folders leading to the given folder path
        /// </summary>
        /// <param name="path">Folder to create</param>
        internal static void CreateFolders(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;

            char separator = '/';
            string[] directories = path.Split(separator);
            if (!directories[0].Equals("Assets"))
                throw new Exception("The path for the generated prefab must start with 'Assets'");

            // loop on the directories, creating folders on the way when needed
            StringBuilder pathInProgress = new StringBuilder("Assets");
            bool skipFirst = true;
            foreach (var dir in directories)
            {
                // assume the Assets folder is created
                if (skipFirst)
                {
                    skipFirst = false;
                    continue;
                }

                // folder that would be created if it didn't exist yet
                string createdFolder = $"{pathInProgress.ToString()}/{dir}";

                // create folder if it doesn't exist
                if (!AssetDatabase.IsValidFolder(createdFolder))
                    AssetDatabase.CreateFolder(pathInProgress.ToString(), dir);

                // append created folder
                pathInProgress.Append($"/{dir}");
            }
        }

        /// <summary>
        /// Sets the version number of the package on the generated prefab
        /// </summary>
        /// <param name="asset"></param>
        public void ActOnEnter(InputActionAsset asset)
        {
            var info = UnityEditor.PackageManager.PackageInfo
                .FindForAssetPath(pathToGenerationTemplate);
            var display = $"{info.displayName} {info.version}";

            this.canvas.GetComponent<InputRebinderCanvas>()
                .PluginVersion
                .text
                = display;
        }

        /// <summary>
        /// Adds the map buttons to the scroll view
        /// </summary>
        /// <param name="map"></param>
        public void ActOnEnter(InputActionMap map)
        {
            var mapCount = map.asset.actionMaps.Count;
            if (mapCount <= 0) return;

            var newButton = PrefabUtility.InstantiatePrefab(this.mapButton.gameObject,
                this.scrollView.ButtonContainer.transform)
                as GameObject;

            // set names
            newButton.name = map.name;
            ActionMapButton actionMapButton = newButton.GetComponent<ActionMapButton>();
            actionMapButton.ButtonText = map.name;

            // set scrollview
            actionMapButton.scrollView = this.scrollView;
        }

        public void ActOnEnter(InputAction action)
        {
            //throw new NotImplementedException();
        }

        public void Act(InputBinding b)
        {
            //throw new NotImplementedException();
        }

        public void ActOnExit(InputActionAsset asset)
        {
            PrefabSaveAndCleanUp();
        }

        public void ActOnExit(InputActionMap map)
        {
            //throw new NotImplementedException();
        }

        public void ActOnExit(InputAction action)
        {
            //throw new NotImplementedException();
        }
    }
}