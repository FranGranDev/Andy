using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Game.UserInput;


namespace Game.Context
{
    public class UserInputInstaller : MonoInstaller
    {
        [SerializeField] private ScreenInput screenInput;

        public override void InstallBindings()
        {
            Container.Bind<ScreenInput>()
                .FromInstance(screenInput);
        }
    }
}
