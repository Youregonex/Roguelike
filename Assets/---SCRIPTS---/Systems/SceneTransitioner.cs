using UnityEngine;
using UnityEngine.SceneManagement;

namespace Yg.SceneTransitions
{
    public class SceneTransitioner : MonoBehaviour
    {
        [CustomHeader("Settings")]
        [SerializeField] private string _transisionToSceneName;

        public void StartTransition()
        {
            SceneManager.LoadScene(_transisionToSceneName);
        }
    }
}
