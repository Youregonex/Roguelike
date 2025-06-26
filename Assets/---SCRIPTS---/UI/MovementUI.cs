using UnityEngine;
using TMPro;

namespace Yg.UI
{
    public class MovementUI : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private TextMeshProUGUI _movementText;
        [SerializeField] private CanvasGroup _windowCanvasGroup;

        public void Show()
        {
            _windowCanvasGroup.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _windowCanvasGroup.gameObject.SetActive(false);
        }

        public void UpdateMoves(int movesLeft)
        {
            _movementText.text = movesLeft.ToString();
        }
    }
}