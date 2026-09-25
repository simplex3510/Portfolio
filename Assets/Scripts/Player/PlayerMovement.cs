using UnityEngine;
using UnityEngine.InputSystem;

namespace Portfolio.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference _moveActRef;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _groundedVelocity = -2f;

        private CharacterController _controller;
        private InputAction _moveAction;
        private Vector3  _velocity;

#region Unity Callbacks
        private void Awake()
        {
            _controller = GetComponent<CharacterController>();

            if (_moveActRef == null)
            {
                Debug.LogError($"{nameof(PlayerMovement)} on {name}: {nameof(_moveActRef)} is not assigned.", this);
                enabled = false;
                return;
            }

            _moveAction = _moveActRef.action;
        }

        private void OnEnable()
        {
            _moveAction?.Enable();
        }

        private void OnDisable()
        {
            _moveAction?.Disable();
        }

        private void Update()
        {
            UpdateVelocity();

            _controller.Move(_velocity*Time.deltaTime);
        }
#endregion

        private void UpdateVelocity()
        {
            UpdateVelocityXAxis();
            UpdateVelocityYAxis();
        }

        private void UpdateVelocityXAxis()
        {
            Vector2 input = _moveAction.ReadValue<Vector2>();
            _velocity.x = input.x * _moveSpeed;
            _velocity.z = input.y * _moveSpeed;
        }

        private void UpdateVelocityYAxis()
        {
            if (_controller.isGrounded && _velocity.y < 0f)
            {
                _velocity.y = _groundedVelocity;
            }
            else
            {
                _velocity.y += _gravity * Time.deltaTime;
            }
        }
    }
}
