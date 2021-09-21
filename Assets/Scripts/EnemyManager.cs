using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private List<Enemy> enemies;

    private void Awake()
    {
        Instance = this;

        enemies = GetComponentsInChildren<Enemy>().ToList();
    }

    public bool HaveClosestEnemy(Vector3 position)
    {
        position.y = 0f;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null && enemies[i].enemyState == EnemyState.LIVE)
            {
                if (Vector3.Distance(position, enemies[i].transform.position) < 10f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
