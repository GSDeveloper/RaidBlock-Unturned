using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace IM_BlockRaid
{
	public class RaidBlockConfiguration : IRocketPluginConfiguration, IDefaultable
	{
		public void LoadDefaults()
		{
			this.Commands2 = new List<RaidBlockConfiguration.Command>
			{
				new RaidBlockConfiguration.Command
				{
					name = "home"
				},
				new RaidBlockConfiguration.Command
				{
					name = "kits"
				}
			};
		}

		public List<RaidBlockConfiguration.Command> Commands2;

		public class Command
		{
			public string name;
		}
	}
}
