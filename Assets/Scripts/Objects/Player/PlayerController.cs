using UnityEngine;

namespace MonsterFlow.Objects.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Range(0.1f, 2)] public float moveSpeedMultiplier;
        [SerializeField] public float deadZoneAngle;
        private Controls _controls;
        private Vector2 _halfScreenResolution;
        private int _moveDirection;
        private bool _overwriteLeft;
        private bool _overwriteRight;
        private float _rotationAmount;
        private int _turnValue;

        private void Awake()
        {
            _controls = new Controls();
        }

        private void Start()
        {
            moveSpeedMultiplier *= 360f;

            _halfScreenResolution = new Vector2(Screen.width * 0.5f, Screen.height);

#if UNITY_EDITOR || UNITY_STANDALONE
            SetupKeyboardInput();
#endif
        }

        private void Update()
        {
#if PLATFORM_IOS || PLATFORM_ANDROID
            HandleTouchInput();
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
            HandleKeyboardInput();
#endif
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                for (var i = 0; i < Input.touchCount; ++i)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        if (Input.GetTouch(i).position.x > _halfScreenResolution.x)
                            // A: Right screen part input = clockwise turn (-1)
                            _overwriteRight = true;
                        else if (Input.GetTouch(i).position.x <= _halfScreenResolution.x)
                            // B: Left screen part input = counter-clockwise turn (1)
                            _overwriteLeft = true;
                    }

                    if (Input.GetTouch(i).phase == TouchPhase.Ended || Input.GetTouch(i).phase == TouchPhase.Canceled)
                    {
                        if (Input.GetTouch(i).position.x > _halfScreenResolution.x)
                            // A: Right screen part input = clockwise turn (-1)
                            _overwriteRight = false;
                        else if (Input.GetTouch(i).position.x <= _halfScreenResolution.x)
                            // B: Left screen part input = counter-clockwise turn (1)
                            _overwriteLeft = false;
                    }

                    if (Input.GetTouch(i).position.x > _halfScreenResolution.x)
                    {
                        // A: Right screen part input = clockwise turn (-1)
                        _turnValue = -1;

                        if (_overwriteLeft)
                            _turnValue = 1;
                    }
                    else if (Input.GetTouch(i).position.x <= _halfScreenResolution.x)
                    {
                        // B: Left screen part input = counter-clockwise turn (1)
                        _turnValue = 1;

                        if (_overwriteRight)
                            _turnValue = -1;
                    }
                }
            }
            else
            {
                _turnValue = 0;
                _overwriteRight = false;
                _overwriteLeft = false;
            }

            Rotate(_turnValue);
        }

        private void Rotate(int direction)
        {
            var targetRotation = new Vector3(0f, 0f, direction * moveSpeedMultiplier * Time.deltaTime);
            transform.Rotate(targetRotation);
        }

        private void SetupKeyboardInput()
        {
            _controls.Standard.Move.performed += context =>
            {
                // x = RightArrow, y = LeftArrow
                var value = context.ReadValue<Vector2>();

                // If both pressed, invert current direction
                if (value.x + value.y > 1f) _moveDirection *= -1;

                // Otherwise turn in the direction that's still pressed
                else _moveDirection = value.x > 0 ? -1 : 1;
            };

            // Stop moving when both keys are not pressed
            _controls.Standard.Move.canceled += context => { _moveDirection = 0; };
        }

        private void HandleKeyboardInput()
        {
            var zAngle = 0f;
            if (_moveDirection != 0)
            {
                zAngle = moveSpeedMultiplier * _moveDirection * Time.deltaTime;
            }
            else if (Mathf.Abs(_rotationAmount) > deadZoneAngle)
            {
                zAngle = (_rotationAmount > 0 ? 1f : -1f) * moveSpeedMultiplier * Time.deltaTime;
                _rotationAmount -= zAngle;
            }

            transform.Rotate(0f, 0f, zAngle);
        }
    }
}