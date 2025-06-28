using UnityEngine;
using Yg.Battle.BattleUnits;

public class PlayerUnitsHighlight : MonoBehaviour
{
    [CustomHeader("Settings")]
    [SerializeField] private GameObject _highlightCircleGO;

    private void Start()
    {
        BattleUnitCore core = transform.root.GetComponent<BattleUnitCore>();

        if (core.UnitFaction == EUnitFaction.Player)
        {
            _highlightCircleGO.SetActive(true);
            core.OnDeath += PlayerUnitsHighlight_OnDeath;
        }
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        transform.root.GetComponent<BattleUnitCore>().OnDeath -= PlayerUnitsHighlight_OnDeath;
    }

    private void PlayerUnitsHighlight_OnDeath(BattleUnitCore unit)
    {
        _highlightCircleGO.gameObject.SetActive(false);
    }
}
