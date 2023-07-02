using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tiles.Data
{
    [CreateAssetMenu(menuName = "Tiles/LevelData")]
    public class TilesLevelData : ScriptableObject
    {
        [field: SerializeField] public List<Tile.Data> Data { get; set; }
    }
}
