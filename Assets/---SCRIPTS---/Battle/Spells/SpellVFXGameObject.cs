using System.Collections;
using UnityEngine;
using Yg.Pooling;

namespace Yg.Battle
{
    public class SpellVFXGameObject : MonoBehaviour
    {
        private UltimatePooler _pooler;
        private SpellVFXGameObject _selfPrefab;
        private float _lifetime;
        private float _spellImpactRange;

        public bool IsInitialized { get; protected set; }

        public void Initialize(UltimatePooler pooler, SpellVFXGameObject prefab, float lifetime, float spellImpactRange)
        {
            _pooler = pooler;
            _selfPrefab = prefab;
            _spellImpactRange = spellImpactRange;

            SetLifetime(lifetime);
            IsInitialized = true;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void SetLifetime(float lifetime)
        {
            _lifetime = lifetime;
            StartCoroutine(DelayedEnqueue());
        }

        private IEnumerator DelayedEnqueue()
        {
            yield return new WaitForSeconds(_lifetime);
            _pooler.Enqueue(_selfPrefab, this, true);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _spellImpactRange);
        }
    }
}

