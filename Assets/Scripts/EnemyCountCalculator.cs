using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCountCalculator : MonoBehaviour
{
    public float radius;

    public Color color;

    public LayerMask layermask;

    public TextMeshProUGUI countText;

    public List<Enemy> enemies = new List<Enemy>();

    private void Start()
    {
        CalculateCount();

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].onDie += EnemyKill;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;

        Gizmos.DrawSphere(transform.position, radius);
    }

    [Button("Calculate Count")]
    public void CalculateCount()
    {
	
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius ,layermask);

        enemies.Clear();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetComponent<Enemy>() != null && colliders[i].gameObject.GetComponent<Enemy>().enemyState == EnemyState.LIVE)
            {
                if (enemies.Contains(colliders[i].gameObject.GetComponent<Enemy>()) == false)
                {
                    enemies.Add(colliders[i].gameObject.GetComponent<Enemy>());
                }
            }
        }

        countText.text = enemies.Count.ToString();
    }

    public void EnemyKill() 
    {
        CalculateCount();

        if (enemies.Count == 0)
        {
            this.gameObject.SetActive(false);

        }
    }
}
