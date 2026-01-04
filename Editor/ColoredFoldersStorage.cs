using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ColoredFolders.Editor
{
    public static class ColoredFoldersStorage
    {
        private static ColoredFoldersSettings _colorSettings;
        const string ASSET_PATH = "Assets/ColoredFoldersSettings.asset";

        public static ColoredFoldersSettings Settings => _colorSettings;
        public static FolderColorEntry DefaultColorEntry { get; } = new("", false, null, false);

        private static ColoredFoldersSettings GetData()
        {
            if (_colorSettings == null)
            {
                string[] guids = AssetDatabase.FindAssets("t:" + nameof(ColoredFoldersSettings));
                string path = AssetDatabase.GUIDToAssetPath(guids.FirstOrDefault());
                _colorSettings = AssetDatabase.LoadAssetAtPath<ColoredFoldersSettings>(path);
                
                if (_colorSettings == null)
                {
                    _colorSettings = ScriptableObject.CreateInstance<ColoredFoldersSettings>();
                    AssetDatabase.CreateAsset(_colorSettings, ASSET_PATH);
                    AssetDatabase.SaveAssets();
                }
            }
            return _colorSettings;
        }

        public static List<FolderColorEntry> ColorsData
        {
            get
            {
                GetData();
                return _colorSettings.ColorsData;
            }
        }
    }
}