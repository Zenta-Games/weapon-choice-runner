using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenta.Core.Runtime.Managers;

public class Boss : MonoBehaviour
{
    public static Boss Instance;

    public EnemyState enemyState;

    public Rigidbody bodyRigid;

    public int health = 50;

    public TextMeshProUGUI healthText;

    private Animator animator;

    private PlayerController playerController;

    private void Awake()
    {
        Instance = this;

        enemyState = EnemyState.LIVE;

        healthText.text = health.ToString();

        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        playerController = PlayerController.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DynamicCube>() != null && enemyState == EnemyState.LIVE)
        {
            if (other.GetComponent<DynamicCube>().cubeState == CubeState.ON_WEAPON)
            {
                if (PlayerController.Instance.GetActiveWeapon.WeaponType == WeaponType.ARROW)
                {
                    TakeDamage(2);
                }
                else
                {
                    TakeDamage(1);
                }

                other.GetComponent<DynamicCube>().DestroyThis();
            }

            if (other.GetComponent<DynamicCube>().cubeState == CubeState.ON_HERO)
            {
                TakeDamage(1);

                other.GetComponent<DynamicCube>().DestroyThis();
            }
        }
    }

    private void TakeDamage(int damage) 
    {
        health -= damage;

        healthText.text = health.ToString();

        if (health <= 0)
        {
            Die();

            Destroy(healthText.transform.parent.gameObject);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.State == Zenta.Core.Runtime.GameState.Failed)
        {
            animator.Play("Victory");
        }
        else
        {
            if (Vector3.Distance(playerController.transform.position, transform.position) < 5f)
            {
                animator.Play("Atack");
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
            rigidbodies[i].AddForce(Vector3.forward * Random.Range(2000f, 3000f));
        }
    }
}
