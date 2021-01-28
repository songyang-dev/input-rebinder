using UnityEngine;
using UnityEditor;
using System;
using InputRebinder.Runtime;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.IO;

namespace InputRebinder.Editor
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
        /// A map button script attached to a prefab loaded from disk,
        /// not instantiated yet
        /// </summary>
        private ActionMapButton mapButton;

        /// <summary>
        /// Scrollview for displaying map contents
        /// </summary>
        private ActionMapDisplayScroll mapDisplayScroll;

        /// <summary>
        /// A map content script attached to a prefab loaded from disk,
        /// not instantiated yet
        /// </summary>
        private ActionMapContent mapContent;

        /// <summary>
        /// Default map content instantiated by default on the template,
        /// must be deactivated when more map contents are added
        /// </summary>
        private ActionMapContent defaultMapContent;

        /// <summary>
        /// Component of the input rebinder's action prefab
        /// </summary>
        private InputRebinderAction inputRebinderAction;

        /// <summary>
        /// Component of the input rebinder's binding prefab
        /// </summary>
        private InputRebinderBinding inputRebinderBinding;

        /// <summary>
        /// Component of the input rebinder's binding pair prefab
        /// </summary>
        private BindingPair bindingPair;

        /// <summary>
        /// Reference to the input action asset on disk
        /// </summary>
        private InputActionAsset asset;

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
        /// Location of the prefab templates
        /// </summary>
        private string pathToPrefabs;

        /// <summary>
        /// Path to the generation template prefab
        /// </summary>
        private string pathToGenerationTemplate = "Input Rebinder Template.prefab";

        /// <summary>
        /// Path to the action map button prefab
        /// </summary>
        private string pathToActionMapButton = "Map Button.prefab";

        /// <summary>
        /// Path to the action map content prefab
        /// </summary>
        private string pathToActionMapContent = "Map Content.prefab";

        /// <summary>
        /// Path to the action prefab
        /// </summary>
        private string pathToAction = "Input Rebinder Action.prefab";

        /// <summary>
        /// Path to the binding prefab
        /// </summary>
        private string pathToBinding = "Input Rebinder Binding.prefab";

        /// <summary>
        /// Path to the binding pair prefab
        /// </summary>
        private string pathToBindingPair = "Input Rebinder Binding Pair.prefab";

        /// <summary>
        /// Finds the folder where the prefab templates are
        /// </summary>
        private void LocatePrefabs()
        {
            if (File.Exists("Packages/Input Rebinder/Runtime/Prefabs/"))
                this.pathToPrefabs = "Packages/Input Rebinder/Runtime/Prefabs/";
            else this.pathToPrefabs = "Assets/Input Rebinder/Runtime/Prefabs/";

            this.pathToAction = Path.Combine(this.pathToPrefabs, this.pathToAction);
            this.pathToActionMapButton = Path.Combine(this.pathToPrefabs, this.pathToActionMapButton);
            this.pathToActionMapContent = Path.Combine(this.pathToPrefabs, this.pathToActionMapContent);
            this.pathToBinding = Path.Combine(this.pathToPrefabs, this.pathToBinding);
            this.pathToBindingPair = Path.Combine(this.pathToPrefabs, this.pathToBindingPair);
            this.pathToGenerationTemplate = Path.Combine(this.pathToPrefabs, this.pathToGenerationTemplate);
        }

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
            LocatePrefabs();
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

            // map display scroll
            this.mapDisplayScroll = this.generatedPrefab.GetComponentInChildren<ActionMapDisplayScroll>();
            this.mapDisplayScroll.Contents.Clear();

            // map content prefab from disk
            this.mapContent =
                AssetDatabase.LoadAssetAtPath<GameObject>(pathToActionMapContent).GetComponent<ActionMapContent>();

            // map content prefab instance in the template by default
            this.defaultMapContent = this.generatedPrefab.GetComponentInChildren<ActionMapContent>();

            // input rebinder action prefab from disk
            this.inputRebinderAction = AssetDatabase.LoadAssetAtPath<GameObject>(pathToAction)
                .GetComponent<InputRebinderAction>();

            // input rebinder binding prefab from disk
            this.inputRebinderBinding = AssetDatabase.LoadAssetAtPath<GameObject>(pathToBinding)
                .GetComponent<InputRebinderBinding>();

            // input rebinder binding pair prefab from disk
            this.bindingPair = AssetDatabase.LoadAssetAtPath<GameObject>(pathToBindingPair)
                .GetComponent<BindingPair>();
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
        public bool ActOnEnter(InputActionAsset asset)
        {
            var info = UnityEditor.PackageManager.PackageInfo
                .FindForAssetPath(pathToGenerationTemplate);

            string display;

            if (info != null) 
                display = $"{info.displayName} {info.version}";
            else
                display = "Input Rebinder (custom version)";

            this.canvas.GetComponent<InputRebinderCanvas>()
                .PluginVersion
                .text
                = display;

            this.asset = asset;

            return true;
        }

        /// <summary>
        /// Adds the map buttons to the scroll view
        /// and create a map display content
        /// </summary>
        /// <param name="map"></param>
        public bool ActOnEnter(InputActionMap map)
        {
            if (AddMapButton(map) == false) return false;

            // Create a map display content for the larger scroll view

            var mapContentInstance = (PrefabUtility.InstantiatePrefab(
                this.mapContent.gameObject,
                this.mapDisplayScroll.Viewport.transform)
                as GameObject)
                .GetComponent<ActionMapContent>();

            AssignMapContent(map, mapContentInstance);

            return true;
        }

        /// <summary>
        /// Sets the map content prefab instance to respond 
        /// to the correct map button
        /// </summary>
        /// <param name="map"></param>
        /// <param name="mapContent"></param>
        private void AssignMapContent(InputActionMap map, ActionMapContent mapContent)
        {
            // set data on the map content script and object
            mapContent.Map = map;
            mapContent.name = map.name;

            // add the map content to the list of contents on the display scrollview script
            this.mapDisplayScroll.Contents.Add(mapContent);
        }

        /// <summary>
        /// Makes a map button on the scroll view
        /// </summary>
        /// <param name="map"></param>
        /// <returns>False means parsing must be interrupted</returns>
        private bool AddMapButton(InputActionMap map)
        {
            // ignore map if the analysis says so
            if (!this.analysis.maps[map]) return false;

            var mapCount = map.asset.actionMaps.Count;
            if (mapCount <= 0) return false;

            var newButton = PrefabUtility.InstantiatePrefab(this.mapButton.gameObject,
                this.scrollView.ButtonContainer.transform)
                as GameObject;

            // set names
            newButton.name = map.name;
            ActionMapButton actionMapButton = newButton.GetComponent<ActionMapButton>();
            actionMapButton.ButtonText = map.name;

            // set scrollview
            actionMapButton.scrollView = this.scrollView;

            // set map
            actionMapButton.Map = map;

            return true;
        }

        /// <summary>
        /// Makes a section in the map display screen for this action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool ActOnEnter(InputAction action)
        {
            // skip if not meant to generate
            if (this.analysis.actions[action] == false) return false;

            ActionMapContent actionMapContent = this.mapDisplayScroll.
                GetActionMapContent(action.actionMap);

            var actionInstance = (PrefabUtility.InstantiatePrefab(
                inputRebinderAction.gameObject,
                actionMapContent.gameObject.transform)
                as GameObject)
                .GetComponent<InputRebinderAction>();

            // set the input rebinder action
            actionInstance.ActionName.text = action.name;
            actionInstance.Action = action;

            // add action to the map content
            actionMapContent.Actions.Add(actionInstance);

            // link map content to the action
            actionInstance.BindingsParent = actionMapContent.gameObject;

            return true;
        }

        /// <summary>
        /// Processes the binding
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="action"></param>
        public void Act(InputBinding binding, InputAction action)
        {
            // ignore composite actions for now
            if (binding.isComposite) return;

            // get the input rebinder action, used to instantiate the binding prefab's location
            var relatedAction =
                this.mapDisplayScroll.GetActionMapContent(action.actionMap)
                .GetInputRebinderAction(action)
            ;

            // get the binding pair for presentation
            // or make a new one
            
            // no pair yet
            if (relatedAction.LastPair == null)
                relatedAction.LastPair = CreatePair(relatedAction);
            
            // not enough space
            if (relatedAction.LastPair.Count >= 2)
            {
                relatedAction.LastPair = CreatePair(relatedAction);
            }

            var bindingInstance = (PrefabUtility.InstantiatePrefab(
                this.inputRebinderBinding.gameObject,
                relatedAction.LastPair.transform
            ) as GameObject).GetComponent<InputRebinderBinding>();

            // increment pair's inner count
            relatedAction.LastPair.Count++;

            // link up the binding to the action
            relatedAction.InputRebinderBindings.Add(bindingInstance);

            // set the name
            bindingInstance.CurrentBindingText.text = 
                InputControlPath.ToHumanReadableString(binding.effectivePath);

            // add refs to the binding
            bindingInstance.OriginalBinding = binding;
            bindingInstance.Asset = this.asset;
            bindingInstance.BindingIndex = action.GetBindingIndex(binding);
            bindingInstance.MapName = action.actionMap.name;
            bindingInstance.ActionName = action.name;
        }

        private BindingPair CreatePair(InputRebinderAction action)
        {
            return (PrefabUtility.InstantiatePrefab(this.bindingPair.gameObject,
                    action.BindingsParent.transform
            ) as GameObject).GetComponent<BindingPair>();
        }

        public void ActOnExit(InputActionAsset asset)
        {
            // activate one map content only
            this.mapDisplayScroll.ActivateFirstMapContent();

            // saving
            PrefabSaveAndCleanUp();
        }

        public void ActOnExit(InputActionMap map)
        {
            // disable the default map content in the prefab
            this.defaultMapContent.gameObject.SetActive(false);
        }

        public void ActOnExit(InputAction action)
        {
            //throw new NotImplementedException();
        }
    }
}