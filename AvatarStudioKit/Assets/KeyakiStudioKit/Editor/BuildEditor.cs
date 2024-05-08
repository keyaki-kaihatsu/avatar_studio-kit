using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            var proptyModalWindow = ScriptableObject.CreateInstance<BuildPropertyEditor>();
            proptyModalWindow.Show(inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(ap.gameObject);

                Directory.CreateDirectory(outputPath);

                AssetBuild(inputPath, outputPath, assetId);
            };
        }

        protected static void AssetBuild(string inputPath, string outputPath, string assetId)
        {
            var builds = new List<AssetBundleBuild>();

            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);

            var build = new AssetBundleBuild();
            build.assetBundleName = assetId.ToLower() + ".bundle";
            build.assetNames = new string[1] { inputPath };
            builds.Add(build);

            var pref = JsonUtility.FromJson<Preference>(PlayerPrefs.GetString("prefrence"));

            if (pref.enabled_build_ios)
            {
                var iOSPath = outputPath + "/iOS";
                Directory.CreateDirectory(iOSPath);
                BuildPipeline.BuildAssetBundles(iOSPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
            }

            if (pref.enabled_build_android)
            {
                var androidPath = outputPath + "/Android";
                Directory.CreateDirectory(androidPath);
                BuildPipeline.BuildAssetBundles(androidPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
            }

            if (pref.enabled_build_standalone_osx)
            {
                var osxPath = outputPath + "/StandaloneOSX";
                Directory.CreateDirectory(osxPath);
                BuildPipeline.BuildAssetBundles(osxPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);
            }

            if (pref.enabled_build_standalone_windows64)
            {
                var winPath = outputPath + "/StandaloneWindows64";
                Directory.CreateDirectory(winPath);
                BuildPipeline.BuildAssetBundles(winPath, builds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
            }

            if (pref.enabled_compression)
            {
                var manifest = new AssetManifestData();
                manifest.asset_id = assetId;
                File.WriteAllText(outputPath + "/manifest.txt", JsonUtility.ToJson(manifest));

                if (File.Exists(outputPath + ".zip"))
                    File.Delete(outputPath + ".zip");

                ZipFile.CreateFromDirectory(outputPath, outputPath + ".zip");
            }
        }
    }

    #endregion

    #region -- Scene Build --

    public class SceneBuilder : AssetBuilder
    {
        static public void Build(SceneAsset scene)
        {
            var inputPath = AssetDatabase.GetAssetOrScenePath(scene);

            // Property Window
            var proptyModalWindow = ScriptableObject.CreateInstance<BuildPropertyEditor>();
            proptyModalWindow.Show(inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(scene);

                Directory.CreateDirectory(outputPath);

                AssetBuild(inputPath, outputPath, assetId);
            };
        }
    }

    #endregion


    #region -- BuildPropertyEditor --

    class BuildPropertyEditor : EditorWindow
    {
        public Action<string, string> _completion;

        public string _outputPath;

        public string _assetId = "";

        public Preference _preference;

        public void Show(string inputPath)
        {
            _assetId = Path.GetFileNameWithoutExtension(inputPath);

            ShowUtility();
        }

        void Awake()
        {
            titleContent = new GUIContent("Asset Build");

            minSize = new Vector2(320, 520);
            position = new Rect(Vector2.zero, minSize);

            _outputPath = Application.dataPath + "/KeyakiStudioKit/AssetBundles";

            if (PlayerPrefs.HasKey("prefrence"))
            {
                _preference = JsonUtility.FromJson<Preference>(PlayerPrefs.GetString("prefrence"));
            }
            else
            {
                _preference = new Preference();
                _preference.enabled_build_ios = true;
                _preference.enabled_build_android = true;
                _preference.enabled_build_standalone_windows64 = true;
                _preference.enabled_build_standalone_osx = true;
                _preference.enabled_compression = false;
            }
        }

        void OnGUI()
        {
            var titleTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/KeyakiStudioKit/Editor/title.png");
            EditorGUILayout.LabelField(new GUIContent(titleTex), GUILayout.Height(240 * 1600f / 2400f), GUILayout.Width(320));

            GUILayout.Space(20);

            EditorGUILayout.LabelField("#1 Please enter the output directory path.", EditorStyles.wordWrappedLabel);

            _outputPath = EditorGUILayout.TextField(_outputPath);

            GUILayout.Space(20);

            EditorGUILayout.LabelField("#2 Please enter asset name.", EditorStyles.wordWrappedLabel);

            _assetId = EditorGUILayout.TextField(_assetId);

            GUILayout.Space(20);

            EditorGUILayout.LabelField("#3 OS", EditorStyles.wordWrappedLabel);

            _preference.enabled_build_ios = EditorGUILayout.Toggle("iOS", _preference.enabled_build_ios);

            _preference.enabled_build_android = EditorGUILayout.Toggle("Android", _preference.enabled_build_android);

            _preference.enabled_build_standalone_windows64 = EditorGUILayout.Toggle("Windows", _preference.enabled_build_standalone_windows64);

            _preference.enabled_build_standalone_osx = EditorGUILayout.Toggle("MacOS", _preference.enabled_build_standalone_osx);

            GUILayout.Space(20);

            EditorGUILayout.LabelField("#4 Compression (Zip)", EditorStyles.wordWrappedLabel);

            _preference.enabled_compression = EditorGUILayout.Toggle("Compress", _preference.enabled_compression);

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

                PlayerPrefs.SetString("prefrence", JsonUtility.ToJson(_preference));
                PlayerPrefs.Save();
            }
        }
    }

    #endregion

    #region -- Inner Class --

    [Serializable]
    class Preference
    {
        public bool enabled_build_ios;

        public bool enabled_build_android;

        public bool enabled_build_standalone_windows64;

        public bool enabled_build_standalone_osx;

        public bool enabled_compression;
    }

    class ZipUtils
    {
        //public static void Compress(byte[] srcBytes, string filePath)
        //{
        //    using var stream = new MemoryStream();
        //    using var gzipStream = new GZipStream(stream, System.IO.Compression.CompressionLevel.Optimal);

        //    gzipStream.Write(srcBytes, 0, srcBytes.Length);
        //    gzipStream.Dispose();

        //    var bytes = stream.ToArray();
        //    File.WriteAllBytes(filePath, srcBytes);
        //}

        public static void Decompress(byte[] srcBytes, string dirPath, bool overwrite = true)
        {
            using var stream = new MemoryStream(srcBytes);

            using var zipArchive = new ZipArchive(stream);

            foreach (var file in zipArchive.Entries)
            {
                var fileName = Path.Combine(dirPath, file.FullName);
                var directory = Path.GetDirectoryName(fileName);

                if (directory != null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!string.IsNullOrEmpty(file.Name))
                {
                    if (overwrite || !File.Exists(fileName))
                    {
                        file.ExtractToFile(fileName, overwrite);
                    }
                }
            }
        }
    }

    #endregion
}