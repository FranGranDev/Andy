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
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI loadingText;


        public async UniTask Load(Queue<ILoadingOperation> loadingOperations)
        {
            foreach(ILoadingOperation operation in loadingOperations)
            {
                loadingText.text = operation.Description;
                IDisposable subscription = operation.Progress
                    .Subscribe(x => Debug.Log($"Loading: {x}"))
                    .AddTo(this);

                await operation.Load();

                subscription.Dispose();
            }
        }
    }
}
