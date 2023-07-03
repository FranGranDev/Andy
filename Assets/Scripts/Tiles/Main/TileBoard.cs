using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tiles.Data;
using System.Linq;
using Zenject;


namespace Game.Tiles
{
    public class TileBoard : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Transform container;


        [Inject]
        private IFactory<TileTypes, Tile> tileFactory;


        private Dictionary<Vector2, Stack<Tile>> tiles = new Dictionary<Vector2, Stack<Tile>>();


        public IEnumerable<Tile> AllTiles
        {
            get => tiles.SelectMany(x => x.Value.Select(x => x));
        }


        public void Remove(Tile tile)
        {
            if(tiles.ContainsKey(tile.Position))
            {
                tiles[tile.Position].Pop();
                if(tiles[tile.Position].Count == 0)
                {
                    tiles.Remove(tile.Position);
                }
            }
            else
            {
                throw new System.Exception($"No tile at {tile.Position}");
            }
        }


        public void Create(TilesLevelData levelData)
        {
            if (levelData == null)
            {
                Debug.LogWarning("levelData is empty");
                return;
            }

            Clear();

            tiles = levelData.Data
                .GroupBy(data => data.Position)
                .ToDictionary(
                    group => group.Key,
                    group => new Stack<Tile>(group
                    .OrderBy(x => x.Layer)
                    .Select(x =>
                    {
                        Tile tile = tileFactory.Create(x.TileType);
                        tile.transform.SetParent(container);
                        tile.Apply(x);

                        return tile;
                    }))
                );
        }
        public void Clear()
        {
            tiles.Clear();

            while (container.childCount > 0)
            {
                DestroyImmediate(container.GetChild(0).gameObject);
            }
        }
    }
}
