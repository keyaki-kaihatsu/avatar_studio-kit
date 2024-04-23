using UnityEngine;

namespace AvatarStudio
{
    public class AssetPrefab : MonoBehaviour
    {
        #region -- Public Property --

        [Header("Options")]

        [Tooltip("Objects you want to partially edit.")]
        public GameObject[] parts;

        #endregion
    }
}