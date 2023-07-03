using UnityEngine;
using Zenject;
using Game.Tiles.Data;


namespace Game.Context
{
    [CreateAssetMenu(fileName = "TilesDataInstaller", menuName = "Installers/TilesDataInstaller")]
    public class TilesDataInstaller : ScriptableObjectInstaller<TilesDataInstaller>
    {
        [SerializeField] private TilesData tilesData;

        public override void InstallBindings()
        {
            Container.Bind<TilesData>()
                .FromInstance(tilesData);
        }
    }
}