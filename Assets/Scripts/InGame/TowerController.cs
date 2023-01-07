using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class TowerController : NetworkBehaviour
{
    [SerializeField] private Transform tower;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject canon;
    [SerializeField] private TankAudio tankAudio;
    [SerializeField] private ShellPool shellPool;
    [SerializeField] private TankStats tankStats;
    [SerializeField] private InGameUI inGameUI;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isShoting;
    [SerializeField] private int shotPower = 2000;
    [SerializeField] private float canonRotationSpeed = 0.05f;
    [SerializeField] private int maxCanonAngle = 20;
    [SerializeField] private int cdTime = 5;

    [Client]
    private void Update()
    {
        if (!isOwned) { return; }

        if (Input.GetMouseButton(0) && !isShoting)
        {
            StartCoroutine(ShotCD());
            CmdShot();
            tankAudio.PlayTankShot();

        }
    }


    public override void OnStartAuthority()
    {
        enabled = true;
        inGameUI = FindObjectOfType<InGameUI>();
        inGameUI.UpdateCDandShell(tankStats.ShellCount, false);
    }
    private IEnumerator ShotCD()
    {
        tankStats.DecreaseShellCount();
        inGameUI.UpdateCDandShell(tankStats.ShellCount, true);
        isShoting = true;
        yield return new WaitForSeconds(cdTime);
        isShoting = false;
        inGameUI.UpdateCDandShell(tankStats.ShellCount, false);
    }
    [Command]
    private void CmdShot()
    {
        if (tankStats.ShellCount > 0)
            
            RpcShot();
    }


    [ClientRpc]
    private void RpcShot()
    {
        GameObject bullet = shellPool.shellPool.Find(x => x.activeSelf == false);
        bullet.SetActive(true);
        bullet.transform.position = weapon.transform.position;
        bullet.transform.localRotation = weapon.transform.rotation;
        //shellPool.shellPool.RemoveAt(0);
        var shell = bullet.GetComponent<Shell>();
        shell.TeamOwner = tankStats.Team;
        shell.hasCollision = false;

        bullet.GetComponent<Rigidbody>().AddRelativeForce(weapon.transform.forward * -shotPower, ForceMode.Impulse);
    }


    public void RotateTower(Vector3 direction)
    {
        tower.transform.eulerAngles += direction;
    }
    public void RotateWeapon(Vector3 direction)
    {
        if (canon.transform.localEulerAngles.x + direction.x * canonRotationSpeed < maxCanonAngle)
            canon.transform.localEulerAngles += direction * canonRotationSpeed;
        if (canon.transform.localEulerAngles.x + direction.x * canonRotationSpeed > 360 - maxCanonAngle)
            canon.transform.localEulerAngles += direction * canonRotationSpeed;
    }
}
