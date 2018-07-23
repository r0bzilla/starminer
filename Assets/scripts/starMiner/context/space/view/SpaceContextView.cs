using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using strange.extensions.context.impl;
using StarMiner.Context.Space;

namespace StarMiner.Context.Space.View
{
    public class SpaceContextView : ContextView
    {
        protected void Awake()
        {
            this.context = new SpaceContext(this);
        }
    }
}
