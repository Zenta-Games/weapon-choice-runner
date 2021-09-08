using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeRotater : MonoBehaviour
{
    public Transform baseRotationPivot;

    public float rotateSpeed;
    
    public List<CubePivot> cubePivots;

    private void Start()
    {
        int direction = Random.Range(0, 2);

        if (direction == 0)
        {
            rotateSpeed = Random.Range(-120f, -180f);
        }
        else
        {
            rotateSpeed = Random.Range(120f, 180f);
        }

        cubePivots = GetComponentsInChildren<CubePivot>().ToList();
    }

    void Update()
    {
        baseRotationPivot.transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime,0f),Space.Self);
    }
}
