using System;
using System.Collections.Generic;
using UnityEngine;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Perks
{
    public abstract class PerkSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string PerkId { get; private set; }
        [field: SerializeField, TextArea(3,10)] public string Description { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; protected set; }
        [field: SerializeField] public List<ETag> Tags { get; private set; }
        [field: SerializeField] public EPerkApplicationEvent PerkApplicationEvent { get; protected set; }

        public abstract Perk BuildPerk();

        public void GenerateId() => PerkId = Guid.NewGuid().ToString();

        protected virtual void Validate()
        {
            if (string.IsNullOrEmpty(PerkId)) GenerateId();
        }

        protected void OnValidate()
        {
            Validate();
        }
    }
}
