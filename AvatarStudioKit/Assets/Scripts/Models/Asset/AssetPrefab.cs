using UnityEngine;

namespace AvatarStudio
{
    public enum AssetType : int
    {
        NoCharacteristics,
        Grip,
        Scene
    }

    public class AssetPrefab : MonoBehaviour
    {
        public AssetType type;
    }
}