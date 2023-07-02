using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tiles.Data
{
    [CreateAssetMenu(menuName = "Tiles/TilesData")]
    public class TilesData : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [Space]
        [SerializeField] private int sizeX;
        [SerializeField] private int sizeY;
        [Space]
        [SerializeField] private List<Item> items;
       

        public GameObject Prefab
        {
            get => prefab;
        }
        public ViewData GetViewData(TileTypes tileType)
        {
            return items.FirstOrDefault(x => x.TileType == tileType)?.ViewData;
        }

        public float SizeX => sizeX;
        public float SizeY => sizeY;


        [System.Serializable]
        public class Item
        {
            [SerializeField] private string name;
            [SerializeField] private TileTypes tileType;
            [Space]
            [SerializeField] private ViewData viewData;

            public TileTypes TileType
            {
                get => tileType;
            }
            public ViewData ViewData
            {
                get => viewData;
            }
        }

        [System.Serializable]
        public class ViewData
        {
            [SerializeField] private Sprite sprite;
            [SerializeField] private Color color;


            public Sprite Sprite => sprite;
            public Color Color => color;
        }
    }
}