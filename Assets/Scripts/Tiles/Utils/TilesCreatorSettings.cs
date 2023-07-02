using Game.Tiles.Data;
using UnityEngine;


namespace Utils.Tiles
{
    [CreateAssetMenu(fileName = "Utils/Tiles Creator")]
    public class TilesCreatorSettings : ScriptableObject
    {
        [Header("Delete GUI Radius")]
        [Min(0.01f)] public float DeleteRaduis;

        [Header("Links")]
        [SerializeField] private TilesData tilesData;


        public float TileSizeX => tilesData.SizeX;
        public float TileSizeY => tilesData.SizeY;
    }
}
