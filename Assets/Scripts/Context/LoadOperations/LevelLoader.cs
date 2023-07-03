using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Services;
using UniRx;
using System;
using Game.Tiles;
using Cysharp.Threading.Tasks;
using Game.Controllers;

namespace Game.Context
{
    public class LevelLoader : ILoadingOperation, IUnloadingOperation
    {
        public LevelLoader(TileGameController gameController, LevelsData.Level level)
        {
            this.level = level;
            this.gameController = gameController;

            progress = new Subject<float>();
        }

        private Subject<float> progress;
        private LevelsData.Level level;
        private TileGameController gameController;

        private SceneLoader sceneLoader;


        public string Description => "Level Loading";
        public IObservable<float> Progress => progress;


        public async UniTask Load()
        {
            sceneLoader = new SceneLoader(level.BackgroundSceneId, LoadSceneMode.Additive);

            await sceneLoader.Load();

            gameController.Create(level.LevelData);
        }
        public async UniTask Unload()
        {
            await sceneLoader.Unload();

            gameController.Clear();
        }
    }
}
