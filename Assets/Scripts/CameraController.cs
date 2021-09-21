using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Transform defaultReference;

    private void Awake()
    {
        Instance = this;    
    }

    public void SetTransformReference(Transform reference,float actionTime) 
    {
        return;

        StartCoroutine(_SetTransformReference(reference,actionTime));
    }

    private IEnumerator _SetTransformReference(Transform reference, float actionTime) 
    {
        transform.DOLocalMove(reference.localPosition, .5f, false);

        transform.DOLocalRotateQuaternion(reference.localRotation, .5f);

        yield return new WaitForSeconds(actionTime);

        SetDefaultReference();
    }

    public void SetDefaultReference() 
    {
        transform.DOLocalMove(defaultReference.localPosition, .5f, false);

        transform.DOLocalRotateQuaternion(defaultReference.localRotation, .5f);
    }
}
