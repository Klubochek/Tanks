using System;
using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour
{

    public int TeamOwner;
    private Coroutine _coroutine;
    public bool HasCollision;

    private void OnEnable()
    {

        _coroutine = StartCoroutine(AutoDestroy());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            Debug.Log("Colision with ground");
            DestroyShell(gameObject);

        }
        if (collision.gameObject.CompareTag("Player") && !HasCollision)
        {
            HasCollision = true;
            Debug.Log("Colision with player");
            var tankStats = collision.gameObject.GetComponentInParent<TankStats>();
            if (tankStats.HP > 0 && tankStats.Team != TeamOwner)
            {
                tankStats.Damage();

            }
            Console.WriteLine($"Current hp:{tankStats.HP}");
            if (tankStats.HP == 1)
            {
                tankStats._joint.connectedBody = null;
                TankAnimation anim = collision.gameObject.transform.GetComponentInParent<TankAnimation>();
                anim.PlayDestroyAnimation();
                tankStats.Death();
            }
            DestroyShell(gameObject);
        }
    }


    private void DestroyShell(GameObject gameObject)
    {
        StopCoroutine(_coroutine);
        gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        var rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        gameObject.SetActive(false);
    }
    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(5);
        DestroyShell(gameObject);
    }
}
