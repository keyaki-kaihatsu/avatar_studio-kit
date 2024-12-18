using System;
using UnityEngine;

namespace AvatarStudio
{
    [Serializable]
    public class AnimationClipData
    {
        public AnimationClipFrameData[] frames;
    }

     [Serializable]
    public class AnimationClipFrameData
    {
        public AnimationClipKeyData[] keys;

        public float timestamp;
    }

    [Serializable]
    public class AnimationClipKeyData
    {
        public string path;

        public string class_type;

        public string property_name;

        public float value;

        public Type ClassType 
        {
            get
            {
                if (class_type == typeof(SkinnedMeshRenderer).ToString())
                    return typeof(SkinnedMeshRenderer);
                if (class_type == typeof(Animator).ToString())
                    return typeof(Animator);
                return typeof(Transform);
            }
        }
    }
}
