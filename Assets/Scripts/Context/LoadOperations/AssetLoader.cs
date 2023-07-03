using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace Game.Context
{
    public class AssetLoader
    {
        private GameObject instance;

        protected async UniTask<GameObject> LoadInternal(string id)
        {
            var handle = Addressables.InstantiateAsync(id);
            instance = await handle.Task;

            return instance;
        }
        protected async UniTask<T> LoadInternal<T>(string id)
        {
            await LoadInternal(id);

            if(instance.TryGetComponent(out T result))
            {
                return result;
            }
            else
            {
                throw new System.Exception($"Error, asset {id} is not {typeof(T)}");
            }
        }


        protected void UnloadInternal()
        {
            if (!instance)
                return;

            instance.SetActive(false);
            Addressables.ReleaseInstance(instance);

            instance = null;
        }
    }
}