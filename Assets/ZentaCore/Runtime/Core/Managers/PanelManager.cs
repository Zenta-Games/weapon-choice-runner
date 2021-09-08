using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenta.Core.Runtime.UI.Panel;
using Zenta.Core.Runtime.UI.Panel.Panels;

namespace Zenta.Core.Runtime.Managers
{
    public class PanelManager : MonoBehaviour
    {
        public static PanelManager Instance;

        [SerializeField] private float planeDistance = 1f;

        private Canvas _canvas;
        private List<IPanel> panels;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _canvas = GetComponent<Canvas>();

            panels = GetComponentsInChildren<IPanel>(true).ToList();

            GameManager.Instance.OnStateChanged += OnStateChanged;
        }

        public void Initialize(Camera camera)
        {
            if (camera)
            {
                _canvas.worldCamera = camera;
            }
            else
            {
                if (Camera.main)
                {
                    _canvas.worldCamera = Camera.main;
                }
            }

            if (planeDistance <= 0)
            {
                planeDistance = 1f;
            }

            _canvas.planeDistance = planeDistance;
        }

        private void OnValidate()
        {
            if (planeDistance <= 0f)
            {
                planeDistance = 1f;
            }
        }

        private void OnStateChanged(GameState prev, GameState next)
        {
            if (prev == next) return;

            if (next == GameState.Initial)
            {
                ChangePanel<InitialPanel>();
            }
            else if (next == GameState.Playing)
            {
                ChangePanel<PlayingPanel>();
            }
            else if (next == GameState.Completed)
            {
                ChangePanel<CompletedPanel>();
            }
            else if (next == GameState.Failed)
            {
                ChangePanel<FailedPanel>();
            }
            else
            {
                Debug.LogError($"No panel was found with => {next} state");
            }
        }

        public T GetPanel<T>() where T : IPanel
        {
            IPanel panel = panels.Find(x => x.GetType() == typeof(T));
            return (T)panel;
        }

        public T ChangePanel<T>() where T : IPanel
        {
            IPanel panel = GetPanel<T>();

            if (panel == null)
            {
                Debug.LogError($"There is no panel : {typeof(T)}");
                return null;
            }

            if (panel.status)
            {
                return null;
            }

            UpdatePanels(false);

            panel.UpdatePanel(true);

            return (T)panel;
        }

        public void LoadDefaultPanel()
        {
            ChangePanel<InitialPanel>();
            UpdatePanels(false);
        }

        public void UpdatePanels(bool status)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].UpdatePanel(status);
            }
        }

        private void OnDestroy()
        {
            Instance = null;
            GameManager.Instance.OnStateChanged -= OnStateChanged;
        }
    }
}
