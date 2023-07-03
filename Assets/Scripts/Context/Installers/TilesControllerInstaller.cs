using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Tiles;
using Game.Controllers;


namespace Game.Context
{
    public class TilesControllerInstaller : MonoInstaller
    {
        [SerializeField] private TileGameController tileController;

        public override void InstallBindings()
        {
            Container.Bind<TileGameController>()
                .FromInstance(tileController);
        }
    }
}
