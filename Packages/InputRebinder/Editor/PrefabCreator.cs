using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using InputRebinder.Runtime;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace InputRebinder
{
    internal class PrefabCreator
    {
        /// <summary>
        /// Reference to the analysis object
        /// </summary>
        private Analysis analysis;

        /// <summary>
        /// Preview scene for creating the prefab
        /// </summary>
        private Scene previewScene;

        /// <summary>
        /// Generation template prefab instance
        /// </summary>
        private GameObject generationTemplate;

        /// <summary>
        /// Input Rebinder Canvas prefab instance
        /// </summary>
        private GameObject canvas;
        
        /// <summary>
        /// Path to the folder of the new prefab asset
        /// </summary>
        private readonly string newPrefabFolder;

        /// <summary>
        /// Name of the new prefab
        /// </summary>
        private readonly string newPrefabName;

        /// <summary>
        /// Path to the input rebinder canvas prefab in the package
        /// </summary>
        private const string pathToCanvasPrefab = "Packages/com.songyang.inputrebinder/Runtime/Prefabs/Input Rebinder Canvas.prefab";
        
        /// <summary>
        /// Path to the generation template prefab
        /// </summary>
        private const string pathToGenerationTemplate = "Packages/com.songyang.inputrebinder/Runtime/Prefabs/Input Rebinder.prefab";

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

            // load prefabs
            LoadPrefabs();

            // create a new prefab by using a template
            // set up the generation in an isolated scene
            // then copy the generated prefab to the user's given location

            PrefabSetUp();
            PrefabSaveAndCleanUp();
        }

        /// <summary>
        /// Saves the instantiated prefab as a completely new prefab
        /// and cleans up the preview scene
        /// </summary>
        private void PrefabSaveAndCleanUp()
        {
            // save prefab and clean up the editing environment
            bool success;
            PrefabUtility.SaveAsPrefabAsset(this.generationTemplate, $"{this.newPrefabFolder}/{this.newPrefabName}.prefab", out success);
            EditorSceneManager.ClosePreviewScene(previewScene);

            if (!success) throw new Exception($"New prefab did not generate at {this.newPrefabFolder}");
        }

        /// <summary>
        /// Creates a preview scene and instantiates the template prefab there
        /// </summary>
        private void PrefabSetUp()
        {
            // set up prefab editing environment
            this.previewScene = EditorSceneManager.NewPreviewScene();
            this.generationTemplate = (GameObject)PrefabUtility.InstantiatePrefab(generationTemplate, previewScene);
        }

        private void LoadPrefabs()
        {
            // generation template
            this.generationTemplate = AssetDatabase.LoadAssetAtPath<GameObject>(pathToGenerationTemplate);

            // canvas
            this.canvas = AssetDatabase.LoadAssetAtPath<GameObject>(pathToCanvasPrefab);
            
        }

        /// <summary>
        /// Creates all the folders leading to the given folder path
        /// </summary>
        /// <param name="path">Folder to create</param>
        internal static void CreateFolders(string path)
        {
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
                if (skipFirst) {
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

        
    }
}