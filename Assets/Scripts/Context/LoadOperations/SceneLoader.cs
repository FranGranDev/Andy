using Cysharp.Threading.Tasks;
using Game.Services;
using System;
using UniRx;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Game.Context
{
    public sealed class SceneLoader : ILoadingOperation, IUnloadingOperation
    {
        public SceneLoader(string sceneId, LoadSceneMode loadSceneMode)
        {
            this.sceneId = sceneId;
            this.loadSceneMode = loadSceneMode;

            progress = new Subject<float>();
        }


        private readonly string sceneId;
        private readonly LoadSceneMode loadSceneMode;
        private readonly Subject<float> progress;

        private AsyncOperationHandle handle;


        public string Description => $"Loading scene";
        public IObservable<float> Progress => progress;
        public async UniTask Load()
        {
            handle = Addressables.LoadSceneAsync(sceneId, loadSceneMode);

            while (!handle.IsDone)
            {
                progress.OnNext(handle.PercentComplete);
                await UniTask.Yield();
            }

            progress.OnCompleted();
        }

        public async UniTask Unload()
        {
            await Addressables.UnloadSceneAsync(handle);
        }
    }
}
