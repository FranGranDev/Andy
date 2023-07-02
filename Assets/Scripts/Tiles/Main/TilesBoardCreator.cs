using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Game.Tiles.Data;
using UnityEditor;


namespace Game.Tiles
{
    public class TilesBoardCreator : MonoBehaviour
    {
        [Header("File info")]
        [SerializeField] private string fileName = "Level_0";
        [SerializeField] private string folderPath = "Assets/Data/Levels";
        [Header("Components")]
        [SerializeField] private Transform container;
        [Header("Links")]
        [SerializeField] private TilesData tilesData;
        [Header("Load Data")]
        [SerializeField] private TilesLevelData levelData;


        private Dictionary<Vector2, Stack<Tile>> tiles = new Dictionary<Vector2, Stack<Tile>>();


        public void CreateAt(Vector2 position, TileTypes tileType)
        {
            Tile tile = Create(tileType);

            int layer = 0;
            List<Tile> neighbours = Neighbours(position);
            neighbours.ForEach(x => x.Hidden = true);
            if (neighbours.Count > 0)
            {
                tile.transform.localPosition = position;
                layer = neighbours.Max(x => x.Layer) + 1;
            }


            tile.Position = position;
            tile.transform.localPosition = position;

            if (tiles.ContainsKey(position))
            {
                Stack<Tile> tileStack = tiles[position];

                if(neighbours.Count == 0)
                {
                    tile.transform.localPosition += Vector3.down * 0.25f * tileStack.Count;
                }
                tile.Layer = tileStack.Count + layer;


                tileStack.Push(tile);
            }
            else
            {
                Stack<Tile> tileStack = new Stack<Tile>();


                tile.Layer = layer;

                tileStack.Push(tile);

                tiles.Add(position, tileStack);
            }
        }
        public void DeleteAt(Vector2 position)
        {
            if(!tiles.ContainsKey(position))
            {
                return;
            }

            Tile tile = tiles[position].Pop();
            DestroyImmediate(tile.gameObject);
            if(tiles[position].Count == 0)
            {
                tiles.Remove(position);
            }

            List<Tile> affecteds = Neighbours(position);
            foreach(Tile affected in affecteds)
            {
                List<Tile> neighbours = Neighbours(affected.Position);
                affected.Hidden = neighbours.Count(x => x.Layer > affected.Layer) > 0;
            }
        }

        public void Clear()
        {
            tiles.Clear();

            while(container.childCount > 0)
            {
                DestroyImmediate(container.GetChild(0).gameObject);
            }
        }
        public void Save()
        {
            TilesLevelData data = ScriptableObject.CreateInstance<TilesLevelData>();

            List<Tile.Data> tilesData = tiles
                .SelectMany(x => x.Value.Select(t => t.GetData()))
                .ToList();

            data.Data = tilesData;

            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = $"{folderPath}/{fileName}.asset";

            AssetDatabase.CreateAsset(data, filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"{nameof(TilesLevelData)} {fileName} saved at: " + folderPath);
        }
        public void Load()
        {
            if(levelData == null)
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
                        Tile tile = Create(x.TileType);
                        tile.Apply(x);

                        return tile;
                    }))
                );
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

            foreach(Vector2 offset in offsets)
            {
                if (tiles.ContainsKey(position + offset))
                {
                    neighbours.AddRange(tiles[position + offset]);
                }
            }

            return neighbours;
        }


        private Tile Create(TileTypes type)
        {
            Tile tile = Instantiate(tilesData.Prefab, container).GetComponent<Tile>();

            tile.Initialize(type, tilesData.GetViewData(type));

            return tile;
        }
    }
}
