using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yg.Battle;
using Yg.Character;
using Yg.GameData;
using Zenject;
using System;

namespace Yg.UI
{
    public class SquadPlacementUI : MonoBehaviour
    {
        public event Action OnTroopsReady;

        [CustomHeader("Settings")]
        [SerializeField] private RectTransform _squadUIHolder;
        [SerializeField] private RectTransform _window;
        [SerializeField] private SquadUI _squadUIPrefab;
        [SerializeField] private SpriteRenderer _mouseSquadVisual;
        [SerializeField] private List<SquadPlacementArea> _squadPlacementAreaList;
        [SerializeField] private Button _startBattleButton;

        private PersistentData _persistentData;
        private List<SquadUI> _squadUIList = new();

        private SquadUI _currentSquadUI;
        private SquadPlacementArea _currentSquadPlacementArea;

        private Vector3 _mouseVisualPosition;


        [Inject]
        private void Construct(PersistentData persistentData)
        {
            _persistentData = persistentData;
        }

        public void Initialize()
        {
            CreateSquadUIs(_persistentData.BattleTransitionData.PlayerWarband);

            for (int i = 0; i < _squadPlacementAreaList.Count; i++)
            {
                _squadPlacementAreaList[i].OnHover += SquadPlacementArea_OnSelect;
                _squadPlacementAreaList[i].OnHoverEnd += SquadPlacementArea_OnDeselect;
                _squadPlacementAreaList[i].OnClick += SquadPlacementUI_OnClick;
                _squadPlacementAreaList[i].OnRelease += SquadPlacementUI_OnRelease;
            }
        }

        private void Awake()
        {
            Initialize();
            _startBattleButton.onClick.AddListener(() =>
            {
                _window.gameObject.SetActive(false);
                OnTroopsReady?.Invoke();
            });
        }

        private void Update()
        {
            _mouseVisualPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mouseVisualPosition.z = 0f;
            _mouseSquadVisual.transform.position = _mouseVisualPosition;

            if (_currentSquadUI is not null)
                _mouseSquadVisual.sprite = _currentSquadUI.WarbandSlot.UnitData.UnitIcon;
            else
                _mouseSquadVisual.sprite = null;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _squadUIList.Count; i++)
            {
                _squadUIList[i].OnSquadClicked -= SquadUI_OnSquadClicked;
                _squadUIList[i].OnSquadReleased -= SquadUI_OnSquadReleased;
            }

            for (int i = 0; i < _squadPlacementAreaList.Count; i++)
            {
                _squadPlacementAreaList[i].OnHover -= SquadPlacementArea_OnSelect;
                _squadPlacementAreaList[i].OnHoverEnd -= SquadPlacementArea_OnDeselect;
                _squadPlacementAreaList[i].OnClick -= SquadPlacementUI_OnClick;
                _squadPlacementAreaList[i].OnRelease -= SquadPlacementUI_OnRelease;
            }
        }

        private void CreateSquadUIs(List<WarbandSlot> warband)
        {
            for (int i = 0; i < warband.Count; i++)
            {
                if (warband[i].UnitData is null) continue;

                SquadUI squadUI = Instantiate(_squadUIPrefab, _squadUIHolder);
                squadUI.Initialize(warband[i]);
                squadUI.OnSquadClicked += SquadUI_OnSquadClicked;
                squadUI.OnSquadReleased += SquadUI_OnSquadReleased;
                _squadUIList.Add(squadUI);
            }
        }

        private void SquadPlacementUI_OnRelease(SquadPlacementArea squadPlacementArea)
        {
            if(_currentSquadPlacementArea is not null)
            {
                if (!_currentSquadPlacementArea.Empty)
                {
                    SquadUI previousSquadUI = _currentSquadPlacementArea.SquadUI;
                    _currentSquadPlacementArea.SetSquadUI(squadPlacementArea.SquadUI);
                    squadPlacementArea.SetSquadUI(previousSquadUI);
                    _currentSquadPlacementArea = null;
                    _currentSquadUI = null;
                }
                else
                {
                    _currentSquadPlacementArea.SetSquadUI(_currentSquadUI);
                    squadPlacementArea.ClearSquadUI();
                    _currentSquadPlacementArea = null;
                    _currentSquadUI = null;
                }

                UnhighlightPlacementAreas();
            }
            else
            {
                _currentSquadUI?.Show();
                squadPlacementArea.ClearSquadUI();
                _currentSquadUI = null;
                _currentSquadPlacementArea = null;
                UnhighlightPlacementAreas();
            }
        }

        private void SquadPlacementUI_OnClick(SquadPlacementArea squadPlacementArea)
        {
            if (squadPlacementArea.Empty) return;

            _currentSquadUI = squadPlacementArea.SquadUI;
            HighlightPlacementArea();
        }

        private void SquadPlacementArea_OnSelect(SquadPlacementArea squadPlacementArea)
        {
            _currentSquadPlacementArea = squadPlacementArea;
        }

        private void SquadPlacementArea_OnDeselect(SquadPlacementArea squadPlacementArea)
        {
            if (_currentSquadPlacementArea is not null && _currentSquadPlacementArea == squadPlacementArea)
                _currentSquadPlacementArea = null;
        }

        private void SquadUI_OnSquadReleased(SquadUI squadUI)
        {
            if (_currentSquadPlacementArea is not null)
            {
                _currentSquadPlacementArea.SetSquadUI(squadUI);
                _currentSquadUI = null;
                _currentSquadPlacementArea = null;

                UnhighlightPlacementAreas();

                _squadUIList.Remove(squadUI);
            }
            else
            {
                _currentSquadUI = null;
                squadUI.Show();
                UnhighlightPlacementAreas();
            }
        }

        private void SquadUI_OnSquadClicked(SquadUI squadUI)
        {
            _currentSquadUI = squadUI;
            squadUI.Hide();
            HighlightPlacementArea();
        }

        private void UnhighlightPlacementAreas()
        {
            for (int i = 0; i < _squadPlacementAreaList.Count; i++)
                _squadPlacementAreaList[i].DefaultUnhighlight();
        }

        private void HighlightPlacementArea()
        {
            for (int i = 0; i < _squadPlacementAreaList.Count; i++)
            {
                if (_squadPlacementAreaList[i].Empty)
                    _squadPlacementAreaList[i].DefaultHighlight();
            }
        }
    }
}
