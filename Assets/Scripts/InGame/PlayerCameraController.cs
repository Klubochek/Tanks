using Cinemachine;
using Mirror;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Tanks.Input
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [SerializeField] private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
        [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
        [SerializeField] private Transform towerTransform;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private CinemachineVirtualCamera scopelCamera;
        [SerializeField] private TowerController towerController;
        [SerializeField] private GameObject aim;
        [SerializeField] private GameObject aimBg;


        private CinemachineTransposer transposer;
        private Controls controls;
        public InputAction input;
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
            aim = GameObject.FindGameObjectWithTag("Aim");
            aimBg = GameObject.FindGameObjectWithTag("Aimbg");
            aimBg.SetActive(false);
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            virtualCamera.gameObject.SetActive(true);
            enabled = true;

            
            TankControls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
            TankControls.Player.Scope.started += ctx => StartScope(true);
            TankControls.Player.Scope.canceled += ctx => StartScope(false); ;

        }

        private void StartScope(bool isScope)
        {
            if (isScope)
            {
                virtualCamera.gameObject.SetActive(false);
                scopelCamera.gameObject.SetActive(true);
                aim.SetActive(false);
                aimBg.SetActive(true);
            }
            else
            {
                virtualCamera.gameObject.SetActive(true);
                scopelCamera.gameObject.SetActive(false);

                aim.SetActive(true);
                aimBg.SetActive(false);
            }
                
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
        private void Look(Vector2 vector2)
        {
            if (transposer == null)
                Debug.Log("Null transposer");
                if (virtualCamera==null) Debug.Log("Null camera");
            if (transposer != null)
            {
                float deltaTime = Time.deltaTime;
                float folloOffset = Mathf.Clamp(
                    transposer.m_FollowOffset.y - (vector2.y * cameraVelocity.y * deltaTime),
                    maxFollowOffset.x,
                    maxFollowOffset.y);
                transposer.m_FollowOffset.y = folloOffset;

                
                towerController.RotateTower(new Vector3(0f, vector2.x * cameraVelocity.x * deltaTime, 0f));
                towerController.RotateWeapon(new Vector3(vector2.y, 0, 0));
            }   
        }
    }
}
