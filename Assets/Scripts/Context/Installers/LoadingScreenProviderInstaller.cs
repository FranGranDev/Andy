using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game.Context
{
    public class LoadingScreenProviderInstaller : MonoInstaller
    {
        [SerializeField] private string screenId;

        public override void InstallBindings()
        {
            Container.Bind<LoadingScreenProvider>()
                .FromInstance(new LoadingScreenProvider(screenId))
                .AsSingle();
        }
    }
}
