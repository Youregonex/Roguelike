using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yg.Battle.BattleUnits;

namespace Yg.GameData.Configs
{
    [CreateAssetMenu(fileName = "StatIconConfig", menuName = "Configs/StatIconConfig")]
    public class StatIconConfig : ScriptableObject
    {
        [field: SerializeField] public List<StatToIcon> StatToIconList { get; private set; }

        public Sprite GetIcon(EStat stat)
        {
            return StatToIconList.Where(e => e.Stat == stat).FirstOrDefault().Icon;
        }
    }

    [System.Serializable]
    public class StatToIcon
    {
        public EStat Stat;
        public Sprite Icon;
    }
}
