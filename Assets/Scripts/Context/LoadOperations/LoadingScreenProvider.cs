using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Services;


namespace Game.Context
{
    public class LoadingScreenProvider : AssetLoader
    {
        public LoadingScreenProvider(string screenId)
        {
            this.screenId = screenId;
        }
        private readonly string screenId;


        public async UniTask LoadAndDestroy(params ILoadingOperation[] loadingOperations)
        {
            var loadingScreen = await Load();
            await loadingScreen.Load(loadingOperations);
            Unload();
        }

        public UniTask<LoadingScreen> Load()
        {
            return LoadInternal<LoadingScreen>(screenId);
        }
        public void Unload()
        {
            UnloadInternal();
        }
    }
}
