using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WeaponSelectionButton : MonoBehaviour
{
    public WeaponType weaponType;

    public Action<WeaponType> onSelectWeapon;

    public Animator animator;

    public Sprite activeImage, passiveImage;

    public Image image;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => { onSelectWeapon?.Invoke(weaponType); });

        animator.enabled = false;
    }

    public void SetCanUse(bool active) 
    {
        if (active)
        {
            image.overrideSprite = activeImage;

            button.interactable = true;
        }
        else
        {
            image.overrideSprite = passiveImage;

            button.interactable = false;
        }
    }
}
