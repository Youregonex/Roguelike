using UnityEngine;
using Yg.Battle.BattleUnits;
using System.Collections;
using Yg.Pooling;

namespace Yg.Battle
{
    [SelectionBase]
    public class Projectile : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private float _destructionDelay;

        private Projectile _projectilePrefab;
        private UltimatePooler _ultimatePooler;
        private DamageStruct _currentDamageStruct;
        private Rigidbody2D _rigidbody;

        private Coroutine _destructionCoroutine;

        public bool IsInitialized { get; private set; }

        public void Initialize(UltimatePooler ultimatePooler, Projectile prefab, DamageStruct damageStruct, Vector2 velocity)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _ultimatePooler = ultimatePooler;
            _projectilePrefab = prefab;
            IsInitialized = true;

            Setup(damageStruct, velocity);
        }

        public void Setup(DamageStruct damageStruct, Vector2 velocity)
        {
            _currentDamageStruct = damageStruct;
            _rigidbody.linearVelocity = velocity;

            _destructionCoroutine = StartCoroutine(DestructionDelayCoroutine());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out BattleUnitCore target))
            {
                if (target.UnitFaction != _currentDamageStruct.UnitFaction)
                {
                    if (_currentDamageStruct.Sender is not null)
                        _currentDamageStruct.Sender.DealDamage(_currentDamageStruct, target, true);
                    else
                        target.GetUnitComponent<BattleUnitHealthComponent>().TakeDamage(_currentDamageStruct);

                    DestroyProjectile();
                }
            }
        }

        private void DestroyProjectile()
        {
            _currentDamageStruct = default;
            _rigidbody.linearVelocity = Vector2.zero;

            if (_destructionCoroutine is not null)
            {
                StopAllCoroutines();
                _destructionCoroutine = null;
            }

            _ultimatePooler.Enqueue(_projectilePrefab, this);
        }

        private IEnumerator DestructionDelayCoroutine()
        {
            yield return new WaitForSeconds(_destructionDelay);
            _destructionCoroutine = null;
            DestroyProjectile();
        }
    }
}
