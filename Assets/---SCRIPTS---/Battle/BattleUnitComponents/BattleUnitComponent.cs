using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public abstract class BattleUnitComponent : MonoBehaviour
    {
        protected BattleUnitCore _battleUnitCore;

        public virtual void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            _battleUnitCore = battleUnitCore;
        }
    }
}
