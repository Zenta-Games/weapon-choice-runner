using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubePivot : MonoBehaviour
{
    [HideInInspector]
    public Transform cubePoint;

    public bool hasAttach;
 
    private void Awake()
    {
        cubePoint = transform.GetChild(0);

        cubePoint.gameObject.SetActive(false);

        hasAttach = false;

        if (GetComponentInChildren<MeshRenderer>())
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    public void RandomChildPosition() 
    {
        cubePoint = transform.GetChild(0);

        cubePoint.transform.localPosition = new Vector3(0, 0, Random.Range(1.5f, 2.1f));
    }
}
