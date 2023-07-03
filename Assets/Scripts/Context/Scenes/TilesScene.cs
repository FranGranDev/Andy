using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tiles;
using Zenject;
using Game.UI;
using Game.Services;
using System;
using Game.Controllers;

namespace Game.Context
{
    public class TilesScene : MonoBehaviour, IGameEvents
    {
        [Header("Settings")]
        [SerializeField] private int levelIndex;

        private LevelLoader levelLoader;

        [Inject]
        private LevelsData levelsData;

        [Inject]
        private TileGameController gameController;

        [Inject]
        private LoadingScreenProvider screenProvider;


        public event Action OnLevelLoaded;


        public void Start()
        {
            gameController.OnFail += ReloadLevel;
            gameController.OnWin += NextLevel;

            LoadLevel(levelIndex);
        }

        private async void LoadLevel(int index)
        {            
            LevelsData.Level level = levelsData.GetLevel(index);

            LoadingScreen screen = await screenProvider.Load();
            await screen.Show();

            if (levelLoader != null)
            {
                await screen.Unload(levelLoader);
            }

            levelLoader = new LevelLoader(gameController, level);

            await screen.Load(levelLoader);

            await screen.Hide();
            screenProvider.Unload();
        }

        private async void NextLevel()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            levelIndex++;

            LoadLevel(levelIndex);
        }
        private async void ReloadLevel()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));

            LoadLevel(levelIndex);
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                NextLevel();
            }
        }
    }
}
