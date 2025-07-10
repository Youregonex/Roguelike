using UnityEngine;
using System;
using Yg.Character;

namespace Yg.Battle
{
    public class SquadPlacementArea : MonoBehaviour
    {
        public event Action<SquadPlacementArea> OnHover;
        public event Action<SquadPlacementArea> OnHoverEnd;
        public event Action<SquadPlacementArea> OnClick;

        [CustomHeader("Settings")]
        [SerializeField] private SpriteRenderer _squadIconSR;
        [SerializeField] private SpriteRenderer _highlightSR;
        [SerializeField] private SpriteRenderer _prePlacementHighlightSR;

        private WarbandSlot _warbandSlot;
        private BoxCollider2D _boxCollider;

        public WarbandSlot WarbandSlot => _warbandSlot;
        public BoxCollider2D Collider => _boxCollider;
        public SpriteRenderer SquadIconSR => _squadIconSR;
        public bool Empty => _warbandSlot is null || _warbandSlot.UnitEmpty;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            PrePlacementUnhighlight();
            DefaultUnhighlight();
        }

        public void SetWarbandSlot(WarbandSlot warbandSlot)
        {
            _warbandSlot = warbandSlot;
            _squadIconSR.sprite = warbandSlot.UnitData.Icon;
        }

        public void DefaultHighlight()
        {
            _highlightSR.color = new(1f, 1f, 1f, 1f);
        }

        public void DefaultUnhighlight()
        {
            _highlightSR.color = new(1f, 1f, 1f, .3f);
        }

        public void PrePlacementHighlight()
        {
            _prePlacementHighlightSR.gameObject.SetActive(true);
        }

        public void PrePlacementUnhighlight()
        {
            _prePlacementHighlightSR.gameObject.SetActive(false);
        }

        public void ClearSlot()
        {
            _warbandSlot = null;
            _squadIconSR.sprite = null;
        }

        private void OnMouseDown()
        {
            OnClick?.Invoke(this);
        }

        private void OnMouseEnter()
        {
            OnHover?.Invoke(this);
            PrePlacementHighlight();
        }

        private void OnMouseExit()
        {
            OnHoverEnd?.Invoke(this);
            PrePlacementUnhighlight();
        }
    }
}
