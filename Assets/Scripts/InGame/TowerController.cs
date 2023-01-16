using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class TowerController : NetworkBehaviour
{
    [SerializeField] private Transform _tower;
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private GameObject _canon;
    [SerializeField] private TankAudio _tankAudio;
    [SerializeField] private ShellPool _shellPool;
    [SerializeField] private TankStats _tankStats;
    [SerializeField] private InGameUI _inGameUI;

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private bool _isShoting;
    [SerializeField] private int _shotPower = 2000;
    [SerializeField] private float _canonRotationSpeed = 0.05f;
    [SerializeField] private int _maxCanonAngle = 20;
    [SerializeField] private int _cdTime = 5;

    [Client]
    private void Update()
    {
        if (!isOwned) { return; }

        if (Input.GetMouseButton(0) && !_isShoting)
        {
            StartCoroutine(ShotCD());
            CmdShot();
            _tankAudio.PlayTankShot();

        }
    }


    public override void OnStartAuthority()
    {
        enabled = true;
        _inGameUI = FindObjectOfType<InGameUI>();
        _inGameUI.UpdateCDandShell(_tankStats.ShellCount, false);
    }
    private IEnumerator ShotCD()
    {
        _tankStats.DecreaseShellCount();
        _inGameUI.UpdateCDandShell(_tankStats.ShellCount, true);
        _isShoting = true;
        yield return new WaitForSeconds(_cdTime);
        _isShoting = false;
        _inGameUI.UpdateCDandShell(_tankStats.ShellCount, false);
    }
    [Command]
    private void CmdShot()
    {
        if (_tankStats.ShellCount > 0)
            
            RpcShot();
    }


    [ClientRpc]
    private void RpcShot()
    {
        GameObject bullet = _shellPool.shellPool.Find(x => x.activeSelf == false);
        bullet.SetActive(true);
        bullet.transform.position = _weapon.transform.position;
        bullet.transform.localRotation = _weapon.transform.rotation;
        var shell = bullet.GetComponent<Shell>();
        shell.TeamOwner = _tankStats.Team;
        shell.HasCollision = false;

        bullet.GetComponent<Rigidbody>().AddRelativeForce(_weapon.transform.forward * -_shotPower, ForceMode.Impulse);
    }


    public void RotateTower(Vector3 direction)
    {
        _tower.transform.eulerAngles += direction;
    }
    public void RotateWeapon(Vector3 direction)
    {
        if (_canon.transform.localEulerAngles.x + direction.x * _canonRotationSpeed < _maxCanonAngle)
            _canon.transform.localEulerAngles += direction * _canonRotationSpeed;
        if (_canon.transform.localEulerAngles.x + direction.x * _canonRotationSpeed > 360 - _maxCanonAngle)
            _canon.transform.localEulerAngles += direction * _canonRotationSpeed;
    }
}
