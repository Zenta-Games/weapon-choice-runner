using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCube : MonoBehaviour
{
    public CubePivot cubePivot;

    public CubeState cubeState;

    private Rigidbody rgd;

    private BoxCollider boxCollider;

    private Transform model;
    
    public CubeState CubeState 
    {
        get { return cubeState; }

        set
        {
            cubeState = value;
         
            OnCubeStateChanged(cubeState);
        }
    }

    private void Awake()
    {
        rgd = GetComponent<Rigidbody>();

        boxCollider = GetComponent<BoxCollider>();

        CubeState = CubeState.COLLECTABLE;

        model = transform.GetChild(0);
    }

    public void OnCubeStateChanged(CubeState cubeState) 
    {
        switch (cubeState)
        {
            case CubeState.ON_HERO:

                CubeManager.Instance.OnNewCubeAttachedHero(this);

                rgd.isKinematic = true;

                boxCollider.enabled = false;

                positionLerpSpeed = 15f;

                break;

            case CubeState.ON_WEAPON:

                positionLerpSpeed = 5f;

                break;
         
            case CubeState.COLLECTABLE:

                positionLerpSpeed = 0f;

                break;
            
            case CubeState.DESTROYED:

                Debug.Log("Destroy This");

                cubePivot.hasAttach = false;

                cubePivot = null;

                rgd.isKinematic = false;

                rgd.useGravity = true;

                boxCollider.enabled = true;

                rgd.AddForce(Vector3.up * -250f);

                Destroy(this.gameObject, 2f);

                break;
            
            default:
                break;
        }
    }

    [Button("Attach Hero")]
    public void AttachHero() 
    {
        CubeState = CubeState.ON_HERO;
    }

    [Button("Attach Weapon")]
    public void AttachWeapon()
    {
        CubeState = CubeState.ON_WEAPON;
    }

    [Button("Destroy This")]
    public void DestroyThis()
    {
        CubeState = CubeState.DESTROYED;
    }

    [Button("Set Collectable Mode")]
    public void SetCollectableMode()
    {
        CubeState = CubeState.COLLECTABLE;
    }

    public void SetHero(CubePivot pivotTarget) 
    {
        cubePivot = pivotTarget;
    }

    public void SetWeapon(CubePivot pivotTarget) 
    {
        cubePivot = pivotTarget;

        cubePivot.hasAttach = true;

        AttachWeapon();
    }

    private float positionLerpSpeed = 5f;

    private void Update()
    {
        if (cubePivot != null)
        {
            transform.position = Vector3.Slerp(transform.position, cubePivot.cubePoint.transform.position, Time.deltaTime * positionLerpSpeed);

            model.transform.rotation = Quaternion.Lerp(model.transform.rotation,cubePivot.cubePoint.transform.rotation,Time.deltaTime * 5f);

            model.transform.localScale = Vector3.Slerp(model.transform.localScale,cubePivot.cubePoint.transform.localScale,Time.deltaTime * 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CubeState == CubeState.COLLECTABLE)
        {
            AttachHero();
        }
    }
}

public enum CubeState 
{
    ON_HERO,
    ON_WEAPON,
    COLLECTABLE,
    DESTROYED
}
