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

        [Inject]
        private TilesData tilesData;


        private Dictionary<Vector2, Stack<Tile>> tiles = new Dictionary<Vector2, Stack<Tile>>();


        public event System.Action OnEmpty;


        public void Remove(Tile tile)
        {
            Vector2 position = tile.Position;

            if (!tiles.ContainsKey(position))
            {
                return;
            }

            tiles[position].Pop();
            if (tiles[position].Count == 0)
            {
                tiles.Remove(position);
            }
            else
            {
                tiles[position].Peek().Hidden = false;
            }


            List<Tile> affecteds = Neighbours(position);
            foreach (Tile affected in affecteds)
            {
                List<Tile> neighbours = Neighbours(affected.Position);
                affected.Hidden = neighbours.Count(x => x.Layer > affected.Layer) > 0;
            }

            if(tiles.Count == 0)
            {
                OnEmpty?.Invoke();
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


        private List<Tile> Neighbours(Vector2 position)
        {
            List<Tile> neighbours = new List<Tile>();

            List<Vector2> offsets = new List<Vector2>()
            {
                new Vector2(0, tilesData.SizeY / 2),
                new Vector2(tilesData.SizeX / 2, tilesData.SizeY / 2),
                new Vector2(tilesData.SizeX / 2, 0),
                new Vector2(tilesData.SizeX / 2, -tilesData.SizeY / 2),
                new Vector2(0, -tilesData.SizeY / 2),
                new Vector2(-tilesData.SizeX / 2, -tilesData.SizeY / 2),
                new Vector2(-tilesData.SizeX / 2, 0),
                new Vector2(-tilesData.SizeX / 2, tilesData.SizeY / 2),
            };

            foreach (Vector2 offset in offsets)
            {
                if (tiles.ContainsKey(position + offset))
                {
                    neighbours.AddRange(tiles[position + offset]);
                }
            }

            return neighbours;
        }

    }
}
