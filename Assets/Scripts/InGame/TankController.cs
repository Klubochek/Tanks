using Mirror;
using UnityEngine;

namespace Tanks.Input
{


    public class TankController : NetworkBehaviour
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody _tankbody;
        [SerializeField] private TankAudio _tankAudio;


        private bool _isMoving = false;
        private Vector2 _moveDirection;
        public Controls Controls;
        private Controls _tankControls
        {
            get
            {
                if (Controls != null) { return Controls; }
                return Controls = new Controls();
            }
        }
        [ClientCallback]
        private void OnEnable()
        {
            _tankControls.Enable();
        }
        [ClientCallback]
        private void OnDisable()
        {
            _tankControls.Disable();
        }
        public override void OnStartAuthority()
        {
            Cursor.visible = false;
            enabled = true;
            _tankControls.Player.WASD.performed += ctx => SetMoveDirection(ctx.ReadValue<Vector2>());
            _tankControls.Player.WASD.canceled += ctx => ResetMoveDirection();
            _tankControls.Player.Cursor.performed += ctx => ShowCursor();
            _tankControls.Player.Cursor.canceled += ctx => HideCursor();
        }
        private void HideCursor()
        {
            Cursor.visible = false;
        }

        private void ShowCursor()
        {
            Cursor.visible = true;
        }

        private void SetMoveDirection(Vector2 vector2)
        {
            _moveDirection = vector2;
        }

        private void ResetMoveDirection()
        {
            _moveDirection = Vector2.zero;
        }


        private void FixedUpdate()
        {
            if (!isOwned) { Debug.Log("NoAuth"); return; }

            if (Mathf.Abs(_moveDirection.y) > 0)
            {
                MoveBody();
            }
            else _isMoving = false;
            if (Mathf.Abs(_moveDirection.x) > 0)
            {
                RotateBody();
            }

        }

        private void MoveBody()
        {
            _isMoving = true;
            _tankbody.AddRelativeForce(new Vector3(0, 0, _moveDirection.y) * -_speed);

        }

        private void RotateBody()
        {
            if (!_isMoving)
                _tankbody.AddRelativeTorque(new Vector3(0, _moveDirection.x, 0) * _rotationSpeed, ForceMode.VelocityChange);
            else
                _tankbody.AddRelativeTorque(new Vector3(0, _moveDirection.x, 0) * _rotationSpeed * 0.8f, ForceMode.VelocityChange);

        }
    }
}