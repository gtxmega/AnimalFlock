using Events;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Components.Rotate
{
    public class RotateObject : MonoBehaviour
    {
        public float _sensitivity;

        private Vector2 _startTouchPosition;
        private float _multiplyX;
        private float _multiplyY;

        private bool _levelEnd;

        private IGameEventsListener _gameventListener;


        [Inject]
        private void Construct(IGameEventsListener gameventListener)
        {
            _gameventListener = gameventListener;
            _gameventListener.LevelLose += OnLevelLose;
        }

        private void OnLevelLose()
        {
            _levelEnd = true;
        }

        private void Update()
        {
            if (_levelEnd) return;

            if(Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        _startTouchPosition = touch.position;

                        if (_startTouchPosition.y <= Screen.height / 2)
                        {
                            _multiplyX = 1.0f;
                        }else
                        {
                            _multiplyX = -1.0f;
                        }

                        if(_startTouchPosition.x <= Screen.width / 2)
                        {
                            _multiplyY = -1.0f;
                        }else
                        {
                            _multiplyY = 1.0f;
                        }

                        break;
                    case TouchPhase.Moved:

                        Vector2 touchPosition = touch.position;

                        Vector2 direction = (_startTouchPosition - touchPosition).normalized;

                        float deltaAngleX = direction.x * _sensitivity * _multiplyX * Time.deltaTime;
                        float deltaAngleY = direction.y * _sensitivity * _multiplyY * Time.deltaTime;

                        if(direction.x < -0.2f && direction.x > 0.2f)
                        {
                            transform.localRotation *= Quaternion.Euler(new Vector3(0.0f, deltaAngleX, 0.0f));
                        }else
                        {
                            transform.localRotation *= Quaternion.Euler(new Vector3(0.0f, deltaAngleY, 0.0f));
                        }

                        _startTouchPosition = touchPosition;

                        break;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Ended:

                        _multiplyX = 0.0f;
                        _multiplyY = 0.0f;

                        break;
                    case TouchPhase.Canceled:
                        break;

                }
            }
        }
    }
}