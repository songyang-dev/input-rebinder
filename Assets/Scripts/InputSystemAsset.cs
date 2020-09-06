using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class InputSystemAsset : Editor
{
    [MenuItem("Test/Reference equality test")]
    static void Main()
    {
        var path = "Assets/Controls/controls.inputactions";
        InputActionAsset actions = AssetDatabase.LoadAssetAtPath<InputActionAsset>(path);

        foreach (var map in actions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                foreach (var bind in action.bindings)
                {
                    Debug.Log(bind);
                }
            }
        }
    }
}
