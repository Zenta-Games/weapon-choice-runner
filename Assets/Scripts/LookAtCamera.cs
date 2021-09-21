using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCam;

    private void Awake()
    {
        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        if (mainCam != null)
        {
            transform.LookAt(mainCam);
        }
    }
}
