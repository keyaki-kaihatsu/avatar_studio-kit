using UnityEditor;
using UnityEngine;

namespace AvatarStudio
{
    public class CleanMissingScriptsContextMenu
    {
        [MenuItem("Assets/Keyaki Studio/Clean Missing Scripts", false, 110)]
        private static void OnAssets()
        {
            var selectedObjects = Selection.gameObjects;
            var totalRemovedScripts = 0;

            foreach (var obj in selectedObjects)
            {
                var allTransforms = obj.GetComponentsInChildren<Transform>(true);
                foreach (var child in allTransforms)
                {
                    var removedScripts = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(child.gameObject);
                    if (removedScripts > 0)
                    {
                        totalRemovedScripts += removedScripts;
                        Debug.Log($"Removed {removedScripts} missing script(s) from GameObject: {child.gameObject.name}", child.gameObject);
                    }
                }

                EditorUtility.SetDirty(obj);
            }
            
            AssetDatabase.SaveAssets();
            Debug.Log($"Finished cleaning. Total missing scripts removed: {totalRemovedScripts}");
        }
    }
}
