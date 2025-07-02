using UnityEngine;
using UnityEngine.UI;
using Yg.Battle.GameSystems;
using TMPro;

public class BattleTestUI : MonoBehaviour
{
    [CustomHeader("Settings")]
    [SerializeField] private BattleUnitSpawner _battleUnitSpawner;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _stopButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Slider _playerMeleeSlider;
    [SerializeField] private Slider _playerRangedSlider;
    [SerializeField] private Slider _enemyMeleeSlider;
    [SerializeField] private Slider _enemyRangedSlider;
    [SerializeField] private TextMeshProUGUI _pmaText;
    [SerializeField] private TextMeshProUGUI _praText;
    [SerializeField] private TextMeshProUGUI _emaText;
    [SerializeField] private TextMeshProUGUI _eraText;
    [SerializeField] private int _maxUnits;

    private void Awake()
    {
        _exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        _startButton.onClick.AddListener(() =>
        {
            int playerMeleeAmount = Mathf.RoundToInt(_playerMeleeSlider.value * _maxUnits);
            int playerRangedAmount = Mathf.RoundToInt(_playerRangedSlider.value * _maxUnits);
            int enemyMeleeAmount = Mathf.RoundToInt(_enemyMeleeSlider.value * _maxUnits);
            int enemyRangedAmount = Mathf.RoundToInt(_enemyRangedSlider.value * _maxUnits);

            _battleUnitSpawner.StartBattleTEST(playerMeleeAmount, playerRangedAmount, enemyMeleeAmount, enemyRangedAmount);
        });

        _playerMeleeSlider.onValueChanged.AddListener(x =>
        {
            _pmaText.text = Mathf.RoundToInt(x * _maxUnits).ToString();
        });

        _playerRangedSlider.onValueChanged.AddListener(x =>
        {
            _praText.text = Mathf.RoundToInt(x * _maxUnits).ToString();
        });

        _enemyMeleeSlider.onValueChanged.AddListener(x =>
        {
            _emaText.text = Mathf.RoundToInt(x * _maxUnits).ToString();
        });

        _enemyRangedSlider.onValueChanged.AddListener(x =>
        {
            _eraText.text = Mathf.RoundToInt(x * _maxUnits).ToString();
        });

        _stopButton.onClick.AddListener(() =>
        {
            _battleUnitSpawner.StopBattleTEST();
        });
    }
}
