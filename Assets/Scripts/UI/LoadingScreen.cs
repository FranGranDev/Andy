using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using UniRx;
using Cysharp.Threading.Tasks;
using TMPro;
using System;

namespace Game.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private float showTime;
        [SerializeField] private float hideTime;
        [Header("Components")]
        [SerializeField] private CanvasGroup canvasGroup;
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI loadingText;


        public async UniTask Load(params ILoadingOperation[] loadingOperations)
        {
            foreach(ILoadingOperation operation in loadingOperations)
            {
                loadingText.text = operation.Description;
                //IDisposable subscription = operation.Progress
                //    .Subscribe(x => Debug.Log($"Loading: {x}"))
                //    .AddTo(this);

                await operation.Load();

                //subscription.Dispose();
            }
        }
        public async UniTask Unload(params IUnloadingOperation[] unloadingOperations)
        {
            foreach (IUnloadingOperation operation in unloadingOperations)
            {
                loadingText.text = "Unload";

                await operation.Unload();
            }
        }


        public async UniTask Show()
        {
            canvasGroup.alpha = 0;

            float time = 0;
            while(time < showTime)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, time / showTime);

                time += Time.fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
            }

            canvasGroup.alpha = 1;
        }
        public async UniTask Hide()
        {
            float time = 0;
            while (time < hideTime)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, time / hideTime);

                time += Time.fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
            }

            canvasGroup.alpha = 0;
        }
    }
}
