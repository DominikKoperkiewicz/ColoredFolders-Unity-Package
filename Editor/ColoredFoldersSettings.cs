using System.Collections.Generic;
using UnityEngine;

namespace ColoredFolders.Editor
{    
    [System.Serializable]
    [CreateAssetMenu(menuName = ColoredFoldersConstants.CREATE_ASSET_MENU_PATH + nameof(ColoredFoldersSettings), 
                     fileName = nameof(ColoredFoldersSettings), order = ColoredFoldersConstants.CREATE_ASSET_MENU_ORDER)]
    public class ColoredFoldersSettings : ScriptableObject
    {
        [SerializeField]
        private bool _enableZebraStriping = true;
        
        [SerializeField, Space]
        private List<FolderColorEntry> _colorsData = new();
        
        
        public bool EnableZebraStriping => _enableZebraStriping;
        public List<FolderColorEntry> ColorsData => _colorsData;
        
        private void OnValidate()
        {
            ColoredFoldersDrawer.ReloadColors();
        }
    }
}
