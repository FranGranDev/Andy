using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Game.Tiles.Data;


namespace Game.Context
{
    [CreateAssetMenu(menuName = "Data/Levels")]
    public class LevelsData : ScriptableObject
    {
        [SerializeField] private List<Level> levels; 


        public Level GetLevel(int index)
        {
            if (levels.Count == 0)
                return null;

            index = index % levels.Count;

            return levels.FirstOrDefault(x => x.Index == index);
        }


        [System.Serializable]
        public class Level
        {
            public string Name;
            public int Index;
            [Space]
            public TilesLevelData LevelData;
            public string BackgroundSceneId;
        }


        private void OnValidate()
        {
            int index = 0;
            foreach(Level level in levels)
            {
                level.Index = index;
                index++;
            }
        }
    }
}
