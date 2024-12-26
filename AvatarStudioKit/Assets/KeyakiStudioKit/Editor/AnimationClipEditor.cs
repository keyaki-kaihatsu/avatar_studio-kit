using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEditor;
using UnityEditor.Animations;

namespace AvatarStudio
{
    public class AnimationClipEditor : MonoBehaviour
    {
        [MenuItem("Assets/KeyakiStudio/Export Animation/Create Animator Controller", false, 100)]
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
                    var key = new Keyframe(frameData.timestamp, keyData.value);
                    key.weightedMode = WeightedMode.None;

                    //【注意】Hipsに関係するプロパティを無効化 
                    key.value = keyData.property_name.Contains("Spine") ? 0 : keyData.value;

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

    public class AnimationClipSetUpEditor : MonoBehaviour
    {
        [MenuItem("GameObject/KeyakiStudio/Animation/Set up VRM")]
        static public void OnGameObject()
        {
            if (Selection.objects.Length > 0)
            {
                if (Selection.objects[0] is GameObject obj)
                {
                    // Animator以外（VRMInstance）を無効化
                    obj.GetComponents<Behaviour>().Where(b => !(b is Animator)).ToList().ForEach(b => b.enabled = false);

                    // Parent Constraint
                    var animator = obj.GetComponent<Animator>();
                    var hips = animator.GetBoneTransform(HumanBodyBones.Hips);

                    var constraint = hips.GetComponent<ParentConstraint>();
                    if (constraint == null)
                    {
                        constraint = hips.gameObject.AddComponent<ParentConstraint>();
                        constraint.constraintActive = true;
                        constraint.locked = true;

                        var constObj = new GameObject();
                        constObj.name = "ConstraintSource";
                        constObj.transform.SetParent(animator.transform, false);

                        var constSource = new ConstraintSource();
                        constSource.sourceTransform = constObj.transform;
                        constraint.AddSource(constSource);
                    }

                    return;
                }
            }

            Debug.LogError("[KeyakiStudioKit] Not Selected.");
        }
    }
}
