using Zenta.Core.Runtime.UI.Utils;
using System;

namespace Zenta.Core.Runtime.UI.Panel.Panels
{
    public class PlayingPanel : IPanel
    {
        public ProgressBar ProgressBar;

        public Action<WeaponType> onSelectWeapon;

        public FixedTouchField touchField;

        public override void UpdatePanel(bool status)
        {
            base.UpdatePanel(status);
        }

        private void Start()
        {
            WeaponSelectionButton[] buttons = GetComponentsInChildren<WeaponSelectionButton>();

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onSelectWeapon += OnSelectWeapon;
            }
        }

        public void OnSelectWeapon(WeaponType weaponType) 
        {
            onSelectWeapon?.Invoke(weaponType);
        }
    }
}
