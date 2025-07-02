using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yg.Battle;
using Yg.Character;
using Yg.GameData;
using Zenject;
using System;
using System.Linq;

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
        [SerializeField] private Button _autoPlaceButton;

        [CustomHeader("Debug")]
        [SerializeField] private int _listAmount;

        private PersistentData _persistentData;
        private SquadUI _currentSquadUI;
        private SquadPlacementArea _currentSquadPlacementArea;

        private List<SquadUI> _squadUIList = new();

        private int _lastPlacementIndex = 0;

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

            _autoPlaceButton.onClick.AddListener(() =>
            {
                AutoPlaceTroops();
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

            _startBattleButton.interactable = _squadUIList.Count == 0;

            //Debug
            _listAmount = _squadUIList.Count;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _squadUIList.Count; i++)
            {
                if (_squadUIList[i] is null) continue;

                _squadUIList[i].OnSquadClicked -= SquadUI_OnSquadClicked;
                _squadUIList[i].OnSquadReleased -= SquadUI_OnSquadReleased;
            }

            for (int i = 0; i < _squadPlacementAreaList.Count; i++)
            {
                if (_squadPlacementAreaList[i] is null) continue;
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
                    ClearCurrentSelection();
                }
                else
                {
                    _currentSquadPlacementArea.SetSquadUI(_currentSquadUI);
                    squadPlacementArea.ClearSquadUI();
                    ClearCurrentSelection();
                }
            }
            else
            {
                _currentSquadUI?.Show();
                _squadUIList.Add(squadPlacementArea.SquadUI);
                squadPlacementArea.ClearSquadUI();
                ClearCurrentSelection();
            }

            UnhighlightPlacementAreas();
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
                if(!_currentSquadPlacementArea.Empty)
                {
                    SquadUI previousSquadUI = _currentSquadPlacementArea.SquadUI;
                    _currentSquadPlacementArea.SetSquadUI(squadUI);

                    _squadUIList.Remove(squadUI);
                    _squadUIList.Add(previousSquadUI);
                    previousSquadUI.Show();

                    ClearCurrentSelection();
                }
                else
                {
                    _currentSquadPlacementArea.SetSquadUI(squadUI);
                    _squadUIList.Remove(squadUI);

                    ClearCurrentSelection();
                }
            }
            else
            {
                squadUI.Show();
                ClearCurrentSelection();
            }

            UnhighlightPlacementAreas();
        }

        private void SquadUI_OnSquadClicked(SquadUI squadUI)
        {
            _currentSquadUI = squadUI;
            squadUI.Hide();
            HighlightPlacementArea();
        }

        private void AutoPlaceTroops()
        {
            ReturnTroopsToHand();

            _squadPlacementAreaList = _squadPlacementAreaList.OrderByDescending(e => e.transform.position.x).ToList();

            List<WarbandSlot> enemyWarband = _persistentData.BattleTransitionData.EnemyWarband;

            List<SquadUI> meleeSquads = _squadUIList.Where(e => e.WarbandSlot.UnitData.AttackType == EAttackType.Melee).ToList();
            List<SquadUI> rangeSquads = _squadUIList.Where(e => e.WarbandSlot.UnitData.AttackType == EAttackType.Ranged).ToList();

            for (int i = 0; i < meleeSquads.Count; i++)
            {
                if (i < _squadPlacementAreaList.Count)
                {
                    _squadPlacementAreaList[i].SetSquadUI(meleeSquads[i]);
                    _lastPlacementIndex = i + 1;
                }
                else
                {
                    Debug.LogError("Not enough placement areas for melee squads!");
                    return;
                }
            }

            int placementIndex = 0;
            for (int i = 0; i < rangeSquads.Count; i++)
            {
                placementIndex = i + _lastPlacementIndex;
                if (placementIndex < _squadPlacementAreaList.Count)
                    _squadPlacementAreaList[placementIndex].SetSquadUI(rangeSquads[i]);
                else
                {
                    Debug.LogError("Not enough placement areas for ranged squads!");
                    return;
                }
            }

            for (int i = 0; i < _squadUIList.Count; i++)
                _squadUIList[i].Hide();

            _squadUIList.Clear();
        }

        private void ReturnTroopsToHand()
        {
            for (int i = 0; i < _squadPlacementAreaList.Count; i++)
            {
                if (_squadPlacementAreaList[i].Empty) continue;
                SquadPlacementUI_OnRelease(_squadPlacementAreaList[i]);
            }
        }

        private void ClearCurrentSelection()
        {
            _currentSquadUI = null;
            _currentSquadPlacementArea = null;
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
