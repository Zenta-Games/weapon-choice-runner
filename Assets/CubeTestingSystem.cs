using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTestingSystem : MonoBehaviour
{
    public DynamicCube dynamicCubeSample;

    [Button("Spawn Cube")]
    public void SpawnCube()
    {
        DynamicCube dynamicCube = Instantiate(dynamicCubeSample.gameObject, transform.position, transform.rotation, null).GetComponent<DynamicCube>();
        
        dynamicCube.AttachHero();
    }
}
