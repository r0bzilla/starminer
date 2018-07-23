using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using strange.extensions.context.impl;
using StarMiner.Context.Space.Commands;

namespace StarMiner.Context.Space
{
    public class SpaceContext : MVCSSignalContext
    {

        public SpaceContext(MonoBehaviour contextView) : base(contextView)
        {
        }


        protected override void mapSignals()
        {

        }

        protected override void mapCommands()
        {
            commandBinder.Bind<StartContextSignal>().To<StartSpaceContextCommand>();
        }

        protected override void mapMediators()
        {

        }

        protected override void postBindings()
        {

        }
    }
}
