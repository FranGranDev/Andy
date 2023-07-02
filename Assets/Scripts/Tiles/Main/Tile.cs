using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tiles.Data;
using System;

namespace Game.Tiles
{
    [RequireComponent(typeof(ITileView), typeof(ITileContact))]
    public class Tile : MonoBehaviour
    {
        [Header("States")]
        [SerializeField] private Vector2 position;
        [SerializeField] private TileTypes tileType;
        [SerializeField] private int layer;
        [SerializeField] private bool hidden;

        private ITileView tileView;
        private ITileContact tileContact;



        public Vector2 Position
        {
            get => position;
            set => position = value;
        }
        public TileTypes TileType
        {
            get => tileType;
            set => tileType = value;
        }
        public int Layer
        {
            get => layer;
            set
            {
                layer = value;
                tileView.Layer = value;
            }
        }
        public bool Hidden
        {
            get => hidden;
            set
            {
                hidden = value;
                tileView.Hidden = value;
            }
        }


        public event Action OnSelect;
        public event Action OnUnselect;
        public event Action OnTap;


        public void Initialize(TileTypes tileType, TilesData.ViewData viewData)
        {
            TileType = tileType;

            tileView = GetComponent<ITileView>();
            tileContact = GetComponent<ITileContact>();

            tileView.Setup(viewData.Sprite, viewData.Color);

            tileContact.OnSelect += Select;
            tileContact.OnUnselect += Unselect;
            tileContact.OnTap += Tap;
        }
        public void Apply(Data data)
        {
            TileType = data.TileType;
            Position = data.Position;
            Layer = data.Layer;
            Hidden = data.Hidden;

            transform.localPosition = data.LocalPosition;
        }



        private void Tap()
        {
            
        }
        private void Unselect()
        {
            
        }
        private void Select()
        {
            
        }


        public Data GetData()
        {
            return new Data(this);
        }

        [System.Serializable]
        public class Data
        {
            public Data(Tile tile)
            {
                Position = tile.Position;
                LocalPosition = tile.transform.localPosition;
                TileType = tile.TileType;
                Layer = tile.Layer;
                Hidden = tile.Hidden;
            }

            [field: SerializeField] public Vector2 Position { get; private set; }
            [field: SerializeField] public Vector2 LocalPosition { get; private set; }
            [field: SerializeField] public TileTypes TileType { get; private set; }
            [field: SerializeField] public int Layer { get; private set; }
            [field: SerializeField] public bool Hidden { get; private set; }
        }
    }


    public enum TileTypes
    {
        Red,
        Green,
        Orange,
        Purple,
        Black,
        White,
    }
}