using System.Collections.Generic;
using UnityEngine;
using Yg.GameData.Equipment;
using Yg.GameData.Units;
using Yg.Systems;
using Yg.UI;
using Zenject;

namespace Yg.Character
{
    public class PlayerWarbandComponent : CharacterWarbandComponent
    {
        private const int MAX_ARMORY_SLOTS = 3;

        private UnitSelectionGenerator _unitSelectionGenerator;
        private EquipmentBuilder _equipmentBuilder;
        private PlayerMovementComponent _playerMovementComponent;

        private List<EquipmentData> _armoryList = new(MAX_ARMORY_SLOTS);

        private WarbandUI _warbandUI;
        private RecruitmentUI _recruitmentUI;

        [SerializeField] private EquipmentUI _hoveredEquipmentUI;
        [SerializeField] private EquipmentData _currentEquipmentData;
        [SerializeField] private EquipmentUI _lastClickedEquipmentUI;

        [Inject]
        private void Construct(WarbandUI warbandUI, RecruitmentUI recruitmentUI, EquipmentBuilder equipmentBuilder)
        {
            _warbandUI = warbandUI;
            _warbandUI.OnEquipmentUIHovered += WarbandUI_OnEquipmentUIHovered;
            _warbandUI.OnEquipmentUIHoverEnd += WarbandUI_OnEquipmentUIHoverEnd;

            _recruitmentUI = recruitmentUI;
            _equipmentBuilder = equipmentBuilder;
        }

        private void OnDestroy()
        {
            _warbandUI.OnEquipmentUIHovered -= WarbandUI_OnEquipmentUIHovered;
            _warbandUI.OnEquipmentUIHoverEnd -= WarbandUI_OnEquipmentUIHoverEnd;
        }

        private void WarbandUI_OnEquipmentUIHovered(EquipmentUI equipmentUI)
        {
            _hoveredEquipmentUI = equipmentUI;
        }

        private void WarbandUI_OnEquipmentUIHoverEnd(EquipmentUI EquipmentUI)
        {
            _hoveredEquipmentUI = null;
        }

        public override void InitializeComponent(CharacterCore characterCore)
        {
            base.InitializeComponent(characterCore);

            _unitSelectionGenerator = new();
            _playerMovementComponent = _characterCore.GetCharacterComponent<PlayerMovementComponent>();
            InitializeWarbandUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                UnitDataSO unitDataSO = _unitSelectionGenerator.GenerateRandomUnitChoiceList(1)[0];
                AddSquad(unitDataSO);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                for (int i = 0; i < _warbandSlotList.Count; i++)
                    RemoveSquad(_warbandSlotList[i]);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                int testTier = UnityEngine.Random.Range(1, 4 + 1);
                AddEquipmentSlotToFirstEmptySlot(_equipmentBuilder.BuildEquipment(testTier));
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                MousePress();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                MouseRelease();
            }
        }

        private void MousePress()
        {
            if (_hoveredEquipmentUI == null || _hoveredEquipmentUI.EquipmentData == null) return;

            _currentEquipmentData = _hoveredEquipmentUI.EquipmentData;
            _warbandUI.EnableMouseObject(_currentEquipmentData.Icon);
            _lastClickedEquipmentUI = _hoveredEquipmentUI;

            WarbandSlot warbandSlot = _hoveredEquipmentUI.WarbandSlotUI.WarbandSlot;
            int slotIndex = _hoveredEquipmentUI.WarbandSlotUI.GetIndexOfEquipmentUI(_hoveredEquipmentUI);

            warbandSlot.RemoveEquipmentDataFromSlot(slotIndex);
            _warbandUI.UpdateSlotsEquipmentData();
        }

        private void MouseRelease()
        {
            if (_currentEquipmentData == null || _currentEquipmentData.IsEmpty) return;
            WarbandSlot warbandSlot;
            int slotIndex;

            if (_hoveredEquipmentUI == null)
            {
                warbandSlot = _lastClickedEquipmentUI.WarbandSlotUI.WarbandSlot;
                slotIndex = _lastClickedEquipmentUI.WarbandSlotUI.GetIndexOfEquipmentUI(_lastClickedEquipmentUI);

                warbandSlot.AddEquipmentDataToSlot(slotIndex, _currentEquipmentData);
            }

            if(_hoveredEquipmentUI != null && _hoveredEquipmentUI.EquipmentData == null)
            {
                warbandSlot = _hoveredEquipmentUI.WarbandSlotUI.WarbandSlot;
                slotIndex = _hoveredEquipmentUI.WarbandSlotUI.GetIndexOfEquipmentUI(_hoveredEquipmentUI);

                warbandSlot.AddEquipmentDataToSlot(slotIndex, _currentEquipmentData);
            }

            if (_hoveredEquipmentUI != null && _hoveredEquipmentUI.EquipmentData != null)
            {
                warbandSlot = _hoveredEquipmentUI.WarbandSlotUI.WarbandSlot;
                slotIndex = _hoveredEquipmentUI.WarbandSlotUI.GetIndexOfEquipmentUI(_hoveredEquipmentUI);

                EquipmentData hoveredEquipmentData = _hoveredEquipmentUI.EquipmentData;

                warbandSlot.AddEquipmentDataToSlot(slotIndex, _currentEquipmentData);

                WarbandSlot lastClickedWarbandSlot = _lastClickedEquipmentUI.WarbandSlotUI.WarbandSlot;
                int lastClickedIndex = _lastClickedEquipmentUI.WarbandSlotUI.GetIndexOfEquipmentUI(_lastClickedEquipmentUI);

                lastClickedWarbandSlot.AddEquipmentDataToSlot(lastClickedIndex, hoveredEquipmentData);
            }

            _currentEquipmentData = null;
            _lastClickedEquipmentUI = null;

            _warbandUI.DisableMouseObject();
            _warbandUI.UpdateSlotsEquipmentData();
        }

        public void InitiateUnitSelction()
        {
            int optionsAmount = 3;
            List<UnitDataSO> options = _unitSelectionGenerator.GenerateRandomUnitChoiceList(optionsAmount);
            _recruitmentUI.Show(options);
            _recruitmentUI.OnChoiceMade += RecruitmentUI_OnChoiceMade;
            _playerMovementComponent.LockMovement();
        }

        public override void AddWarbandSlot(WarbandSlot warbandSlot)
        {
            base.AddWarbandSlot(warbandSlot);
            _warbandUI.UpdateSlotsUnitData();
        }

        public override bool AddSquad(UnitDataSO unitDataSO)
        {
            for (int i = 0; i < _warbandSlotList.Count; i++)
            {
                if (!_warbandSlotList[i].UnitEmpty) continue;

                _warbandSlotList[i].UpdateUnitData(unitDataSO);
                _warbandUI.UpdateSlotsUnitData();
                return true;
            }

            Debug.Log($"Not enough space in warband for {unitDataSO.Name}");
            return false;
        }

        public override void RemoveSquad(WarbandSlot warbandSlot)
        {
            warbandSlot.RemoveUnit();
            _warbandUI.UpdateSlotsUnitData();
        }

        public override void AddEmptyWarbandSlot()
        {
            base.AddEmptyWarbandSlot();
            _warbandUI.UpdateSlotsUnitData();
        }

        public override void RemoveWarbandSlot()
        {
            base.RemoveWarbandSlot();
            _warbandUI.UpdateSlotsUnitData();
        }

        private void RecruitmentUI_OnChoiceMade(UnitDataSO unitDataSO)
        {
            _recruitmentUI.OnChoiceMade -= RecruitmentUI_OnChoiceMade;

            if (AddSquad(unitDataSO))
            {
                _recruitmentUI.Hide();
                _playerMovementComponent.UnlockMovement();
            }
            else
            {
                _recruitmentUI.OnReplaceChoiceMade += RecruitmentUI_OnReplaceChoiceMade;
                _recruitmentUI.CurrentWarbandSlotReplacement();
            }
        }

        private void RecruitmentUI_OnReplaceChoiceMade(WarbandSlot replaceSlot, UnitDataSO unitDataSO)
        {
            _recruitmentUI.OnReplaceChoiceMade -= RecruitmentUI_OnReplaceChoiceMade;
            ReplaceWarbandSlotSquad(replaceSlot, unitDataSO);
            _recruitmentUI.Hide();
            _playerMovementComponent.UnlockMovement();
        }

        protected override void ReplaceWarbandSlotSquad(WarbandSlot replaceSlot, UnitDataSO unitDataSO)
        {
            base.ReplaceWarbandSlotSquad(replaceSlot, unitDataSO);
            _warbandUI.UpdateSlotsUnitData();
        }

        protected override void AddEquipmentSlotToFirstEmptySlot(EquipmentData equipmentData)
        {
            for (int i = 0; i < _warbandSlotList.Count; i++)
            {
                if (!_warbandSlotList[i].CanFitNewEqupment) continue;

                _warbandSlotList[i].AddEquipmentDataToFirstEmptySlot(equipmentData);
                _warbandUI.UpdateSlotsEquipmentData();
                return;
            }

            for (int i = 0; i < _armoryList.Count; i++)
            {
                if (_armoryList[i].IsEmpty) continue;

                _armoryList[i] = equipmentData;
                return;
            }

            Debug.Log($"There is no space in armory for item: {equipmentData.Name}");
        }

        private void InitializeWarbandUI()
        {
            for (int i = 0; i < _warbandSlotList.Count; i++)
                _warbandUI.CreateWarbandSlotUIs(_warbandSlotList[i]);
        }
    }
}
