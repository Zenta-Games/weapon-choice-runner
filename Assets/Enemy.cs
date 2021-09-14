using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyState enemyState;

    public Rigidbody bodyRigid;

    private void Awake()
    {
        enemyState = EnemyState.LIVE;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IWeapon>() != null && enemyState == EnemyState.LIVE)
        {
            if (other.GetComponent<IWeapon>().WeaponState == WeaponState.ACTIVE)
            {
                Die();
            }
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
            rigidbodies[i].AddForce(Vector3.forward * Random.Range(2000f,3000f));
        }
    }

}

public enum EnemyState
{
    LIVE,
    DIE
} 
