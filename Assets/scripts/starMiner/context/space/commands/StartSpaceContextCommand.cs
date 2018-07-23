using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using strange.extensions.command.impl;

namespace StarMiner.Context.Space.Commands
{
    /**
	 * This command is executed when the space context finished loading.
	 */
    public class StartSpaceContextCommand : Command
    {
        public override void Execute()
        {
            Debug.Log("START");
        }
    }
}
