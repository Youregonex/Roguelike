using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yg.Character;
using System;
using UnityEngine.EventSystems;
using Zenject;
using DG.Tweening;

namespace Yg.UI
{
    public class RecruitmentChoiceUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
        private WarbandSlot _warbandSlot;
        private DiContainer _container;
        private Sequence _selectionSequence;

        private bool _selected = false;

        public WarbandSlot WarbandSlot => _warbandSlot;

        public void Initialize(DiContainer container, WarbandSlot warbandSlot)
        {
            _container = container;
            _warbandSlot = warbandSlot;
            _squadImage.sprite = _warbandSlot.UnitData.UnitIcon;
            _squadNameText.text = _warbandSlot.UnitData.UnitName;
            _squadSizeText.text = _warbandSlot.UnitData.DefaultSquadSize.ToString();

            CreatePerkUIs();
            CreateSpellUIs();
        }

        private void OnDestroy()
        {
            _selectionSequence?.Complete();
        }

        private void CreatePerkUIs()
        {
            for (int i = 0; i < _warbandSlot.UnitData.PerkList.Count; i++)
            {
                PerkUI perkUI = _container.InstantiatePrefab(_perkUIPrefab, _perkHolder).GetComponent<PerkUI>();
                _perkUILits.Add(perkUI);

                perkUI.SetPerk(_warbandSlot.UnitData.PerkList[i]);
            }
        }

        private void CreateSpellUIs()
        {
            for (int i = 0; i < _warbandSlot.UnitData.SpellSOList.Count; i++)
            {
                SpellUI spellUI = _container.InstantiatePrefab(_spellUIPrefab, _spellHolder).GetComponent<SpellUI>();
                _spellUIList.Add(spellUI);

                spellUI.SetSpell(_warbandSlot.UnitData.SpellSOList[i]);
            }
        }

        public void PlayUnselectionAnimation()
        {
            if (_selectionSequence is not null) _selectionSequence.Kill();

            _selectionSequence = DOTween.Sequence();
            _selectionSequence
                .Append(transform.DOScale(Vector3.one, _animationDuration))
                .OnComplete(() => _selectionSequence = null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySelectionAnimation();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!_selected)
                PlayUnselectionAnimation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _selected = true;
            OnSelected?.Invoke(this);
        }

        private void PlaySelectionAnimation()
        {
            if (_selectionSequence is not null) _selectionSequence.Kill();

            _selectionSequence = DOTween.Sequence();
            _selectionSequence
                .Append(transform.DOScale(_selectionScaleGoal, _animationDuration))
                .OnComplete(() => _selectionSequence = null);
        }
    }
}
