using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Services
{
    public interface IGameEvents
    {
        public event System.Action OnLevelLoaded;
    }
}
