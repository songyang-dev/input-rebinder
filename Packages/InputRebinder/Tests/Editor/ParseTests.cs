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
        UserGUI window;
        Parser parser;

        // A Test behaves as an ordinary method
        [Test]
        public void Empty()
        {
            // pass in the asset
            ParseAsset("Packages/com.songyang.inputrebinder/Tests/Editor/Test Assets/Empty.inputactions");
        }

        [Test]
        public void One()
        {
            // pass in the asset
            ParseAsset("Packages/com.songyang.inputrebinder/Tests/Editor/Test Assets/One.inputactions");
        }

        [Test]
        public void Default()
        {
            // pass in the asset
            ParseAsset("Packages/com.songyang.inputrebinder/Tests/Editor/Test Assets/Default.inputactions");
        }

        private void ParseAsset(string path)
        {
            InputActionAsset asset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(path);
            Assert.NotNull(asset, "Asset not found");
            parser.Parse(asset);
        }

        [SetUp]
        public void GetParser()
        {
            parser = new Parser(new Analysis());
        }
    }
}
