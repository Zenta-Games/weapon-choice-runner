using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GateSample : MonoBehaviour
{
    public MultiplerHolder upper, down;

    public Action<float, MultiplerType> onTakeHolder;

    public void OnTakeGate(float value,MultiplerType multiplerType) 
    {
        onTakeHolder?.Invoke(value,multiplerType);

        DestroyGates();
    }

    public void DestroyGates() 
    {
        Destroy(this.gameObject);

        upper.gameObject.SetActive(false);

        down.gameObject.SetActive(false);
    }

    private void Start()
    {
        upper.onTakeHolder += OnTakeGate;

        down.onTakeHolder += OnTakeGate;
    }
}

