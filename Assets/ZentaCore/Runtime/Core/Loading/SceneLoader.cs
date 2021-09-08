using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenta.Core.Runtime;
using Zenta.Core.Runtime.Managers;

namespace Zenta.Core.Runtime
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image slider;
        [SerializeField] private Image preview;

        private void Start()
        {
            LoadScene();
        }

        public void LoadScene()
        {
            Level level = GameManager.Instance.GetLevel();
            int buildIndex = level.scene;


            if (preview)
            {
                if (level.preview)
                {
                    preview.overrideSprite = level.preview;
                    preview.SetNativeSize();
                }
            }

            StartCoroutine(LoadSceneSync(buildIndex, level));
        }

        private IEnumerator LoadSceneSync(int buildIndex, Level level)
        {
            yield return null;

            if (slider)
            {
                slider.fillAmount = 0.2f;
            }

            AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

            operation.completed += (obj) => OnSceneLoaded(level);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                if (slider)
                {
                    slider.fillAmount = progress;
                }

                yield return null;
            }
        }

        private void OnSceneLoaded(Level level)
        {
            GameManager.Instance.OnLevelLoaded?.Invoke(level);
        }
    }
}