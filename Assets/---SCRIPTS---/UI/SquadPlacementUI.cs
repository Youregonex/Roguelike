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
        public event Action<List<WarbandSlot>, List<SquadPlacementArea>> OnAutoPlaceRequired;

        [CustomHeader("Settings")]
        [SerializeField] private RectTransform _squadUIHolder;
        [SerializeField] private RectTransform _window;
        [SerializeField] private SquadUI _squadUIPrefab;
        [SerializeField] private SpriteRenderer _mouseSquadVisual;
        [SerializeField] private List<SquadPlacementArea> _squadPlacementAreaList;
        [SerializeField] private Button _startBattleButton;
        [SerializeField] private Button _autoPlaceButton;
        [SerializeField] private Button _clearPlacementAreasButton;

        private SquadPlacementArea _currentSquadPlacementArea;
        private WarbandSlot _currentWarbandSlot;
        private SquadPlacementArea _lastClickedPlacementArea;
        private PersistentData _persistentData;

        private readonly List<SquadUI> _squadUIList = new();

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
                _squadPlacementAreaList[i].OnHover += SquadPlacementArea_OnHover;
                _squadPlacementAreaList[i].OnHoverEnd += SquadPlacementArea_OnHoverEnd;
                _squadPlacementAreaList[i].OnClick += SquadPlacementUI_OnClick;
            }

            _startBattleButton.onClick.AddListener(() =>
            {
                _window.gameObject.SetActive(false);
                OnTroopsReady?.Invoke();
            });

            _autoPlaceButton.onClick.AddListener(() =>
            {
                AutoPlaceSquads();
            });

            _clearPlacementAreasButton.onClick.AddListener(() =>
            {
                ClearPlacementAreas();
            });

            _currentWarbandSlot = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                HighlightEmptyAreas();

            if (Input.GetKeyUp(KeyCode.Mouse0))
                TryPlaceSlot();

            _mouseVisualPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mouseVisualPosition.z = 0f;
            _mouseSquadVisual.transform.position = _mouseVisualPosition;

            if (_currentWarbandSlot is not null)
                _mouseSquadVisual.sprite = _currentWarbandSlot?.UnitData?.Icon;
            else
                _mouseSquadVisual.sprite = null;

            _startBattleButton.interactable = _squadUIList.Count == 0;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _squadUIList.Count; i++)
            {
                if (_squadUIList[i] is null) continue;
                _squadUIList[i].OnSquadClicked -= SquadUI_OnSquadClicked;
            }

            for (int i = 0; i < _squadPlacementAreaList.Count; i++)
            {
                if (_squadPlacementAreaList[i] is null) continue;
                _squadPlacementAreaList[i].OnHover -= SquadPlacementArea_OnHover;
                _squadPlacementAreaList[i].OnHoverEnd -= SquadPlacementArea_OnHoverEnd;
                _squadPlacementAreaList[i].OnClick -= SquadPlacementUI_OnClick;
            }
        }

        private void CreateSquadUIs(List<WarbandSlot> warband)
        {
            for (int i = 0; i < warband.Count; i++)
                CreateSquadUI(warband[i]);
        }

        private void CreateSquadUI(WarbandSlot warbandSlot)
        {
            if (warbandSlot.UnitData is null) return;

            SquadUI squadUI = Instantiate(_squadUIPrefab, _squadUIHolder);
            squadUI.Initialize(warbandSlot);
            squadUI.OnSquadClicked += SquadUI_OnSquadClicked;
            _squadUIList.Add(squadUI);
        }

        private void DeleteSquadUI(SquadUI squadUI)
        {
            if (_squadUIList.Contains(squadUI))
            {
                _squadUIList.Remove(squadUI);
            }
            else
                Debug.Log("Not found");

            squadUI.OnSquadClicked -= SquadUI_OnSquadClicked;
            Destroy(squadUI.gameObject);
        }

        private void HighlightEmptyAreas()
        {
            if(_currentWarbandSlot is not null && !_currentWarbandSlot.UnitEmpty)
                HighlightNotEmptyPlacementAreas();
        }

        private void TryPlaceSlot()
        {
            if (_currentWarbandSlot is null || _currentWarbandSlot.UnitEmpty) return;

            // Return to hand
            if(Utilities.MouseOverUI() || _currentSquadPlacementArea is null)
            {
                CreateSquadUI(_currentWarbandSlot);
                _currentWarbandSlot = null;
                _lastClickedPlacementArea = null;
                Debug.Log("Returned to hand");
                UnhighlightPlacementAreas();
                return;
            }

            // Place on empty area
            if(_currentSquadPlacementArea is not null && _currentSquadPlacementArea.Empty)
            {
                _currentSquadPlacementArea.SetWarbandSlot(_currentWarbandSlot);
                _currentWarbandSlot = null;
                _lastClickedPlacementArea = null;
                Debug.Log("Placed on empty area");
                UnhighlightPlacementAreas();
                return;
            }

            // Place on populated area
            if (_currentSquadPlacementArea is not null && !_currentSquadPlacementArea.Empty)
            {
                // Swap witch last clicked area
                if(_lastClickedPlacementArea is not null)
                {
                    SwapAreaSlotWithLastClicked(_currentSquadPlacementArea);
                    _currentWarbandSlot = null;
                    _lastClickedPlacementArea = null;
                    Debug.Log("Swapped areas");
                }
                // Swap with hand
                else
                {
                    CreateSquadUI(_currentSquadPlacementArea.WarbandSlot);
                    _currentSquadPlacementArea.SetWarbandSlot(_currentWarbandSlot);
                    _currentWarbandSlot = null;
                    _lastClickedPlacementArea = null;
                    Debug.Log("Swapped with hand");
                }

                UnhighlightPlacementAreas();
                return;
            }
        }

        private void SquadPlacementUI_OnClick(SquadPlacementArea squadPlacementArea)
        {
            if (squadPlacementArea.Empty) return;

            _lastClickedPlacementArea = squadPlacementArea;
            PickWarbandSlotFromArea(squadPlacementArea);
        }

        private void PickWarbandSlotFromArea(SquadPlacementArea squadPlacementArea)
        {
            if (squadPlacementArea.Empty) return;

            _currentWarbandSlot = squadPlacementArea.WarbandSlot;
            squadPlacementArea.ClearSlot();
        }

        private void SquadPlacementArea_OnHover(SquadPlacementArea squadPlacementArea)
        {
            _currentSquadPlacementArea = squadPlacementArea;
        }

        private void SquadPlacementArea_OnHoverEnd(SquadPlacementArea squadPlacementArea)
        {
            _currentSquadPlacementArea = null;
        }

        private void SquadUI_OnSquadClicked(SquadUI squadUI)
        {
            PickWarbandSlotFromSquadUI(squadUI);
        }

        private void AutoPlaceSquads()
        {
            ClearPlacementAreas();
            OnAutoPlaceRequired?.Invoke(_persistentData.BattleTransitionData.PlayerWarband, _squadPlacementAreaList);
            DiscardHand();
        }

        private void ClearPlacementAreas()
        {
            for (int i = 0; i < _squadPlacementAreaList.Count; i++)
            {
                if (_squadPlacementAreaList[i].Empty) continue;

                CreateSquadUI(_squadPlacementAreaList[i].WarbandSlot);
                _squadPlacementAreaList[i].ClearSlot();
            }
        }

        private void DiscardHand()
        {
            for (int i = _squadUIList.Count - 1; i >= 0; i--)
                DeleteSquadUI(_squadUIList[i]);
        }

        private void PickWarbandSlotFromSquadUI(SquadUI squadUI)
        {
            _currentWarbandSlot = squadUI.WarbandSlot;
            DeleteSquadUI(squadUI);
        }

        private void SwapAreaSlotWithLastClicked(SquadPlacementArea releasedOnArea)
        {
            WarbandSlot preservedSlot = releasedOnArea.WarbandSlot;
            releasedOnArea.SetWarbandSlot(_currentWarbandSlot);
            _lastClickedPlacementArea.SetWarbandSlot(preservedSlot);
        }

        private void HighlightNotEmptyPlacementAreas()
        {
            foreach (var placementArea in _squadPlacementAreaList)
            {
                if (placementArea.Empty)
                    placementArea.DefaultHighlight();
            }
        }

        private void UnhighlightPlacementAreas()
        {
            foreach (var placementArea in _squadPlacementAreaList)
                placementArea.DefaultUnhighlight();
        }
    }
}
