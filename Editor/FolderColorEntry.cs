using System;
using UnityEngine;

namespace ColoredFolders.Editor
{
    [Serializable]
    public class FolderColorEntry
    {
        [SerializeField]
        private FolderColorData _folderColor;
        public FolderColorData FolderColor => _folderColor;
        
        
        [SerializeField]
        private string _path;
        
        [Space, SerializeField]
        private bool _useColor;
        [SerializeField]
        private bool _isColorRecursive;
        
        [SerializeField]
        private bool _isIconRecursive;
        
        public string Path => _path;
        public bool UseColor => _useColor;
        public bool IsColorRecursive => _isColorRecursive;
        public bool IsIconRecursive => _isIconRecursive;

        public FolderColorEntry(string path, bool isColorRecursive, Texture icon, bool isIconRecursive)
        {
            _path = path;
            _isColorRecursive = isColorRecursive;
            _isIconRecursive = isIconRecursive;
        }
    }
}
