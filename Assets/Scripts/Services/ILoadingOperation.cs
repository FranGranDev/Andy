using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Services
{
    public interface ILoadingOperation
    {
        public string Description { get; }
        public IObservable<float> Progress { get; }

        public UniTask Load();
    }

    public interface IUnloadingOperation
    {
        public UniTask Unload();
    }
}
