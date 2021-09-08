using Zenta.Core.Runtime.Managers;

namespace Zenta.Core.Runtime.UI.Buttons
{
    public class NextButton : ButtonBase
    {
        protected override void Start()
        {
            this.onClick.AddListener(() =>
            {
                GameManager.Instance.LoadNextLevel();
            });

            base.Start();
        }
    }
}
