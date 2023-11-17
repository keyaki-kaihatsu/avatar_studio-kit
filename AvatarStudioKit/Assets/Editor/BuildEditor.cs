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

            Build();
        }

        [MenuItem("GameObject/Asset Build", false, 0)]
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

            path = path + "/iOS";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        static void Build()
        {
            var gameObject = Selection.objects[0] as GameObject;
            var inputPath = AssetDatabase.GetAssetOrScenePath(gameObject);

            // Asset Prefab
            var assetType = AssetType.Scene;
            var ap = gameObject.GetComponent<AssetPrefab>();
            if (ap == null)
                ap = gameObject.AddComponent<AssetPrefab>();
            else
                assetType = ap.type;

            // Property Window
            var proptyModalWindow = ScriptableObject.CreateInstance<BuildPropertyEditor>();
            proptyModalWindow.Show(inputPath, assetType);

            proptyModalWindow._completion = (outputPath, assetId, type) =>
            {
                ap.type = type;

                // Save
                EditorUtility.SetDirty(ap.gameObject);

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

            BuildPipeline.BuildAssetBundles(outputPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
        }

        public class BuildPropertyEditor : EditorWindow
        {
            public Action<string, string, AssetType> _completion;

            public string _outputPath;

            public string _assetId = "";

            public AssetType _type;

            public void Show(string inputPath, AssetType type)
            {
                _assetId = Path.GetFileNameWithoutExtension(inputPath);
                _type = type;

                ShowUtility();
            }

            void Awake()
            {
                titleContent = new GUIContent("Properties");

                minSize = new Vector2(320, 500);
                position = new Rect(Vector2.zero, minSize);

                _outputPath = Application.dataPath + "/AssetBundles/iOS";
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

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField("#3 Please select type.", EditorStyles.wordWrappedLabel);

                    EditorGUILayout.LabelField("Grip: hold it in the avatar's hand", EditorStyles.wordWrappedLabel);

                    EditorGUILayout.LabelField("Scene: 3D Scene.", EditorStyles.wordWrappedLabel);
                }
                EditorGUILayout.EndVertical();

                _type = (AssetType)EditorGUILayout.EnumPopup(_type);

                GUILayout.Space(20);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.Space();
                    if (GUILayout.Button("Build", GUILayout.MaxWidth(200f)))
                    {
                        Close();
                        _completion?.Invoke(_outputPath + "/" + _assetId, _assetId, _type);
                    }
                    EditorGUILayout.Space();
                }
            }
        }
    }
}