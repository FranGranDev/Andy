using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Game.Tiles;
using Zenject;
using Game.Services;
using UniRx;


namespace Game.Controllers
{
    public class TileGameController : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private TileStack tileStack;
        [SerializeField] private TileTapController tapController;



        private void Start()
        {
            tapController.TileTapSource
                .Subscribe(OnTileTap)
                .AddTo(this);
        }

        private void OnTileTap(Tile tile)
        {

        }
    }
}
