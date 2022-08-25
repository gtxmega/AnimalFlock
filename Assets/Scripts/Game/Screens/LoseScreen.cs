using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game.Screens
{
    public class LoseScreen : MonoBehaviour
    {
        [SerializeField] private Canvas _rootCanvas;
        [SerializeField] private GameObject _screen;
        private IGameEventsListener _gameEventsListener;

        [Inject]
        private void Construct(IGameEventsListener gameEventsListener)
        {
            _gameEventsListener = gameEventsListener;

            _gameEventsListener.LevelLose += OnLevelLose;
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(0);
        }

        private void OnLevelLose()
        {
            _rootCanvas.enabled = true;
            _screen.SetActive(true);
        }
    }
}