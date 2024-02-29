using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace AvatarStudio
{
    public class BuildEditor
    {
        [MenuItem("Assets/Asset Build", false, 0)]
        static public void OnAssets()
        {
            SetUp();

            if (Selection.objects.Length > 0)
            {
                if (Selection.objects[0] is GameObject obj)
                {
                    AssetBuilder.Build(obj);
                    return;
                }
                else if (Selection.objects[0] is SceneAsset scene)
                {
                    SceneBuilder.Build(scene);
                    return;
                }
            }

            Debug.LogError("[KeyakiStudioKit] Not Selected.");
        }

        static void SetUp()
        {

            var path = Application.dataPath + "/KeyakiStudioKit";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = path + "/AssetBundles";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }

    #region -- Asset Build --

    public class AssetBuilder
    {
        static public void Build(GameObject obj)
        {
            var inputPath = AssetDatabase.GetAssetOrScenePath(obj);

            // Asset Prefab
            var ap = obj.GetComponent<AssetPrefab>();
            if (ap == null)
                ap = obj.AddComponent<AssetPrefab>();

            // Property Window
            var proptyModalWindow = ScriptableObject.CreateInstance<AssetBuildPropertyEditor>();
            proptyModalWindow.Show(inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(ap.gameObject);

                Directory.CreateDirectory(outputPath);

                AssetBuild(inputPath, outputPath, assetId);
            };
        }

        static void AssetBuild(string inputPath, string outputPath, string assetId)
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

            var androidPath = outputPath + "/Android";
            Directory.CreateDirectory(androidPath);
            BuildPipeline.BuildAssetBundles(androidPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);

            var osxPath = outputPath + "/StandaloneOSX";
            Directory.CreateDirectory(osxPath);
            BuildPipeline.BuildAssetBundles(osxPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);

            var winPath = outputPath + "/StandaloneWindows64";
            Directory.CreateDirectory(winPath);
            BuildPipeline.BuildAssetBundles(winPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
        }

        class AssetBuildPropertyEditor : EditorWindow
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

                _outputPath = Application.dataPath + "/KeyakiStudioKit/AssetBundles";
            }

            void OnGUI()
            {
                var titleTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/KeyakiStudioKit/Editor/title.png");
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

    #endregion

    #region -- Scene Build --

    public class SceneBuilder
    {
        static public void Build(SceneAsset scene)
        {
            var inputPath = AssetDatabase.GetAssetOrScenePath(scene);

            // Property Window
            var proptyModalWindow = ScriptableObject.CreateInstance<SceneBuildPropertyEditor>();
            proptyModalWindow.Show(inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(scene);

                Directory.CreateDirectory(outputPath);

                SceneBuild(inputPath, outputPath, assetId);
            };
        }

        static void SceneBuild(string inputPath, string outputPath, string assetId)
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

            var androidPath = outputPath + "/Android";
            Directory.CreateDirectory(androidPath);
            BuildPipeline.BuildAssetBundles(androidPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);

            var osxPath = outputPath + "/StandaloneOSX";
            Directory.CreateDirectory(osxPath);
            BuildPipeline.BuildAssetBundles(osxPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);

            var winPath = outputPath + "/StandaloneWindows64";
            Directory.CreateDirectory(winPath);
            BuildPipeline.BuildAssetBundles(winPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
        }

        class SceneBuildPropertyEditor : EditorWindow
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
                titleContent = new GUIContent("Asset Build (Scene)");

                minSize = new Vector2(320, 390);
                position = new Rect(Vector2.zero, minSize);

                _outputPath = Application.dataPath + "/KeyakiStudioKit/AssetBundles";
            }

            void OnGUI()
            {
                var titleTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/KeyakiStudioKit/Editor/title.png");
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

    #endregion
}