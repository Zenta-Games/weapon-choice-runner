using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GateManager : MonoBehaviour
{
    public static GateManager Instance;

    public List<GateSample> gateSamples;

    public Action<float, MultiplerType> onTakeNewGate;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gateSamples = GetComponentsInChildren<GateSample>().ToList();

        for (int i = 0; i < gateSamples.Count; i++)
        {
            int value = i;

            gateSamples[value].onTakeHolder += OnTakeNewGate;
        }
    }
    
    public void OnTakeNewGate(float value,MultiplerType type) 
    {
        onTakeNewGate?.Invoke(value,type);
    }
}
