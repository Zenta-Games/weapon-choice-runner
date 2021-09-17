using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CubePivot : MonoBehaviour
{
    [HideInInspector]
    public Transform cubePoint;

    public DynamicCube attachedCube;
 
    private void Awake()
    {
        cubePoint = transform.GetChild(0);

        cubePoint.gameObject.SetActive(false);

        attachedCube = null;

        if (GetComponentInChildren<MeshRenderer>())
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }

    public void SetWeaponScale() 
    {
    //       cubePoint.DOScale(new Vector3(0f,0f,0f),.1f);
    }

    public void RandomChildPosition() 
    {
        cubePoint = transform.GetChild(0);

        cubePoint.transform.localPosition = new Vector3(0, 0, Random.Range(1.5f, 2.1f));
    }
}
