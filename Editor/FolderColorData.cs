using UnityEngine;

namespace ColoredFolders.Editor
{
    [CreateAssetMenu(menuName = ColoredFoldersConstants.CREATE_ASSET_MENU_PATH + nameof(FolderColorData), 
                     fileName = nameof(FolderColorData), order = ColoredFoldersConstants.CREATE_ASSET_MENU_ORDER)]
    public class FolderColorData : ScriptableObject
    {
        [SerializeField]
        private Texture _icon;
        [SerializeField]
        private Texture _background;
        
        public Texture Icon => _icon;
        public Texture Background => _background;
    }
}
