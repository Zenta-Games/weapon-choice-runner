using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTestingSystem : MonoBehaviour
{
    public DynamicCube dynamicCubeSample;

    public int spawnedCube;

    [Button("Spawn Cube")]
    public void SpawnCube()
    {
        if (spawnedCube == 140)
        {
            return;
        }

        DynamicCube dynamicCube = Instantiate(dynamicCubeSample.gameObject, transform.position, transform.rotation, null).GetComponent<DynamicCube>();
        
        dynamicCube.AttachHero();

        spawnedCube += 1;
    }
}
