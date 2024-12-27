using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace AvatarStudio
{
    public class AnimationClipEditor : MonoBehaviour
    {
        [MenuItem("Assets/KeyakiStudio/Export Animation (From .txt)", false, 100)]
        static public void OnAssets()
        {
            if (Selection.objects.Length > 0)
            {
                if (Selection.objects[0] is TextAsset txt)
                {
                    var json = JsonUtility.FromJson<AnimationClipData>(txt.text);
                    var clip = GenerateAnimationClip(json);

                    var path = AssetDatabase.GetAssetPath(Selection.objects[0].GetInstanceID());
                    var animPath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + ".anim";
                    
                    AssetDatabase.CreateAsset(clip, animPath);
                    AssetDatabase.Refresh();

                    // Animator Controllerの生成
                    var cntlPath = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + ".controller";
                    var animCntl = AnimatorController.CreateAnimatorControllerAtPath(cntlPath);
                    var animCntlRoot = animCntl.layers[0].stateMachine;
                    var animCntlState = animCntlRoot.AddState(clip.name);
                    animCntlState.motion = clip;
                    animCntlRoot.defaultState = animCntlState;

                    AssetDatabase.SaveAssets();

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
                    curveList[i].AddKey(frameData.timestamp, keyData.value);
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
