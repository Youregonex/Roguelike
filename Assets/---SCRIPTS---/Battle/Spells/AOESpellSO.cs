using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yg.Battle
{
    public abstract class AOESpellSO : SpellSO
    {
        [field: SerializeField] public float ImpactRadius { get; private set; }

    }
}
