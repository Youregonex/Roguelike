using UnityEngine;
using Yg.Battle.BattleUnits;
using System;
using System.Collections;

namespace Yg.Battle
{
    [SelectionBase]
    public class Projectile : MonoBehaviour
    {
        public event Action<Projectile> OnProjectileDestruction;

        [CustomHeader("Settings")]
        [SerializeField] private float _destructionDelay;

        private DamageStruct _damageStruct;
        private Rigidbody2D _rigidbody;

        private Coroutine _destructionCoroutine;
        private bool _enqueueOnDestruction = true;

        public void Initialize(DamageStruct damageStruct, Vector2 velocity)
        {
            if (_rigidbody is null)
                _rigidbody = GetComponent<Rigidbody2D>();

            _damageStruct = damageStruct;
            _rigidbody.velocity = velocity;

            _destructionCoroutine = StartCoroutine(DestructionDelayCoroutine());
        }

        public void DeactivatePooling()
        {
            _enqueueOnDestruction = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out BattleUnitCore battleUnitCore))
            {
                if (battleUnitCore.UnitFaction != _damageStruct.UnitFaction)
                {
                    battleUnitCore.GetUnitComponent<BattleUnitHealthComponent>().TakeDamage(_damageStruct);
                    DestroyProjectile();
                }
            }
        }

        private void DestroyProjectile()
        {
            if(!_enqueueOnDestruction)
            {
                Destroy(gameObject);
                return;
            }

            _damageStruct = default;
            _rigidbody.velocity = Vector2.zero;

            if (_destructionCoroutine is not null)
            {
                StopAllCoroutines();
                _destructionCoroutine = null;
            }

            OnProjectileDestruction?.Invoke(this);
        }

        private IEnumerator DestructionDelayCoroutine()
        {
            yield return new WaitForSeconds(_destructionDelay);
            _destructionCoroutine = null;
            DestroyProjectile();
        }
    }
}
