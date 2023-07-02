using Zenject;
using Game.Tiles.Data;


namespace Game.Tiles.Factory
{
    public class TileFactory : IFactory<TileTypes, Tile>
    {
        [Inject]
        private TilesData tilesData;

        [Inject]
        private DiContainer container;


        public Tile Create(TileTypes param)
        {
            Tile tile = container.InstantiatePrefabForComponent<Tile>(tilesData.Prefab);
            tile.Initialize(param, tilesData.GetViewData(param));

            return tile;
        }
    }
}
