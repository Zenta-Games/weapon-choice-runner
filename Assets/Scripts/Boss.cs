using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public EnemyState enemyState;

    public Rigidbody bodyRigid;

    public int health = 50;

    private void Awake()
    {
        enemyState = EnemyState.LIVE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DynamicCube>() != null && enemyState == EnemyState.LIVE)
        {
            if (other.GetComponent<DynamicCube>().cubeState == CubeState.ON_WEAPON)
            {
                TakeDamage();

                other.GetComponent<DynamicCube>().DestroyThis();
            }
        }
    }

    private void TakeDamage() 
    {
        health -= 1;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        enemyState = EnemyState.DIE;

        ActiveRagdoll();

        Destroy(this.gameObject, 3f);
    }

    public void ActiveRagdoll()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        GetComponentInChildren<Animator>().enabled = false;

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
        }

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].AddForce(Vector3.forward * Random.Range(2000f, 3000f));
        }
    }
}
