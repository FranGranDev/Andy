using UnityEngine;
using Zenject;

namespace Game.Context
{
    [CreateAssetMenu(fileName = "LevelsDataInstaller", menuName = "Installers/LevelsDataInstaller")]
    public class LevelsDataInstaller : ScriptableObjectInstaller<LevelsDataInstaller>
    {
        [SerializeField] private LevelsData levelsData;

        public override void InstallBindings()
        {
            Container.Bind<LevelsData>()
                .FromInstance(levelsData);
        }
    }
}