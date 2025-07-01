using UnityEngine;

namespace Yg.Battle.BattleUnits
{
    public class BattleUnitVisualComponent : BattleUnitComponent, ITickableBattleUnitComponent
    {
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;

        public override void InitializeComponent(BattleUnitCore battleUnitCore)
        {
            base.InitializeComponent(battleUnitCore);

            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _rigidbody = _battleUnitCore.GetComponent<Rigidbody2D>();

            if (_battleUnitCore.UnitFaction == EUnitFaction.Enemy)
                _spriteRenderer.flipX = true;
        }

        public void Tick()
        {
            //_spriteRenderer.flipX = _rigidbody.velocity.x <= 0 ? true : false;
        }
    }
}
