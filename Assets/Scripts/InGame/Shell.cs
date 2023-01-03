using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Shell : MonoBehaviour
{
    private ShellPool shellPool;
    public int TeamOwner;
    public Coroutine coroutine;
    private void Start()
    {
        shellPool = FindObjectOfType<ShellPool>();
        coroutine = StartCoroutine(AutoDestroy());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            Debug.Log("Colision with ground");
            CmdDestroy(gameObject);

        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colision with player");
            var tankStats=collision.gameObject.GetComponentInParent<TankStats>();
            if (tankStats.hp > 0)
            {
                tankStats.CmdDamage();
            }
            else 
            {
                TankAnimation anim = collision.gameObject.transform.GetComponentInParent<TankAnimation>();
                anim.PlayDestroyAnimation(); 
            }
            CmdDestroy(gameObject);
        }
    }

    private void CmdDestroy(GameObject gameObject)
    {
        StopCoroutine(coroutine);
        shellPool.shellPool.Add(gameObject);
        gameObject.transform.rotation = new Quaternion(0, 0, 0,0);
        var rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
     
        gameObject.SetActive(false);
    }
    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(5);
        CmdDestroy(gameObject);
    }
}
