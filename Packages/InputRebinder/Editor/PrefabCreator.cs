using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using InputRebinder.Runtime;
using System.Text;

namespace InputRebinder
{
    internal class PrefabCreator
    {
        /// <summary>
        /// Reference to the analysis object
        /// </summary>
        private Analysis analysis;

        /// <summary>
        /// Canvas script
        /// </summary>
        private InputRebinderCanvas canvas = default;
        
        /// <summary>
        /// Name and path of the new prefab asset
        /// </summary>
        private string newPrefabAsset;

        /// <summary>
        /// Path to the input rebinder canvas prefab in the package
        /// </summary>
        private const string pathToCanvasPrefab = "Packages/com.songyang.inputrebinder/Runtime/Prefabs/Input Rebinder Canvas.prefab";

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="analysis">Analysis results</param>
        internal PrefabCreator(Analysis analysis, string newPrefab)
        {
            this.analysis = analysis ?? throw new ArgumentNullException(nameof(analysis));
            this.newPrefabAsset = newPrefab;

            // load prefabs
            var canvas = AssetDatabase.LoadAssetAtPath<GameObject>(pathToCanvasPrefab);
            this.canvas = canvas.GetComponent<InputRebinderCanvas>() ?? throw new ArgumentNullException(nameof(canvas));
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