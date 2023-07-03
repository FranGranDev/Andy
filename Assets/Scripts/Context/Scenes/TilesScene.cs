using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tiles;
using Zenject;
using Game.UI;
using Game.Services;
using System;

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
        private TileBoard tileBoard;

        [Inject]
        private LoadingScreenProvider screenProvider;


        public event Action OnLevelLoaded;


        public void Start()
        {
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

            levelLoader = new LevelLoader(tileBoard, level);

            await screen.Load(levelLoader);

            await screen.Hide();
            screenProvider.Unload();
        }

        private void NextScene()
        {
            levelIndex++;

            LoadLevel(levelIndex);
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                NextScene();
            }
        }
    }
}
