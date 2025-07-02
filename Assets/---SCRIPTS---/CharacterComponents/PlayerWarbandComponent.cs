using System.Collections.Generic;
using UnityEngine;
using Yg.Systems;
using Yg.UI;
using Zenject;

namespace Yg.Character
{
    public class PlayerWarbandComponent : CharacterWarbandComponent
    {
        private WarbandUI _warbandUI;
        private RecruitmentUI _recruitmentUI;
        private UnitSelectionGenerator _unitSelectionGenerator;
        private PlayerMovementComponent _playerMovementComponent;

        [Inject]
        private void Construct(WarbandUI warbandUI, RecruitmentUI recruitmentUI)
        {
            _warbandUI = warbandUI;
            _recruitmentUI = recruitmentUI;
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
                WarbandSlot warbandSlot = _unitSelectionGenerator.GenerateRandomUnitChoiceList(1)[0];
                AddSquad(warbandSlot);
                Debug.Log($"Added squad: {warbandSlot.UnitData.UnitName}");
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                for (int i = 0; i < _warbandSlotList.Count; i++)
                    RemoveSquad(_warbandSlotList[i]);
            }
        }

        public void InitiateUnitSelction()
        {
            int optionsAmount = 3;
            List<WarbandSlot> options = _unitSelectionGenerator.GenerateRandomUnitChoiceList(optionsAmount);
            _recruitmentUI.Show(options);
            _recruitmentUI.OnChoiceMade += RecruitmentUI_OnChoiceMade;
            _playerMovementComponent.LockMovement();
        }

        public override bool AddSquad(WarbandSlot warbandSlot)
        {
            if (warbandSlot.Empty)
            {
                Debug.LogError("Trying to add empty squad!");
                return false;
            }

            for (int i = 0; i < _warbandSlotList.Count; i++)
            {
                if (_warbandSlotList[i].Empty)
                {
                    _warbandSlotList[i].UpdateData(warbandSlot.UnitData, warbandSlot.SlotSize);
                    _warbandUI.UpdateSlotsData();
                    return true;
                }
            }

            Debug.Log("There is no free space in warband!");
            return false;
        }

        public override void RemoveSquad(WarbandSlot warbandSlot)
        {
            warbandSlot.ClearSlot();
            _warbandUI.UpdateSlotsData();
        }

        public override void AddWarbandSlot()
        {
            base.AddWarbandSlot();
            _warbandUI.UpdateSlotsData();
        }

        public override void RemoveWarbandSlot()
        {
            base.RemoveWarbandSlot();
            _warbandUI.UpdateSlotsData();
        }

        private void RecruitmentUI_OnChoiceMade(WarbandSlot warbandSlot)
        {
            _recruitmentUI.OnChoiceMade -= RecruitmentUI_OnChoiceMade;

            if (AddSquad(warbandSlot))
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

        private void RecruitmentUI_OnReplaceChoiceMade(WarbandSlot replaceSlot, WarbandSlot newSlot)
        {
            _recruitmentUI.OnReplaceChoiceMade -= RecruitmentUI_OnReplaceChoiceMade;
            ReplaceWarbandSlot(replaceSlot, newSlot);
            _recruitmentUI.Hide();
            _playerMovementComponent.UnlockMovement();
        }

        private void ReplaceWarbandSlot(WarbandSlot replaceSlot, WarbandSlot newSlot)
        {
            replaceSlot.UpdateData(newSlot.UnitData, newSlot.SlotSize);
            _warbandUI.UpdateSlotsData();
        }

        private void InitializeWarbandUI()
        {
            for (int i = 0; i < _warbandSlotList.Count; i++)
                _warbandUI.CreateWarbandSlotUI(_warbandSlotList[i]);
        }
    }
}
