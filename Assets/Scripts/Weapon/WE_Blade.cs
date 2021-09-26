using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class WE_Blade : MonoBehaviour , IWeapon
{
    public WeaponState weaponState;
    public WeaponState WeaponState { get { return weaponState; } set { weaponState = value; } }
    public WeaponType WeaponType { get { return weaponType; } set { weaponType = value; } }

    public WeaponType weaponType;

    public Transform camReference;
    public Transform CamReference { get { return camReference; } }

    public Transform characterReference;
    public Transform CharacterReference { get { return camReference; } }

    public float actionTime;
    public float ActionTime { get { return actionTime; } }

    public Color weaponColor { get { return WeaponColor; } }

    public Color WeaponColor;

    public bool CanActive()
    {
        return true;
    }

    public List<CubePivot> CubePivots { get { return cubePivots; } set { cubePivots = value; } }

    public List<CubePivot> cubePivots;

    public int RequiredCubeCount
    {
        get { return cubePivots.Count; }
    }
    private void Awake()
    {
        CubePivots = GetComponentsInChildren<CubePivot>().ToList();

        WeaponState = WeaponState.READY;

        startPosition = transform.localPosition;
    }

    

    private void Update()
    {
        if (WeaponState == WeaponState.ACTIVE)
        {
            transform.Rotate(Vector3.up * -350f*Time.deltaTime);
        }
    }

    private Vector3 startPosition;

    public void Active()
    {
        WeaponState = WeaponState.ACTIVE;

        transform.DOMove(transform.position + new Vector3(0,0,25), 1f, false).OnComplete(() =>
        {
            WeaponState = WeaponState.READY;

            transform.localEulerAngles = Vector3.zero;

            transform.localPosition = startPosition;
        }); 

        StartCoroutine(_Destroy());
    }

    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(.7f);

        Destroy();
    }

    public void Destroy() 
    {
        for (int i = 0; i < cubePivots.Count; i++)
        {
            if (cubePivots[i].attachedCube != null)
            {
                cubePivots[i].attachedCube.DestroyThis();
            }
        }
    }

    public CubePivot GetNextPivot()
    {
        return null;
    }
}
