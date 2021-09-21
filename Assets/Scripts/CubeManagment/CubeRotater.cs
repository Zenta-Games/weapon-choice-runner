using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CubeRotater : MonoBehaviour
{
    public Transform baseRotationPivot;

    public float rotateSpeed;
    
    public List<CubePivot> cubePivots;
    
    public int direction;

    private void Awake()
    {
        if (direction == 0)
        {
            rotateSpeed = Random.Range(-240f, -380f);
        }
        else
        {
            rotateSpeed = Random.Range(240f, 380f);
        }

        cubePivots = GetComponentsInChildren<CubePivot>().ToList();

        for (int i = 0; i < cubePivots.Count; i++)
        {
            cubePivots[i].RandomChildPosition();
        }
    }

    void Update()
    {
        baseRotationPivot.transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime,0f),Space.Self);
    }
}
