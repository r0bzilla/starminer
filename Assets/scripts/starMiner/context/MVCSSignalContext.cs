using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.signal.impl;

namespace StarMiner.Context
{
    /**
	 * This context uses a signal command binder instead of an event
	 * command binder and as such, dispatches the StartContextSignal
	 * instead of the StartEvent.
	 */
    public abstract class MVCSSignalContext : MVCSContext
    {
        public MVCSSignalContext(MonoBehaviour contextView) : base(contextView)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            mapSignals();
            mapCommands();
            mapMediators();
        }

        protected abstract void mapSignals();

        protected abstract void mapCommands();

        protected abstract void mapMediators();

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        public override void Launch()
        {
            base.Launch();
            Signal startSignal = injectionBinder.GetInstance<StartContextSignal>();
            startSignal.Dispatch();
        }
    }
}
