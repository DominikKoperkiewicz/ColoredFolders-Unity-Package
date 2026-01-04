using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ColoredFolders.Editor
{
    [InitializeOnLoad]
    public static class ColoredFoldersDrawer
    {
        private static List<FolderColorEntry> _folderColors = new();
        
        private static Color EditorBackgroundColor
        {
            get
            {
                return EditorGUIUtility.isProSkin
                               ? new Color(0.219f, 0.219f, 0.219f)
                               : new Color(0.76f, 0.76f, 0.76f);
            }
        }
        
        static ColoredFoldersDrawer()
        {
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
            LoadColors();
        }

        static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
        {
            UpdateFolderColors(guid, selectionRect);
        }

        private static void UpdateFolderColors(string guid, Rect selectionRect)
        {
            bool isListView = selectionRect.height <= EditorGUIUtility.singleLineHeight + 2f;

            if (isListView && ColoredFoldersStorage.Settings.EnableZebraStriping)
            {
                TryDrawZebraStrip(selectionRect);
            }
            
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (AssetDatabase.IsValidFolder(path) == false) return;

            FolderColorEntry folderEntry = FindSettingsForFolder(path);
            if (folderEntry.Path == "" ) return;
            
            bool isRecursion = folderEntry.Path != path;
            

            if (isListView)
            {
                if (folderEntry.UseColor)
                {
                    if (isRecursion == false || folderEntry.IsColorRecursive)
                    {
                        GUI.DrawTexture(selectionRect, folderEntry.FolderColor.Background, ScaleMode.StretchToFill);
                    }
                }
            }

            Texture icon;

            if (folderEntry.FolderColor.Icon != null && isRecursion == false || folderEntry.IsIconRecursive)
            {
                icon = folderEntry.FolderColor.Icon;
            }
            else
            {
                icon = EditorGUIUtility.ObjectContent(AssetDatabase.LoadAssetAtPath<Object>(path), null).image;
            }
            
            bool small = selectionRect.height <= 20f;

            if (small)
            {
                Rect iconRect = new Rect(selectionRect.x , selectionRect.y, 16, 16);
                EditorGUI.DrawRect(iconRect, EditorBackgroundColor);
                GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            }
            else
            {
                GUI.DrawTexture(selectionRect, icon, ScaleMode.ScaleToFit);
            }
        }

        private static void TryDrawZebraStrip(Rect itemRect)
        {
            int index = Mathf.FloorToInt(itemRect.y / itemRect.height);

            if (index % 2 == 0)
            {
                Color backColor = new Color(0f, 0f, 0f, 0.07f);
                Rect backRect = new Rect(0f , itemRect.y, EditorGUIUtility.currentViewWidth , 16);
                EditorGUI.DrawRect(backRect, backColor);
            }
        }

        static FolderColorEntry FindSettingsForFolder(string path)
        {
            
            if (string.IsNullOrEmpty(path))
            {
                return ColoredFoldersStorage.DefaultColorEntry;
            }

            // Normalize
            path = path.Replace("\\", "/");

            // Direct match
            FolderColorEntry match = _folderColors.FirstOrDefault(colorData => colorData.Path == path);

            if (match != null && match.FolderColor != null)
            {
                return match;
            }

            // Get parent
            string parentPath = System.IO.Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(parentPath))
            {
                return ColoredFoldersStorage.DefaultColorEntry;
            }

            // Normalize again for safety
            parentPath = parentPath.Replace("\\", "/");

            match = _folderColors.FirstOrDefault(colorData => colorData.Path == parentPath);
            
            // Unity root folder names like "Assets" should stop recursion
            if (parentPath == "Assets")
            {
                if (match != null && match.FolderColor != null)
                {
                    return match;
                }
            }

            // Recursive lookup
            return FindSettingsForFolder(parentPath);
        }


        static void LoadColors()
        {
            _folderColors = ColoredFoldersStorage.ColorsData;
        }
        
        public static void ReloadColors(bool repaint = true)
        {
            _folderColors = ColoredFoldersStorage.ColorsData;

            if (repaint)
            {
                EditorApplication.RepaintProjectWindow();
            }
        }

    }
}
