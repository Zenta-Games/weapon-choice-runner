using Zenta.Core.Runtime.Managers;

namespace Zenta.Core.Runtime.UI.Labels
{
    public class LevelLabel : LabelBase
    {
        protected override void Start()
        {
            if (GameManager.Instance)
            {
                this.text = GameManager.Instance.Config.LevelLabelPrefix + " " + (GameManager.Instance.Level + 1);
            }

            base.Start();
        }
    }
}
