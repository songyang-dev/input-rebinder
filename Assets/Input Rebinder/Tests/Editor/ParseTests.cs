using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using InputRebinder.Editor;

using System.IO;

namespace Tests
{
    public class ParseTests
    {
        UserGUI window;
        Parser parser;

        string TestAssetFolder = "Tests/Editor/Test Assets/";

        // A Test behaves as an ordinary method
        [Test]
        public void Empty()
        {
            ParseAsset("Empty.inputactions");
        }

        [Test]
        public void One()
        {
            // pass in the asset
            ParseAsset("One.inputactions");
        }

        [Test]
        public void Default()
        {
            // pass in the asset
            ParseAsset("Default.inputactions");
        }

        private void ParseAsset(string name)
        {
            var location = Path.Combine(FindFolderLocation(), TestAssetFolder);
            // pass in the asset
            var path = Path.Combine(location, name);

            InputActionAsset asset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(path);
            Assert.NotNull(asset, "Asset not found");
            parser.Parse(asset);
        }

        private string FindFolderLocation()
        {
            if (File.Exists("Packages/Input Rebinder/")) return "Packages/Input Rebinder/";
            else return "Assets/Input Rebinder/";
        }


        [SetUp]
        public void GetParser()
        {
            parser = new Parser(new Analysis());
        }
    }
}
