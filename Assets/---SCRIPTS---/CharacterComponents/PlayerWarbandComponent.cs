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

        [Inject]
        private void Construct(WarbandUI warbandUI, RecruitmentUI recruitmentUI, EquipmentBuilder equipmentBuilder)
        {
            _warbandUI = warbandUI;
            _recruitmentUI = recruitmentUI;
            _equipmentBuilder = equipmentBuilder;
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

                _warbandSlotList[i].AddEquipmentData(equipmentData);
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
                _warbandUI.CreateWarbandSlotUI(_warbandSlotList[i]);
        }
    }
}
