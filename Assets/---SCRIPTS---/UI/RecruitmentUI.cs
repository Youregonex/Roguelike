using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Yg.Character;
using System;
using Zenject;
using DG.Tweening;
using TMPro;
using Yg.GameData.Units;

namespace Yg.UI
{
    public class RecruitmentUI : MonoBehaviour
    {
        public event Action<UnitDataSO> OnChoiceMade;
        public event Action<WarbandSlot, UnitDataSO> OnReplaceChoiceMade;

        [CustomHeader("Settings")]
        [SerializeField] private RectTransform _windowRectTransform;
        [SerializeField] private RecruitmentChoiceUI _recruitmentChoiceUIPrefab;
        [SerializeField] private RectTransform _recruitmentChoiceUIHolder;
        [SerializeField] private TextMeshProUGUI _titleText;

        [CustomHeader("DOTween Settings")]
        [SerializeField] private float _windowOpenAnimationDuration;
        [SerializeField] private float _elementsFadeInDuration;
        [SerializeField] private float _choicesCreationDelay;

        private List<RecruitmentChoiceUI> _recruitmentChoiceUIList = new();
        private RecruitmentChoiceUI _currentChoice;
        private DiContainer _container;
        private Sequence _openAnimationSequence;
        private WarbandUI _warbandUI;

        private float _originalWindowHeight;

        [Inject]
        private void Contruct(DiContainer container, WarbandUI warbandUI)
        {
            _container = container;
            _warbandUI = warbandUI;
        }

        private void Awake()
        {
            _windowRectTransform.gameObject.SetActive(false);
            _originalWindowHeight = _windowRectTransform.sizeDelta.y;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _recruitmentChoiceUIList.Count; i++)
                _recruitmentChoiceUIList[i].OnSelected -= RecruitmentChoiceUI_OnSelected;

            _openAnimationSequence?.Complete();
            StopAllCoroutines();
        }

        public void Show(List<UnitDataSO> _selectionChoices)
        {
            _windowRectTransform.gameObject.SetActive(true);
            StartCoroutine(ShowSequence(_selectionChoices));
        }

        public void Hide()
        {
            ClearSelection();
            _windowRectTransform.gameObject.SetActive(false);
        }

        public void CurrentWarbandSlotReplacement()
        {
            _warbandUI.ScaleUp();

            foreach (var warbandSlotUI in _warbandUI.WarbandSlotUIList)
                warbandSlotUI.OnSelected += WarbandSlotUI_OnSelected;
        }

        private void WarbandSlotUI_OnSelected(WarbandSlotUI warbandSlotUI)
        {
            OnReplaceChoiceMade?.Invoke(warbandSlotUI.WarbandSlot, _currentChoice.UnitData);

            foreach (var warbandSlot in _warbandUI.WarbandSlotUIList)
                warbandSlotUI.OnSelected -= WarbandSlotUI_OnSelected;

            _warbandUI.ScaleDown();
        }

        private IEnumerator ShowSequence(List<UnitDataSO> _selectionChoices)
        {
            yield return StartCoroutine(PlayOpenAnimation());
            yield return StartCoroutine(CreateRecruitmentChoices(_selectionChoices));
        }

        private IEnumerator CreateRecruitmentChoices(List<UnitDataSO> _selectionChoices)
        {
            for (int i = 0; i < _selectionChoices.Count; i++)
            {
                RecruitmentChoiceUI recruitmentChoiceUI = _container.InstantiatePrefab(_recruitmentChoiceUIPrefab).GetComponent<RecruitmentChoiceUI>();
                recruitmentChoiceUI.transform.SetParent(_recruitmentChoiceUIHolder);
                recruitmentChoiceUI.transform.localScale = Vector3.one;
                _recruitmentChoiceUIList.Add(recruitmentChoiceUI);

                recruitmentChoiceUI.Initialize(_container, _selectionChoices[i]);
                recruitmentChoiceUI.OnSelected += RecruitmentChoiceUI_OnSelected;

                yield return new WaitForSeconds(_choicesCreationDelay);
            }
        }

        private IEnumerator PlayOpenAnimation()
        {
            _titleText.gameObject.SetActive(false);

            _openAnimationSequence = DOTween.Sequence();
            _openAnimationSequence
                .Append(_windowRectTransform
                    .DOSizeDelta(new Vector2(_windowRectTransform.sizeDelta.x, _originalWindowHeight), _windowOpenAnimationDuration)
                    .From(new Vector2(_windowRectTransform.sizeDelta.x, 0f)));

            yield return _openAnimationSequence.WaitForCompletion();

            _titleText.gameObject.SetActive(true);

            _openAnimationSequence = DOTween.Sequence();
            _openAnimationSequence
                .Append(_titleText.DOFade(1f, _elementsFadeInDuration).From(0f));
        }

        private void ClearSelection()
        {
            for (int i = 0; i < _recruitmentChoiceUIList.Count; i++)
                Destroy(_recruitmentChoiceUIList[i].gameObject);
            
            _currentChoice = null;
            _recruitmentChoiceUIList.Clear();
        }

        private void RecruitmentChoiceUI_OnSelected(RecruitmentChoiceUI currentChoice)
        {
            if (currentChoice == _currentChoice) return;

            foreach (var recruitmentChoice in _recruitmentChoiceUIList)
            {
                if (recruitmentChoice == currentChoice) continue;
                recruitmentChoice.gameObject.SetActive(false);
            }

            _currentChoice = currentChoice;
            OnChoiceMade?.Invoke(_currentChoice.UnitData);
        }
    }
}
