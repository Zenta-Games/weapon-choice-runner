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

    Vector3 velocity;

    public void OnCubeStateChanged(CubeState cubeState) 
    {
        switch (cubeState)
        {
            case CubeState.ON_HERO:

                CubeManager.Instance.OnNewCubeAttachedHero(this);

                rgd.isKinematic = true;

                boxCollider.enabled = false;

                positionLerpSpeed = 10f;

                break;

            case CubeState.ON_WEAPON:

                positionLerpSpeed = 15f;

                boxCollider.enabled = true;

                break;
         
            case CubeState.COLLECTABLE:

                positionLerpSpeed = 0f;

                break;
            
            case CubeState.DESTROYED:

                if (cubePivot != null)
                {
                    cubePivot.hasAttach = false;

                    cubePivot.attachedCube = null;
                }

                cubePivot = null;

                rgd.isKinematic = false;

                rgd.useGravity = true;

                boxCollider.enabled = true;

                rgd.AddForce(velocity * 50f,ForceMode.VelocityChange);

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

        cubePivot.attachedCube = this;

        cubePivot.hasAttach = true;

        AttachWeapon();
    }

    private float positionLerpSpeed = 5f;

    public float targetDistance;

    private Vector3 lastPosition;

    private void Update()
    {
        velocity = transform.position - lastPosition;

        lastPosition = transform.position;

        if (cubePivot != null)
        {
            if (CubeState == CubeState.ON_WEAPON)
            {
                //targetDistance = Vector3.Distance(cubePivot.cubePoint.transform.position, transform.position);

                //if (targetDistance < 1f)
                //{

                //}
                //else
                //{
                //}

                transform.position = Vector3.Lerp(transform.position, cubePivot.cubePoint.transform.position, Time.deltaTime * positionLerpSpeed);

                transform.rotation = Quaternion.Lerp(model.transform.rotation, cubePivot.transform.rotation, Time.deltaTime * 50f);

                model.transform.localScale = Vector3.Lerp(model.transform.localScale, cubePivot.cubePoint.transform.localScale, Time.deltaTime * 500f);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, cubePivot.cubePoint.transform.position, Time.deltaTime * positionLerpSpeed);

                transform.rotation = Quaternion.Lerp(model.transform.rotation, cubePivot.transform.rotation, Time.deltaTime * 5f);

                model.transform.localScale = Vector3.Slerp(model.transform.localScale, cubePivot.cubePoint.transform.localScale, Time.deltaTime * 5f);
            }

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
