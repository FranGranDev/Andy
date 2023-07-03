using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Services;
using Zenject;

namespace Game.Context
{
    public class GameEventInstaller : MonoInstaller
    {
        [SerializeField] private TilesScene tilesScene;

        public override void InstallBindings()
        {
            Container.Bind<IGameEvents>()
                .FromInstance(tilesScene);
        }
    }
}
