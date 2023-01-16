using Mirror;
using UnityEngine;

public class TankAudio : NetworkBehaviour
{
    [SerializeField] private AudioSource _tankShot;
    [SerializeField] private AudioSource _tankMove;
    [SerializeField] private AudioClip _tankIdleState;
    [SerializeField] private AudioClip _tankMoveState;

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
        _tankMove.clip = _tankIdleState;
        _tankMove.Play();
    }
    public void PlayTankMove()
    {
        _tankMove.clip = _tankMoveState;
        _tankMove.Play();
    }
    public void PlayTankShot()
    {
        _tankShot.Play();
    }

}
