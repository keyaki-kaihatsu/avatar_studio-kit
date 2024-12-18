using UnityEngine;
using UnityEditor;
using System.IO;

namespace AvatarStudio
{
    public class AnimationClipEditor : MonoBehaviour
    {
        [MenuItem("Assets/KeyakiStudio/Export Animation Clip", false, 0)]
        static public void OnAssets()
        {
            if (Selection.objects.Length > 0)
            {
                if (Selection.objects[0] is TextAsset txt)
                {
                    var json = JsonUtility.FromJson<AnimationClipData>(txt.text);
                    var clip = GenerateAnimationClip(json);

                    var path = AssetDatabase.GetAssetPath(Selection.objects[0].GetInstanceID());
                    path = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + ".anim";
                    
                    AssetDatabase.CreateAsset(clip, path);
                    AssetDatabase.Refresh();
                    return;
                }
            }

            Debug.LogError("[KeyakiStudioKit] Not Selected.");
        }

        static AnimationClip GenerateAnimationClip(AnimationClipData data)
        {
            var clip = new AnimationClip();

            var LENGTH = data.frames[0].keys.Length;
            var curveList = new AnimationCurve[LENGTH];
            for (var i = 0; i < curveList.Length; i++)
                curveList[i] = new AnimationCurve();

            foreach (var frameData in data.frames)
            {
                for (var i = 0; i < LENGTH; i++)
                {
                    var keyData = frameData.keys[i];
                    var key = new Keyframe(frameData.timestamp, keyData.value);
                    key.weightedMode = WeightedMode.None;
                    curveList[i].AddKey(key);
                }
            }

            for (var i = 0; i < LENGTH; i++)
            {
                var keyData = data.frames[0].keys[i];
                clip.SetCurve(keyData.path, keyData.ClassType, keyData.property_name, curveList[i]);
            }

            return clip;
        }
    }
}
