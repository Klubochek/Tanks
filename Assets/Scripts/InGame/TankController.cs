using UnityEngine;
using Mirror;
using System;

namespace Tanks.Input
{


    public class TankController : NetworkBehaviour
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float speedForward;
        [SerializeField] private float speedBack;
        [SerializeField] private Rigidbody tankbody;
        [SerializeField] private TankAudio tankAudio;


        private bool IsMoving = false;
        private Vector2 moveDirection;
        private Controls controls;
        private Controls TankControls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new Controls();
            }
        }

        public override void OnStartAuthority()
        {
            Cursor.visible = false;
            enabled = true;
            TankControls.Player.WASD.performed += ctx => SetMoveDirection(ctx.ReadValue<Vector2>());
            TankControls.Player.WASD.canceled += ctx => ResetMoveDirection();
            TankControls.Player.Cursor.performed += ctx => ShowCursor();
            TankControls.Player.Cursor.canceled += ctx => HideCursor();
        }
        private void HideCursor()
        {
            Cursor.visible=false;
        }

        private void ShowCursor()
        {
            Cursor.visible = true;
        }

        private void SetMoveDirection(Vector2 vector2)
        {
            moveDirection = vector2;
        }

        private void ResetMoveDirection()
        {
            moveDirection = Vector2.zero;
        }

        [ClientCallback]
        private void OnEnable()
        {
            TankControls.Enable();
        }
        [ClientCallback]
        private void OnDisable()
        {
            TankControls.Disable();
        }
        private void FixedUpdate()
        {
            if (!isOwned) { Debug.Log("NoAuth"); return; }

            if (Mathf.Abs(moveDirection.y) > 0)
            {
                MoveBody();
            }
            else IsMoving = false;
            if (Mathf.Abs(moveDirection.x) > 0)
            {
                RotateBody();
            }

        }

        private void MoveBody()
        {
            IsMoving = true;
            tankbody.AddRelativeForce(new Vector3(0, 0, moveDirection.y) * -speedBack);
            
        }

        private void RotateBody()
        {
            if(!IsMoving)
                tankbody.AddRelativeTorque(new Vector3(0, moveDirection.x, 0) * rotationSpeed,ForceMode.VelocityChange);
            else
                tankbody.AddRelativeTorque(new Vector3(0, moveDirection.x, 0) * rotationSpeed*0.8f,ForceMode.VelocityChange);
            
        }
    }
}