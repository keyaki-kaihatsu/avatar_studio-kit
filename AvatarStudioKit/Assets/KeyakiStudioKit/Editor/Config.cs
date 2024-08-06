using System.IO;
using UnityEngine;

namespace AvatarStudio
{
    public class Config
    {
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
}