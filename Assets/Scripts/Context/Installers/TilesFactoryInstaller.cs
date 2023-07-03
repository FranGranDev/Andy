using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Tiles;
using Game.Tiles.Factory;


namespace Game.Context
{
    public class TilesFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IFactory<TileTypes, Tile>>()
                .To<TileFactory>()
                .AsSingle();
        }
    }
}
