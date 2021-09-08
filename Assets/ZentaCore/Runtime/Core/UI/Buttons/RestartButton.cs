using Zenta.Core.Runtime.Managers;

namespace Zenta.Core.Runtime.UI.Buttons
{
    public class RestartButton : ButtonBase
    {
        protected override void Start()
        {
            this.onClick.AddListener(() =>
            {
                GameManager.Instance.LoadCurrentLevel();
            });

            base.Start();
        }
    }
}