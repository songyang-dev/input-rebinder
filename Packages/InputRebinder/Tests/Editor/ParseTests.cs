using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using InputRebinder;

namespace Tests
{
    public class ParseTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void Empty()
        {
            // Use the Assert class to test conditions
            
            // get parser
            UserGUI.ShowWindow();
            var window = UserGUI.window;
            var parser = new Parser(Parser.ParserMode.Analyze, window);

            // pass in the asset
            InputActionAsset asset = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Packages/com.songyang.inputrebinder/Tests/Editor/Test Assets/Empty.inputactions");
            Assert.NotNull(asset, "Asset not found");
            parser.Parse(asset);
        }
    }
}
