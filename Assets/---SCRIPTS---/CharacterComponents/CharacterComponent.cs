using UnityEngine;

namespace Yg.Character
{
    public abstract class CharacterComponent : MonoBehaviour
    {
        protected CharacterCore _characterCore;
        protected bool _componentLoaded = false;

        public virtual void InitializeComponent(CharacterCore characterCore)
        {
            _characterCore = characterCore;
        }

        public abstract void SaveComponent(CharacterSaveData characterSaveData);
        public abstract void LoadComponent(CharacterSaveData characterSaveData);
    }
}
