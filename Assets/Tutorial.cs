using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenta.Core.Runtime.Managers;

public class Tutorial : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance.Level == 0)
        {
            Destroy(this.gameObject,3f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
