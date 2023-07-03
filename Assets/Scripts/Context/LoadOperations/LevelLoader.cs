using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Services;
using UniRx;
using System;
using Game.Tiles;
using Cysharp.Threading.Tasks;

namespace Game.Context
{
    public class LevelLoader : ILoadingOperation, IUnloadingOperation
    {
        public LevelLoader(TileBoard board, LevelsData.Level level)
        {
            this.level = level;
            this.board = board;

            progress = new Subject<float>();
        }

        private Subject<float> progress;
        private LevelsData.Level level;
        private TileBoard board;

        private SceneLoader sceneLoader;


        public string Description => "Level Loading";
        public IObservable<float> Progress => progress;


        public async UniTask Load()
        {
            sceneLoader = new SceneLoader(level.BackgroundSceneId, LoadSceneMode.Additive);

            await sceneLoader.Load();

            board.Create(level.LevelData);
        }
        public async UniTask Unload()
        {
            await sceneLoader.Unload();

            board.Clear();
        }
    }
}
