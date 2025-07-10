using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using Zenject;
using DG.Tweening;
using Yg.GameData.Units;

namespace Yg.UI
{
    public class RecruitmentChoiceUI : TooltipHolderUI, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action<RecruitmentChoiceUI> OnSelected;

        [CustomHeader("Settings")]
        [SerializeField] private Image _squadImage;
        [SerializeField] private TextMeshProUGUI _squadNameText;
        [SerializeField] private TextMeshProUGUI _squadSizeText;
        [SerializeField] private RectTransform _perkHolder;
        [SerializeField] private RectTransform _spellHolder;
        [SerializeField] private PerkUI _perkUIPrefab;
        [SerializeField] private SpellUI _spellUIPrefab;

        [CustomHeader("DOTween Settings")]
        [SerializeField] private float _selectionScaleGoal;
        [SerializeField] private float _animationDuration;

        private List<PerkUI> _perkUILits = new();
        private List<SpellUI> _spellUIList = new();
        private UnitDataSO _unitDataSO;
        private DiContainer _container;
        private Sequence _selectionSequence;

        private bool _selected = false;

        public UnitDataSO UnitData => _unitDataSO;

        public void Initialize(DiContainer container, UnitDataSO unitDataSO)
        {
            _container = container;
            _unitDataSO = unitDataSO;
            _squadImage.sprite = _unitDataSO.Icon;
            _squadNameText.text = _unitDataSO.Name;
            _squadSizeText.text = _unitDataSO.DefaultSquadSize.ToString();

            CreatePerkUIs();
            CreateSpellUIs();
        }

        private void OnDestroy()
        {
            _selectionSequence?.Complete();
        }

        public void PlayUnselectionAnimation()
        {
            if (_selectionSequence is not null) _selectionSequence.Kill();

            _selectionSequence = DOTween.Sequence();
            _selectionSequence
                .Append(transform.DOScale(Vector3.one, _animationDuration))
                .OnComplete(() => _selectionSequence = null);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            PlaySelectionAnimation();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _tooltipDrawer.HideUnitTooltip();

            if (!_selected)
                PlayUnselectionAnimation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _selected = true;
            _tooltipDrawer.HideUnitTooltip();
            _tooltipDrawer.HideTooltips();
            OnSelected?.Invoke(this);
        }

        protected override void ShowTooltip()
        {
            _tooltipDrawer.ShowTooltip(_unitDataSO);
        }

        private void PlaySelectionAnimation()
        {
            if (_selectionSequence is not null) _selectionSequence.Kill();

            _selectionSequence = DOTween.Sequence();
            _selectionSequence
                .Append(transform.DOScale(_selectionScaleGoal, _animationDuration))
                .OnComplete(() => _selectionSequence = null);
        }

        private void CreatePerkUIs()
        {
            for (int i = 0; i < _unitDataSO.PerkSOList.Count; i++)
            {
                PerkUI perkUI = _container.InstantiatePrefab(_perkUIPrefab, _perkHolder).GetComponent<PerkUI>();
                _perkUILits.Add(perkUI);

                perkUI.SetPerk(_unitDataSO.PerkSOList[i]);
            }
        }

        private void CreateSpellUIs()
        {
            for (int i = 0; i < _unitDataSO.SpellSOList.Count; i++)
            {
                SpellUI spellUI = _container.InstantiatePrefab(_spellUIPrefab, _spellHolder).GetComponent<SpellUI>();
                _spellUIList.Add(spellUI);

                spellUI.SetSpell(_unitDataSO.SpellSOList[i]);
            }
        }
    }
}
