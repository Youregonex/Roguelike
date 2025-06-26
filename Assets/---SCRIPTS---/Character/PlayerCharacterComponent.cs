using UnityEngine;

namespace Yg.Player
{
    public abstract class PlayerCharacterComponent : MonoBehaviour
    {
        protected PlayerCore _playerCore;
        protected bool _componentLoaded = false;

        public virtual void InitializeComponent(PlayerCore playerCore)
        {
            _playerCore = playerCore;
        }

        public abstract void SaveComponent(PlayerSaveData playerSaveData);
        public abstract void LoadComponent(PlayerSaveData playerSaveData);
    }
}
