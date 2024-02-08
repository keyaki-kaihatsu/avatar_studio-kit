using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace AvatarStudio
{
    public class SceneBuildEditor
    {
        [MenuItem("Assets/Scene Build", false, 0)]
        static public void OnAssets()
        {
            SetUp();

            Build();
        }

        [MenuItem("GameObject/Scene Build", false, 0)]
        static public void OnGameObject()
        {
            SetUp();

            Build();
        }

        static void SetUp()
        {
            var path = Application.dataPath + "/AssetBundles";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        static void Build()
        {
            var scene = Selection.objects[0] as SceneAsset;
            var inputPath = AssetDatabase.GetAssetOrScenePath(scene);

            // Property Window
            var proptyModalWindow = ScriptableObject.CreateInstance<BuildPropertyEditor>();
            proptyModalWindow.Show(inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(scene);

                Directory.CreateDirectory(outputPath);

                Build(inputPath, outputPath, assetId);
            };
        }

        static void Build(string inputPath, string outputPath, string assetId)
        {
            var builds = new List<AssetBundleBuild>();

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            var build = new AssetBundleBuild();
            build.assetBundleName = assetId.ToLower() + ".bundle";
            build.assetNames = new string[1] { inputPath };
            builds.Add(build);

            var iOSPath = outputPath + "/iOS";
            Directory.CreateDirectory(iOSPath);
            BuildPipeline.BuildAssetBundles(iOSPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);

            var osxPath = outputPath + "/StandaloneOSX";
            Directory.CreateDirectory(osxPath);
            BuildPipeline.BuildAssetBundles(osxPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);

            var winPath = outputPath + "/StandaloneWindows64";
            Directory.CreateDirectory(winPath);
            BuildPipeline.BuildAssetBundles(winPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
        }

        public class BuildPropertyEditor : EditorWindow
        {
            public Action<string, string> _completion;

            public string _outputPath;

            public string _assetId = "";

            public void Show(string inputPath)
            {
                _assetId = Path.GetFileNameWithoutExtension(inputPath);

                ShowUtility();
            }

            void Awake()
            {
                titleContent = new GUIContent("Asset Build");

                minSize = new Vector2(320, 390);
                position = new Rect(Vector2.zero, minSize);

                _outputPath = Application.dataPath + "/AssetBundles";
            }

            void OnGUI()
            {
                var titleTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/title.png");
                EditorGUILayout.LabelField(new GUIContent(titleTex), GUILayout.Height(320 * 1600f / 2400f), GUILayout.Width(320));

                EditorGUILayout.LabelField("#1 Please enter the output directory path.", EditorStyles.wordWrappedLabel);

                _outputPath = EditorGUILayout.TextField(_outputPath);

                GUILayout.Space(20);

                EditorGUILayout.LabelField("#2 Please enter asset name.", EditorStyles.wordWrappedLabel);

                _assetId = EditorGUILayout.TextField(_assetId);

                GUILayout.Space(20);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Build", GUILayout.MaxWidth(200f)))
                    {
                        Close();
                        _completion?.Invoke(_outputPath + "/" + _assetId, _assetId);
                    }
                    EditorGUILayout.Space();
                }
            }
        }
    }
}