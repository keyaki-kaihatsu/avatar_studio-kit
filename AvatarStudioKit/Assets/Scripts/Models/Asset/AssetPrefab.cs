using UnityEngine;

namespace AvatarStudio
{
    public enum AssetType : int
    {
        NoCharacteristics,
        AvatarItem,
        Scene
    }

    public class AssetPrefab : MonoBehaviour
    {
        public AssetType type;
    }
}