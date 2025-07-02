using UnityEngine;
using Yg.Battle;
using Yg.Battle.BattleUnits;

public class MouseBomb : MonoBehaviour
{
    [CustomHeader("Settings")]
    [SerializeField] private GameObject _explosionPrefab;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Explode(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void Explode(Vector2 position)
    {
        GameObject explosion = Instantiate(_explosionPrefab, position, Quaternion.identity);
        Destroy(explosion, 2f);

        RaycastHit2D[] raycastHit = Physics2D.CircleCastAll(position, 5f, Vector2.zero);

        foreach (var hit in raycastHit)
        {
            if(hit.transform.TryGetComponent(out BattleUnitCore battleUnitCore))
            {
                DamageStruct damageStruct = new(EUnitFaction.None, null, EAttackType.Magic, EDamageType.Physical, 10f, 20f);
                battleUnitCore.GetUnitComponent<BattleUnitHealthComponent>().TakeDamage(damageStruct);
            }
        }
    }
}
