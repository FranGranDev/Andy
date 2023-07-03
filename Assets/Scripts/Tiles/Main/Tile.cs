using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tiles.Data;
using System;

namespace Game.Tiles
{
    [RequireComponent(typeof(ITileView))]
    public class Tile : MonoBehaviour
    {
        [Header("States")]
        [SerializeField] private Vector2 position;
        [SerializeField] private TileTypes tileType;
        [SerializeField] private int layer;
        [SerializeField] private bool hidden;
        [SerializeField] private bool disabled;
        [Header("Components")]
        [SerializeField] private new Collider2D collider;

        private ITileView tileView;


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
                collider.enabled = !value && !disabled;
            }
        }
        public bool Disabled
        {
            get => disabled;
            set
            {
                disabled = value;
                collider.enabled = !value && !hidden;
            }
        }


        public void Initialize(TileTypes tileType, TilesData.ViewData viewData)
        {
            TileType = tileType;

            tileView = GetComponent<ITileView>();

            tileView.Setup(viewData.Sprite, viewData.Color);
        }
        public void Apply(Data data)
        {
            TileType = data.TileType;
            Position = data.Position;
            Layer = data.Layer;
            Hidden = data.Hidden;

            transform.localPosition = data.LocalPosition;
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