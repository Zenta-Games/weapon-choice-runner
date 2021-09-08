using UnityEngine;
using Zenta.Core.Runtime.Interfaces;

namespace Zenta.Core.Runtime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public void Initialize(Level level)
        {
            Instantiate(GameManager.Instance.Config.GameUI).GetComponent<PanelManager>().Initialize(_camera);

            GameManager.Instance.DispatchInitialize(level);

            GameManager.Instance.LoadInitialState();
        }

        private void OnValidate()
        {
            if (Camera.main)
            {
                _camera = Camera.main;
            }
        }
    }
}
