using UnityEngine;
using System;
using Yg.UI;
using Yg.Character;

namespace Yg.Battle
{
    public class SquadPlacementArea : MonoBehaviour
    {
        public event Action<SquadPlacementArea> OnHover;
        public event Action<SquadPlacementArea> OnHoverEnd;
        public event Action<SquadPlacementArea> OnClick;
        public event Action<SquadPlacementArea> OnRelease;

        [CustomHeader("Settings")]
        [SerializeField] private SpriteRenderer _squadIconSR;
        [SerializeField] private SpriteRenderer _highlightSR;
        [SerializeField] private SpriteRenderer _prePlacementHighlightSR;

        private SquadUI _squadUI = null;
        private WarbandSlot _warbandSlot;

        private BoxCollider2D _boxCollider;

        public SquadUI SquadUI => _squadUI;
        public WarbandSlot WarbandSlot => _warbandSlot;

        public BoxCollider2D Collider => _boxCollider;
        public bool Empty => (_squadUI is null) && (_warbandSlot is null);

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            PrePlacementUnhighlight();
            DefaultUnhighlight();
        }

        public void SetSquadUI(SquadUI squadUI)
        {
            _squadUI = squadUI;
            _squadIconSR.sprite = squadUI.WarbandSlot.UnitData.UnitIcon;
        }

        public void SetWarbandSlot(WarbandSlot warbandSlot)
        {
            _warbandSlot = warbandSlot;
            _squadIconSR.sprite = warbandSlot.UnitData.UnitIcon;
            _squadIconSR.flipX = true;
        }

        public void OnMouseDown()
        {
            OnClick?.Invoke(this);
            ClearAreaVisual();
        }

        public void OnMouseUp()
        {
            OnRelease?.Invoke(this);
        }

        public void OnMouseEnter()
        {
            PrePlacementHighlight();
            OnHover?.Invoke(this);
        }

        public void OnMouseExit()
        {
            PrePlacementUnhighlight();
            OnHoverEnd?.Invoke(this);
        }

        public void ClearSquadUI()
        {
            _squadUI = null;
        }

        public void DefaultHighlight()
        {
            _highlightSR.color = new(1f, 1f, 1f, 1f);
        }

        public void DefaultUnhighlight()
        {
            _highlightSR.color = new(1f, 1f, 1f, .3f);
        }

        private void PrePlacementHighlight()
        {
            _prePlacementHighlightSR.gameObject.SetActive(true);
        }

        private void PrePlacementUnhighlight()
        {
            _prePlacementHighlightSR.gameObject.SetActive(false);
        }

        private void ClearAreaVisual()
        {
            _squadIconSR.sprite = null;
        }
    }
}
