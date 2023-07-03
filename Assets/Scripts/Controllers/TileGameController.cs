using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Game.Tiles;
using Zenject;
using Game.Tiles.Data;
using UniRx;


namespace Game.Controllers
{
    public class TileGameController : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private TileBoard tileBoard;
        [SerializeField] private TileStack tileStack;
        [SerializeField] private TileTapController tapController;


        public event System.Action OnWin;
        public event System.Action OnFail;


        private void Start()
        {
            tapController.TileTapSource
                .Subscribe(OnTileTap)
                .AddTo(this);

            tileStack.OnStackFull += Fail;
            tileBoard.OnEmpty += Win;
        }


        public void Create(TilesLevelData levelData)
        {
            tileBoard.Create(levelData);
        }
        public void Clear()
        {
            tileStack.Clear();
            tileBoard.Clear();
        }

        private void Win()
        {
            Debug.Log("Win");
            OnWin?.Invoke();
        }
        private void Fail()
        {
            Debug.Log("Fail");
            OnFail?.Invoke();   
        }
        

        private void OnTileTap(Tile tile)
        {
            if (tileStack.Add(tile))
            {
                tileBoard.Remove(tile);
            }        
        }
    }
}
