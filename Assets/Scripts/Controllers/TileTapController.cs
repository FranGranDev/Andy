using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.UserInput;
using Game.Tiles;
using UniRx.Triggers;
using UniRx;


namespace Game.Controllers
{
    public class TileTapController : MonoBehaviour
    {
        [Inject]
        private ScreenInput screenInput;


        private CompositeDisposable disposable = new CompositeDisposable();
        public IObservable<Tile> TileTapSource { get; private set; }


        private void OnEnable()
        {
            TileTapSource = Observable.Create<Tile>(observer =>
            {
                return screenInput.ClickSource
                    .Select(x => x.GetComponentInParent<Tile>())
                    .Where(x => x != null && !x.Hidden && !x.Disabled)
                    .Subscribe(x => observer.OnNext(x))
                    .AddTo(disposable);
            });
        }
        private void OnDisable()
        {
            disposable.Dispose();
        }
    }
}
