using Cinemachine;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tanks.Input
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [SerializeField] private Vector2 _maxFollowOffset = new Vector2(-1f, 6f);
        [SerializeField] private Vector2 _cameraVelocity = new Vector2(4f, 0.25f);
        [SerializeField] private Transform _towerTransform;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private CinemachineVirtualCamera _scopelCamera;
        [SerializeField] private TowerController _towerController;
        [SerializeField] private GameObject _aimBg;
        [SerializeField] private CameraManager _cameraManager;
        [SerializeField] private TankStats _tankStats;


        private CinemachineTransposer _transposer;
        private Controls _controls;
        public InputAction input;
        public Controls TankControls
        {
            get
            {
                if (_controls != null) { return _controls; }
                return _controls = new Controls();
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

        public override void OnStartClient()
        {
            _cameraManager = FindObjectOfType<CameraManager>();
            _cameraManager.cameraObjs.Add(_virtualCamera.gameObject);
            if (isOwned)
            {
                _cameraManager.CurrentCamera = _virtualCamera.gameObject;
            }
        }
        public override void OnStartAuthority()
        {
            
            _aimBg = GameObject.FindGameObjectWithTag("Aimbg");
            _aimBg.SetActive(false);
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _virtualCamera.gameObject.SetActive(true);
            enabled = true;





            TankControls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
            TankControls.Player.Scope.started += ctx => StartScope(true);
            TankControls.Player.Scope.canceled += ctx => StartScope(false); ;
            TankControls.Player.SwitchCamera.performed += ctx => SwitchCamera();


            
        }
        public override void OnStopClient()
        {
            base.OnStopClient();
            _cameraManager.cameraObjs.Remove(_virtualCamera.gameObject);
        }
        private void SwitchCamera()
        {
            if (_tankStats.IsDead)
            {
                _cameraManager.SwitchCamera();
            }
        }

        private void StartScope(bool isScope)
        {
            if (isScope)
            {
                _virtualCamera.gameObject.SetActive(false);
                _scopelCamera.gameObject.SetActive(true);
                _aimBg.SetActive(true);
            }
            else
            {
                _virtualCamera.gameObject.SetActive(true);
                _scopelCamera.gameObject.SetActive(false);
                _aimBg.SetActive(false);
            }

        }
        private void Look(Vector2 vector2)
        {
            if (_transposer == null)
                Debug.Log("Null transposer");
            if (_virtualCamera == null) Debug.Log("Null camera");
            if (_transposer != null && !_tankStats.IsDead)
            {
                float deltaTime = Time.deltaTime;
                float folloOffset = Mathf.Clamp(
                    _transposer.m_FollowOffset.y - (vector2.y * _cameraVelocity.y * deltaTime),
                    _maxFollowOffset.x,
                    _maxFollowOffset.y);
                _transposer.m_FollowOffset.y = folloOffset;


                _towerController.RotateTower(new Vector3(0f, vector2.x * _cameraVelocity.x * deltaTime, 0f));
                _towerController.RotateWeapon(new Vector3(vector2.y, 0, 0));
            }
        }
        
    }
}
