using UnityEngine;

namespace Yg.Battle
{
    public abstract class SpellSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField, TextArea(3, 10)] public string Description { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public float Range { get; private set; }

        public abstract Spell BuildSpell();
    }
}
