using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PrefabCreation : Editor
{
    private const string AssetPath = "Assets/Prefabs/Main Camera.prefab";

    [MenuItem("Test/Prefab variant test")]
    static void Main()
    {
        var modelRootGO = (GameObject)AssetDatabase.LoadMainAssetAtPath(AssetPath);

        UnityEngine.SceneManagement.Scene destinationScene = EditorSceneManager.NewPreviewScene();
        var instanceRoot = PrefabUtility.InstantiatePrefab(modelRootGO, destinationScene: destinationScene);
        var variantRoot = PrefabUtility.SaveAsPrefabAsset((GameObject)instanceRoot, "Assets/Prefabs/MyModel_Variant.prefab");
        
        EditorSceneManager.ClosePreviewScene(destinationScene);
    }
}
