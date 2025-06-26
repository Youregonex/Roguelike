using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitAnimationComponent : BattleUnitComponent
    {
        private const string ANIMATION_ATTACK_TRIGGER = "ATTACK";
        private Animator _animator;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);
            _animator = GetComponent<Animator>();
        }

        public void PlayAttackAnimation()
        {
            _animator.SetTrigger(ANIMATION_ATTACK_TRIGGER);
        }
    }
}
