using Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DI
{
    public class ComponentInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Movement();
            GroundChecker();
        }

        private void Movement()
        {
            Container
                .Bind<Movement>()
                .To<Movement>()
                .FromComponentInParents();
        }

        private void GroundChecker()
        {
            Container
                .Bind<GroundChecker>()
                .To<GroundChecker>()
                .FromComponentInParents();
        }
    }
}