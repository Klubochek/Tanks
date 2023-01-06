using System;
using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private ShellPool shellPool;
    public int TeamOwner;
    public Coroutine coroutine;
    public bool hasCollision;

    private void OnEnable()
    {
        shellPool = FindObjectOfType<ShellPool>();
        coroutine = StartCoroutine(AutoDestroy());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            Debug.Log("Colision with ground");
            DestroyShell(gameObject);

        }
        if (collision.gameObject.CompareTag("Player") && !hasCollision)
        {
            hasCollision = true;
            Debug.Log("Colision with player");
            var tankStats = collision.gameObject.GetComponentInParent<TankStats>();
            if (tankStats.hp > 0 && tankStats.Team != TeamOwner)
            {
                tankStats.Damage();

            }
            Console.WriteLine($"Current hp:{tankStats.hp}");
            if (tankStats.hp == 1)
            {
                tankStats.joint.connectedBody = null;
                TankAnimation anim = collision.gameObject.transform.GetComponentInParent<TankAnimation>();
                anim.PlayDestroyAnimation();
                tankStats.CmdDeath();
            }
            DestroyShell(gameObject);
        }
    }
    

    private void DestroyShell(GameObject gameObject)
    {
        StopCoroutine(coroutine);
        shellPool.shellPool.Add(gameObject);
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
