using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zenta.Core.Runtime.UI.Panel
{
    public class IPanel : MonoBehaviour
    {
        public bool status;

        public virtual void UpdatePanel(bool status)
        {
            this.status = status;

            gameObject.SetActive(status);
        }
    }
}