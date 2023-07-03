using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Game.UserInput
{
    public class ScreenInput : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;

        private CompositeDisposable disposable = new CompositeDisposable();

        public IObservable<GameObject> ClickSource { get; private set; }

        private void OnEnable()
        {
            ClickSource = Observable.Create<GameObject>(observer =>
            {
                return this.UpdateAsObservable()
                    .Where(x => Input.GetKeyDown(KeyCode.Mouse0))
                    .Select(x => Camera.main.ScreenToWorldPoint(Input.mousePosition))
                    .Select(x => Physics2D.OverlapPoint(x, layerMask))
                    .Where(x => x != null)
                    .Subscribe(x => observer.OnNext(x.gameObject))
                    .AddTo(disposable);
            });
        }
        private void OnDisable()
        {
            disposable.Dispose();
        }
    }
}