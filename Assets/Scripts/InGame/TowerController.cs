using Mirror;
using System;
using System.Collections;
using System.Linq;
using Tanks.Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerController : NetworkBehaviour
{
    [SerializeField] private Transform tower;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject canon;
    [SerializeField] private TankAudio tankAudio;
    [SerializeField] private ShellPool shellPool;
    [SerializeField] private TankStats tankStats;
     
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isShoting;
    [SerializeField] private int shotPower = 2000;
    [SerializeField] private float canonRotationSpeed = 0.05f;
    [SerializeField] private int maxCanonAngle = 20;

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
    private IEnumerator ShotCD()
    {
        
        isShoting = true;
        yield return new WaitForSeconds(5);
        isShoting = false;
    }
    [Command]
    private void CmdShot()
    {
        if(tankStats.ShellCount>0)
        RpcShot();
    }
    
    
    [ClientRpc]
    private void RpcShot()
    {
        GameObject bullet = shellPool.shellPool.Find(x=>x.activeSelf==false);
        bullet.SetActive(true);
        bullet.transform.position = weapon.transform.position;
        bullet.transform.localRotation = weapon.transform.rotation;
        shellPool.shellPool.RemoveAt(0);
        bullet.GetComponent<Shell>().TeamOwner = tankStats.Team;
        bullet.GetComponent<Rigidbody>().AddRelativeForce(weapon.transform.forward * -shotPower, ForceMode.Impulse);
    }
    

    public void RotateTower(Vector3 direction)
    {
        tower.transform.eulerAngles += direction;
    }
    public void RotateWeapon(Vector3 direction)
    {
        //Debug.Log(canon.transform.localEulerAngles.x - direction.x * canonRotationSpeed);
        if(canon.transform.localEulerAngles.x+direction.x*canonRotationSpeed<maxCanonAngle)
            canon.transform.localEulerAngles += direction * canonRotationSpeed;
        if(canon.transform.localEulerAngles.x + direction.x * canonRotationSpeed > 360 - maxCanonAngle)
            canon.transform.localEulerAngles += direction * canonRotationSpeed;
    }
}
