﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Enemy : MonoBehaviour
{
    public EnemyState enemyState;

    public Rigidbody bodyrgd;

    private NavMeshAgent agent;

    public Action onDie;

    public PlayerController playerController;

    private void Awake()
    {
        enemyState = EnemyState.LIVE;

        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        playerController = PlayerController.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemyState == EnemyState.DIE)
        {
            return;
        }

        if (other.GetComponent<IWeapon>() != null && enemyState == EnemyState.LIVE)
        {
            if (other.GetComponent<IWeapon>().WeaponState == WeaponState.ACTIVE)
            {
                Die();
            }
        }

        if (other.GetComponent<DynamicCube>() != null && enemyState == EnemyState.LIVE)
        {
            if (other.GetComponent<DynamicCube>().cubeState == CubeState.ON_HERO )
            {
                Die(other.gameObject.transform.position);

                other.GetComponent<DynamicCube>().DestroyThis();
            }
        }
    }

    public void Die()
    {
        enemyState = EnemyState.DIE;

        ActiveRagdoll();

        Destroy(this.gameObject, 2f);

        onDie?.Invoke();
    }

    public void Die(Vector3 damagePoint) 
    {
        enemyState = EnemyState.DIE;

        ActiveRagdoll(damagePoint);

        Destroy(this.gameObject, 2f);

        onDie?.Invoke();
    }

    private void Update()
    {
        if (enemyState == EnemyState.LIVE)
        {
            Vector3 playerPosition = PlayerController.Instance.model.transform.position;

            playerPosition.y = 0f;

            float followDistance = 8f;

            if (playerController.currentWeapon != null)
            {
                followDistance = 30f;
            }
            else
            {
                followDistance = 8f;
            }

            if (Vector3.Distance(playerPosition, transform.position) < followDistance)
            {
                agent.SetDestination(playerPosition);

                GetComponentInChildren<Animator>().SetBool("Walk", true);
            }
        }
        else
        {
            Destroy(agent);
        }
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
            rigidbodies[i].AddForce(Vector3.forward * UnityEngine.Random.Range(2000f,3000f));
        }
    }

    public void ActiveRagdoll(Vector3 damagePoint)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        GetComponentInChildren<Animator>().enabled = false;

        damagePoint.y = bodyrgd.transform.position.y;

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
        }

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            bodyrgd.AddForce(((transform.position - damagePoint).normalized + new Vector3(0,.2f,0)) * UnityEngine.Random.Range(3500f, 5000f) );
        }
    }

}

public enum EnemyState
{
    LIVE,
    DIE
} 
