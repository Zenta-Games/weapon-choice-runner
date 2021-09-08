using Zenta.Core.Runtime.UI.Utils;

namespace Zenta.Core.Runtime.UI.Panel.Panels
{
    public class PlayingPanel : IPanel
    {
        public ProgressBar ProgressBar;

        public override void UpdatePanel(bool status)
        {
            base.UpdatePanel(status);
        }
    }
}
