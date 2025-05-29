using UnityEngine;

namespace Y.MapGeneration
{
    public class BaseTile : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private GameObject _tileHoverHighlight;

        private Vector2Int _origin;
        private ETileType _tileType;
        
        public void Initialize(Vector2Int origin, ETileType tileType)
        {
            _origin = origin;
            _tileType = tileType;

            _tileHoverHighlight.gameObject.SetActive(false);
        }

        private void OnMouseEnter()
        {
            _tileHoverHighlight.gameObject.SetActive(true);
        }

        private void OnMouseExit()
        {
            _tileHoverHighlight.gameObject.SetActive(false);
        }
    }
}
