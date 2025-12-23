using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

namespace AvatarStudio
{
    static public class Config
    {
        static public bool DEBUG = false;

        static public string DOMAIN => DEBUG ? "http://localhost:3000" : "https://keyaki-studio.onrender.com";

        static public string ASSETS_URL => DOMAIN + "/assets";

        static public string ASSET_FILES_API_URL => DOMAIN + "/api/v1/asset_files";

        static public string ROOT_PATH
        {
            get
            {
                var path = Application.dataPath;
                var dir = new DirectoryInfo(path);
                return dir.Parent.FullName + "/AssetBundles";
            }
        }
    }

    public class BuildEditor
    {
        [MenuItem("Assets/Keyaki Studio/Asset Build (From Prefab)", false, 0)]
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
            var path = Config.ROOT_PATH;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }

    public class AvatarBuildEditor
    {
        [MenuItem("Assets/Keyaki Studio/Avatar Asset Build (From Prefab)", false, 1)]
        static public void OnAssets()
        {
            SetUp();

            if (Selection.objects.Length > 0)
            {
                if (Selection.objects[0] is GameObject obj)
                {
                    // VRM
                    VRMBuilder.VRMBuild(obj);
                    return;
                }
            }

            Debug.LogError("[KeyakiStudioKit] Not Selected.");
        }

        static void SetUp()
        {
            var path = Config.ROOT_PATH;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }

    public class AnimationBuildEditor
    {
        [MenuItem("Assets/Keyaki Studio/Animation Build (From AnimationClip)", false, 2)]
        static public void OnAssets()
        {
            SetUp();

            if (Selection.objects.Length > 0)
            {
                if (Selection.objects[0] is AnimationClip clip)
                {
                    // Animation Clip
                    VRMBuilder.AnimationClipBuild(clip);
                    return;
                }
            }

            Debug.LogError("[KeyakiStudioKit] Not Selected.");
        }

        // [MenuItem("Assets/Keyaki Studio/Animation Asset Build (From VRM Prefab + Animation)", false, 2)]
        // static public void OnAssets()
        // {
        //     SetUp();

        //     if (Selection.objects.Length > 0)
        //     {
        //         if (Selection.objects[0] is GameObject obj)
        //         {
        //             // Animation Clip
        //             VRMBuilder.AnimationBuild(obj);
        //             return;
        //         }
        //     }

        //     Debug.LogError("[KeyakiStudioKit] Not Selected.");
        // }

        static void SetUp()
        {
            var path = Config.ROOT_PATH;
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
            proptyModalWindow.Show("Asset Build", inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(ap.gameObject);

                Directory.CreateDirectory(outputPath);

                AssetBuild(inputPath, outputPath, assetId);

                System.Diagnostics.Process.Start(outputPath);
            };
        }

        protected static void AssetBuild(string inputPath, string outputPath, string assetId, BuildAssetBundleOptions option = BuildAssetBundleOptions.ChunkBasedCompression)
        // protected static void AssetBuild(string inputPath, string outputPath, string assetId, BuildAssetBundleOptions option = BuildAssetBundleOptions.None)
        {
            var builds = new List<AssetBundleBuild>();

            if (Directory.Exists(outputPath))
                Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);

            var build = new AssetBundleBuild();
            build.assetBundleName = assetId.ToLower() + ".bundle";
            build.assetNames = new string[1] { inputPath };
            builds.Add(build);

            var pref = JsonUtility.FromJson<PreferenceData>(PlayerPrefs.GetString("prefrence"));

            if (pref.enabled_build_ios)
            {
                var iOSPath = outputPath + "/iOS";
                Directory.CreateDirectory(iOSPath);
                BuildPipeline.BuildAssetBundles(iOSPath, builds.ToArray(), option, BuildTarget.iOS);
            }

            if (pref.enabled_build_android)
            {
                var androidPath = outputPath + "/Android";
                Directory.CreateDirectory(androidPath);
                BuildPipeline.BuildAssetBundles(androidPath, builds.ToArray(), option, BuildTarget.Android);
            }

            if (pref.enabled_build_standalone_osx)
            {
                var osxPath = outputPath + "/StandaloneOSX";
                Directory.CreateDirectory(osxPath);
                BuildPipeline.BuildAssetBundles(osxPath, builds.ToArray(), option, BuildTarget.StandaloneOSX);
            }

            if (pref.enabled_build_standalone_windows64)
            {
                var winPath = outputPath + "/StandaloneWindows64";
                Directory.CreateDirectory(winPath);
                BuildPipeline.BuildAssetBundles(winPath, builds.ToArray(), option, BuildTarget.StandaloneWindows64);
            }

            var manifest = new AssetManifestData();
            manifest.asset_id = assetId;
            File.WriteAllText(outputPath + "/manifest.json", JsonUtility.ToJson(manifest));

            if (pref.enabled_compression)
            {
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
            proptyModalWindow.Show("Scene Build", inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(scene);

                Directory.CreateDirectory(outputPath);

                AssetBuild(inputPath, outputPath, assetId);

                System.Diagnostics.Process.Start(outputPath);
            };
        }
    }

    #endregion

    #region -- VRM Build --

    public class VRMBuilder : AssetBuilder
    {
        static public void VRMBuild(GameObject obj)
        {
            var inputPath = AssetDatabase.GetAssetOrScenePath(obj);

            // Avatar Prefab
            var ap = obj.GetComponent<AvatarPrefab>();
            if (ap == null)
                ap = obj.AddComponent<AvatarPrefab>();

            // Property Window
            var proptyModalWindow = ScriptableObject.CreateInstance<BuildPropertyEditor>();
            proptyModalWindow._assetType = "avatar";
            proptyModalWindow.Show("Avatar Build", inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(ap.gameObject);

                Directory.CreateDirectory(outputPath);

                AssetBuild(inputPath, outputPath, assetId);

                System.Diagnostics.Process.Start(outputPath);
            };
        }

        static public void AnimationClipBuild(AnimationClip clip)
        {
            var inputPath = AssetDatabase.GetAssetPath(clip);

            // Property Window
            var proptyModalWindow = ScriptableObject.CreateInstance<BuildPropertyEditor>();
            proptyModalWindow._assetType = "animation";
            proptyModalWindow.Show("AnimationClip Build", inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(clip);

                Directory.CreateDirectory(outputPath);

                AssetBuild(inputPath, outputPath, assetId);

                System.Diagnostics.Process.Start(outputPath);
            };
        }

        static public void AnimationBuild(GameObject obj)
        {
            var inputPath = AssetDatabase.GetAssetOrScenePath(obj);

            // Animator以外（VRMInstance）を無効化
            obj.GetComponents<Behaviour>().Where(b => !(b is Animator)).ToList().ForEach(b => b.enabled = false);

            var animator = obj.GetComponent<Animator>();
            animator.applyRootMotion = true;

            // Property Window
            var proptyModalWindow = ScriptableObject.CreateInstance<BuildPropertyEditor>();
            proptyModalWindow._assetType = "animation";
            proptyModalWindow.Show("Animation Build", inputPath);

            proptyModalWindow._completion = (outputPath, assetId) =>
            {
                // Save
                EditorUtility.SetDirty(obj);

                Directory.CreateDirectory(outputPath);

                AssetBuild(inputPath, outputPath, assetId);

                System.Diagnostics.Process.Start(outputPath);
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

        public PreferenceData _preference;

        public string _assetType = "object";

        public void Show(string title, string inputPath)
        {
            titleContent = new GUIContent(title);

            _assetId = Path.GetFileNameWithoutExtension(inputPath);

            ShowUtility();
        }

        void Awake()
        {
            minSize = new Vector2(380, 800);
            position = new Rect(Vector2.zero, minSize);

            _outputPath = Config.ROOT_PATH;

            if (PlayerPrefs.HasKey("prefrence"))
            {
                _preference = JsonUtility.FromJson<PreferenceData>(PlayerPrefs.GetString("prefrence"));
            }
            else
            {
                _preference = new PreferenceData();
                _preference.enabled_build_ios = true;
                _preference.enabled_build_android = true;
                _preference.enabled_build_standalone_windows64 = true;
                _preference.enabled_build_standalone_osx = true;
                _preference.enabled_compression = false;
                _preference.access_code = "";
            }
        }

        void OnGUI()
        {
            var titleStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
            titleStyle.fontSize = 16;
            titleStyle.fontStyle = FontStyle.Bold;

            var linkStyle = new GUIStyle(EditorStyles.label);
            linkStyle.normal.textColor = Color.white;
            linkStyle.hover.textColor = Color.gray;
            linkStyle.fontStyle = FontStyle.Italic;

            GUILayout.Space(15);

            var titleTex = Resources.Load<Texture>("ksk_title");
            EditorGUILayout.LabelField(new GUIContent(titleTex), GUILayout.Height(160f * 0.7f), GUILayout.Width(320f * 0.7f));

            GUILayout.Space(15);

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField(Locale.Get("build"), titleStyle);

                    GUILayout.Space(15);

                    EditorGUILayout.LabelField("#1 " + Locale.Get("please_enter_output_path"), EditorStyles.wordWrappedLabel);

                    _outputPath = EditorGUILayout.TextField(_outputPath);

                    GUILayout.Space(15);

                    EditorGUILayout.LabelField("#2 " + Locale.Get("please_enter_name"), EditorStyles.wordWrappedLabel);

                    _assetId = EditorGUILayout.TextField(_assetId);

                    GUILayout.Space(15);

                    EditorGUILayout.LabelField("#3 " + Locale.Get("build_target"), EditorStyles.wordWrappedLabel);

                    var isIOSInstalled = BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.iOS, BuildTarget.iOS);
                    if (isIOSInstalled) 
                    {
                        _preference.enabled_build_ios = EditorGUILayout.Toggle("iOS", _preference.enabled_build_ios);
                    }
                    else
                    {
                        GUI.enabled = false;
                        _preference.enabled_build_ios = EditorGUILayout.Toggle("iOS", false);
                        EditorGUILayout.LabelField("Not Installed", EditorStyles.boldLabel);
                        GUI.enabled = true;
                    
                    }

                    var isAndroidInstalled = BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android);
                    if (isAndroidInstalled)
                    {
                        _preference.enabled_build_android = EditorGUILayout.Toggle("Android", _preference.enabled_build_android);
                    }
                    else
                    {
                        GUI.enabled = false;
                        _preference.enabled_build_android = EditorGUILayout.Toggle("Android", false);
                        EditorGUILayout.LabelField("Not Installed", EditorStyles.boldLabel);
                        GUI.enabled = true;
                    }

                    var isWinInstalled = BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
                    if (isWinInstalled)
                    {
                        _preference.enabled_build_standalone_windows64 = EditorGUILayout.Toggle("Windows", _preference.enabled_build_standalone_windows64);
                    }
                    else
                    {
                        GUI.enabled = false;
                        _preference.enabled_build_standalone_windows64 = EditorGUILayout.Toggle("Windows", false);
                        EditorGUILayout.LabelField("Not Installed", EditorStyles.boldLabel);
                        GUI.enabled = true;
                    }

                    var isMacInstalled = BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
                    if (isMacInstalled)
                    {
                        _preference.enabled_build_standalone_osx = EditorGUILayout.Toggle("MacOS", _preference.enabled_build_standalone_osx);
                    }
                    else
                    {
                        GUI.enabled = false;
                        _preference.enabled_build_standalone_osx = EditorGUILayout.Toggle("MacOS", false);
                        EditorGUILayout.LabelField("Not Installed", EditorStyles.boldLabel);
                        GUI.enabled = true;
                    }

                    GUILayout.Space(15);

                    EditorGUILayout.LabelField("#4 " + Locale.Get("compression_zip"), EditorStyles.wordWrappedLabel);

                    _preference.enabled_compression = EditorGUILayout.Toggle(Locale.Get("compression"), _preference.enabled_compression);

                    GUILayout.Space(15);

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.Space();

                        if (GUILayout.Button(Locale.Get("build"), GUILayout.MaxWidth(200f)))
                        {
                            _completion?.Invoke(_outputPath + "/" + _assetId, _assetId);
                        }

                        EditorGUILayout.Space();
                    }
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(25);

                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(10);

                        var rect = GUILayoutUtility.GetRect(1, 1, GUILayout.ExpandWidth(true), GUILayout.Height(1));
                        EditorGUI.DrawRect(rect, Color.gray);

                        GUILayout.Space(10);
                    }
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(25);

                    EditorGUILayout.LabelField(Locale.Get("upload"), titleStyle);

                    GUILayout.Space(15);

                    var linkRect = GUILayoutUtility.GetRect(new GUIContent(Locale.Get("get_access_code")), linkStyle);
                    EditorGUIUtility.AddCursorRect(linkRect, MouseCursor.Link);
                    if (GUI.Button(linkRect, Locale.Get("get_access_code"), linkStyle))
                    {
                        Application.OpenURL("https://keyaki-studio.onrender.com/temporary_sessions/new?redirect_to=new_temporary_sessions");
                    }

                    GUILayout.Space(15);

                    EditorGUILayout.LabelField("#1 " + Locale.Get("access_code"), EditorStyles.wordWrappedLabel);

                    _preference.access_code = EditorGUILayout.TextField(_preference.access_code);

                    GUILayout.Space(10);

                    var zipPath = _outputPath + "/" + _assetId + ".zip";
                    var zipExists = File.Exists(zipPath);
                    var codeExists = !string.IsNullOrEmpty(_preference.access_code);

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.Space();

                        GUI.enabled = zipExists && codeExists;

                        if (GUILayout.Button(Locale.Get("upload"), GUILayout.MaxWidth(200f)))
                        {
                            var error = Upload(zipPath, code: _preference.access_code, assetType: _assetType);
                            if (string.IsNullOrEmpty(error))
                            {
                                if (EditorUtility.DisplayDialog(Locale.Get("success"), Locale.Get("success_message"), "OK"))
                                {
                                    Application.OpenURL(Config.ASSETS_URL);
                                }
                            }
                            else
                            {
                                EditorUtility.DisplayDialog(Locale.Get("failed"), error, "OK");
                            }
                        }

                        GUI.enabled = true;

                        EditorGUILayout.Space();
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!zipExists || !codeExists)
                    {
                        GUILayout.Space(5);

                        if (!zipExists)
                        {
                            GUILayout.Space(10);

                            EditorGUILayout.HelpBox(Locale.Get("please_build_and_compress"), MessageType.Warning);
                        }

                        if (!codeExists)
                        {
                            GUILayout.Space(10);

                            EditorGUILayout.HelpBox(Locale.Get("please_enter_access_code"), MessageType.Warning);
                        }
                    }
                    else
                    {
                        GUILayout.Space(15);

                        EditorGUILayout.HelpBox(Locale.Get("ready_to_upload"), MessageType.Info);
                    }
                }
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);
            }
            EditorGUILayout.EndHorizontal();

            PlayerPrefs.SetString("prefrence", JsonUtility.ToJson(_preference));
            PlayerPrefs.Save();
        }

        string Upload(string zipPath, string code, string assetType)
        {
            var zipBytes = File.ReadAllBytes(zipPath);

            var form = new WWWForm();
            form.AddField("access_code", code);
            form.AddField("file_name", Path.GetFileName(zipPath), Encoding.UTF8);
            form.AddBinaryData("file", zipBytes, "upload.zip", "application/zip");
            form.AddField("asset_type", assetType);

            using (var www = UnityWebRequest.Post(Config.ASSET_FILES_API_URL, form))
            {
                www.SendWebRequest();
                while (!www.isDone)
                {
                    System.Threading.Thread.Sleep(10);
                }

#if UNITY_2020_1_OR_NEWER
                if (www.result != UnityWebRequest.Result.Success)
#else
                if (www.isNetworkError || www.isHttpError)
#endif
                {
                    return www.error.ToString();
                }
                else
                {
                    var json = www.downloadHandler.text;
                    var data = JsonUtility.FromJson<APIData<string>>(json);
                    return data.status == 0 ? null : data.error;
                }
            }
        }
    }

    #endregion

    #region -- Inner Class --

    [Serializable]
    class PreferenceData
    {
        public bool enabled_build_ios;

        public bool enabled_build_android;

        public bool enabled_build_standalone_windows64;

        public bool enabled_build_standalone_osx;

        public bool enabled_compression;

        public string access_code;
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

    [Serializable]
    public class APIData<T>
    {
        public int status;

        public string type;

        public T data;
        
        public string error;
	}

    #endregion
}