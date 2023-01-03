using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TankAudio : NetworkBehaviour
{
    [SerializeField] private AudioSource tankShot;
    [SerializeField] private AudioSource tankMove;
    [SerializeField] private AudioClip tankIdleState;
    [SerializeField] private AudioClip tankMoveState;

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    private void Start()
    {
        PlayTankMove();
    }

    public void PlayTankIdle()
    {
        tankMove.clip = tankIdleState;
        tankMove.Play();
    }
    public void PlayTankMove()
    {
        tankMove.clip = tankMoveState;
        tankMove.Play();
    }
    public void PlayTankShot()
    {
        tankShot.Play();
    }

}
