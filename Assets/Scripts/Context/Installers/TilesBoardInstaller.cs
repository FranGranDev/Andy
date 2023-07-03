using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.Tiles;



namespace Game.Context
{
    public class TilesBoardInstaller : MonoInstaller
    {
        [SerializeField] private TileBoard tileBoard;

        public override void InstallBindings()
        {
            Container.Bind<TileBoard>()
                .FromInstance(tileBoard);
        }
    }
}
