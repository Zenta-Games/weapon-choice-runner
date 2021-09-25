using Zenta.Core.Runtime.UI.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zenta.Core.Runtime.UI.Panel.Panels
{
    public class PlayingPanel : IPanel
    {
        public ProgressBar ProgressBar;

        public Action<WeaponType> onSelectWeapon;

        public FixedTouchField touchField;

        public List<WeaponSelectionButton> buttons;

        public Transform weaponContent;

        public override void UpdatePanel(bool status)
        {
            base.UpdatePanel(status);
        }

        private void Start()
        {
            buttons = GetComponentsInChildren<WeaponSelectionButton>().ToList();

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].onSelectWeapon += OnSelectWeapon;
            }

            weaponContent.gameObject.SetActive(false);
        }

        public void OnSelectWeapon(WeaponType weaponType) 
        {
            onSelectWeapon?.Invoke(weaponType);

            weaponContent.gameObject.SetActive(false);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].animator.enabled = false;
            }
        }

        public void JiggleTargetWeapon(WeaponType weaponType) 
        {
            weaponContent.gameObject.SetActive(true);

            for (int i = 0; i < buttons.Count; i++)
            {
                int requiredCount = PlayerController.Instance.weapons.Find(x => x.WeaponType == buttons[i].weaponType).RequiredCubeCount;

                int cubeCount = CubeManager.Instance.attachedCubeCount;

                if (cubeCount > requiredCount)
                {
                    buttons[i].SetCanUse(true);
                }
                else
                {
                    buttons[i].SetCanUse(false);
                }
            }

            buttons.Find(x => x.weaponType == weaponType).animator.enabled = true;
        }
    }
}
