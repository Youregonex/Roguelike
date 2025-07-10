using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yg.Character;
using Yg.GameData.Units;
using Zenject;

namespace Yg.UI
{
    public class Development_SquadAdderUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private Development_SquadUI _development_SquadUIPrefab;
        [SerializeField] private RectTransform _holder;
        [SerializeField] private RectTransform _window;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _mainMenuButton;

        private PlayerSpawner _playerSpawner;
        private PlayerWarbandComponent _playerWarbandComponent;
        private List<Development_SquadUI> _squadUIList = new();


        [Inject]
        private void Construct(PlayerSpawner playerSpawner)
        {
            _playerSpawner = playerSpawner;
        }

        private void Initialize()
        {
            List<UnitDataSO> unitDataSOList = ResourceLoader.SO_UnitDataSOList;

            for (int i = 0; i < unitDataSOList.Count; i++)
            {
                Development_SquadUI development_SquadUI = Instantiate(_development_SquadUIPrefab, _holder);
                development_SquadUI.OnSquadCreationRequested += Development_SquadUI_OnSquadCreationRequested;
                development_SquadUI.AssignData(unitDataSOList[i]);
            }
        }

        private void Development_SquadUI_OnSquadCreationRequested(UnitDataSO unitDataSO)
        {
            if(_playerWarbandComponent is null)
                _playerWarbandComponent = _playerSpawner.PlayerCore.GetCharacterComponent<PlayerWarbandComponent>();

            _playerWarbandComponent.AddSquad(unitDataSO);
        }

        private void Awake()
        {
            Initialize();
            _window.gameObject.SetActive(false);

            _exitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });

            _mainMenuButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
            });
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                _window.gameObject.SetActive(!_window.gameObject.activeInHierarchy);
            }
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _squadUIList.Count; i++)
                _squadUIList[i].OnSquadCreationRequested -= Development_SquadUI_OnSquadCreationRequested;
        }
    }
}