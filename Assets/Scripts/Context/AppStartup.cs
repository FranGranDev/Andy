using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using UnityEngine.SceneManagement;


namespace Game.Context
{
    public class AppStartup : MonoBehaviour
    {
        [SerializeField] private string sceneId;

        private async void Start()
        {
            AddressableInitializer addressableInitializer = new AddressableInitializer();
            SceneLoader sceneLoader = new SceneLoader("JungleScene", LoadSceneMode.Additive);

            Queue<ILoadingOperation> loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(addressableInitializer);
            loadingOperations.Enqueue(sceneLoader);

            Queue<IUnloadingOperation> unloadingOperations = new Queue<IUnloadingOperation>();
            unloadingOperations.Enqueue(sceneLoader);


            LoadingScreenProvider loadingScreen = new LoadingScreenProvider("LoadingScreen");
            await loadingScreen.LoadAndDestroy(loadingOperations);
        }
    }
}
