using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AvatarStudio
{
    public static class Locale
    {
        #region -- Static Public Properties --

        static public Dictionary<string, string> EN_STRING_LIST;

        static public Dictionary<string, string> JA_STRING_LIST;

        #endregion

        #region -- Static Public --

        static public string Get(string key)
        {
            var lang = UnityEditor.EditorPrefs.GetString("EditorLanguage", "en");
            switch (lang)
            {
                case "ja":
                    return JA_STRING_LIST.ContainsKey(key) ? JA_STRING_LIST[key] : key;
                default:
                    return EN_STRING_LIST.ContainsKey(key) ? EN_STRING_LIST[key] : key;
            }
        }

        #endregion

        #region -- Override --

        static Locale()
        {
            if (EN_STRING_LIST == null)
            {
                EN_STRING_LIST = new Dictionary<string, string>();

                var json = Resources.Load<TextAsset>("strings/ksk_en").text;
                var strings = JsonUtility.FromJson<StringData>(json).strings.ToList();
                strings.ForEach(data => EN_STRING_LIST[data.key] = data.value);
            }
            
            if (JA_STRING_LIST == null)
            {
                JA_STRING_LIST = new Dictionary<string, string>();

                var json = Resources.Load<TextAsset>("strings/ksk_ja").text;
                var strings = JsonUtility.FromJson<StringData>(json).strings.ToList();
                strings.ForEach(data => JA_STRING_LIST[data.key] = data.value);
            }
        }

        #endregion

        #region -- Inner Class --

        [Serializable]
        public class StringData
        {
            public StringDataData[] strings;
        }

        [Serializable]
        public class StringDataData
        {
            public string key;

            public string value;
        }
        
        #endregion
    }
}
