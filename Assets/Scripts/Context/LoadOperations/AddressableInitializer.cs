using Cysharp.Threading.Tasks;
using Game.Services;
using System;
using UniRx;
using UnityEngine.AddressableAssets;


namespace Game.Context
{
    public class AddressableInitializer : ILoadingOperation
    {
        public AddressableInitializer()
        {
            progress = new Subject<float>();
        }

        private readonly Subject<float> progress;


        public string Description => "Addressable Initialize";
        public IObservable<float> Progress => progress;
        public async UniTask Load()
        {
            var handle = Addressables.InitializeAsync();

            while (!handle.IsDone)
            {
                progress.OnNext(handle.PercentComplete);
                await UniTask.Yield();
            }

            progress.OnCompleted();
        }        
    }
}
