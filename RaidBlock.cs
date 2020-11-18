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
	public class RaidBlock : RocketPlugin<RaidBlockConfiguration>
	{
		protected override void Load()
		{
			RaidBlock.Instance = this;
			StructureManager.onDamageStructureRequested = (DamageStructureRequestHandler)Delegate.Combine(StructureManager.onDamageStructureRequested, new DamageStructureRequestHandler(this.OnDamageStruct2));
			BarricadeManager.onDamageBarricadeRequested = (DamageBarricadeRequestHandler)Delegate.Combine(BarricadeManager.onDamageBarricadeRequested, new DamageBarricadeRequestHandler(this.OnDamageBarric2));
			R.Commands.OnExecuteCommand += new RocketCommandManager.ExecuteCommand(this.Commandss);
		}

		protected override void Unload()
		{
			StructureManager.onDamageStructureRequested = (DamageStructureRequestHandler)Delegate.Remove(StructureManager.onDamageStructureRequested, new DamageStructureRequestHandler(this.OnDamageStruct2));
			BarricadeManager.onDamageBarricadeRequested = (DamageBarricadeRequestHandler)Delegate.Remove(BarricadeManager.onDamageBarricadeRequested, new DamageBarricadeRequestHandler(this.OnDamageBarric2));
			R.Commands.OnExecuteCommand -= new RocketCommandManager.ExecuteCommand(this.Commandss);
		}
		private void OnDamageStruct2(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
		{
			shouldAllow = true;
			if (!this.dcv.ContainsKey(instigatorSteamID))
			{
				UnturnedChat.Say(UnturnedPlayer.FromCSteamID(instigatorSteamID), "You got a RAID block 2 min.");
				this.dcv.Add(instigatorSteamID, DateTime.Now);
			}
		}
		private void OnDamageBarric2(CSteamID instigatorSteamID, Transform structureTransform, ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
		{
			shouldAllow = true;
			if (!this.dcv.ContainsKey(instigatorSteamID))
			{
				UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromCSteamID(instigatorSteamID);
				UnturnedChat.Say(unturnedPlayer, "You got a RAID block 2 min.");
				this.dcv.Add(unturnedPlayer.CSteamID, DateTime.Now);
			}
		}
		public void FixedUpdate()
		{
			foreach (SteamPlayer steamPlayer in Provider.players)
			{
				UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(steamPlayer);
				if (this.dcv.ContainsKey(unturnedPlayer.CSteamID) && (DateTime.Now - this.dcv[unturnedPlayer.CSteamID]).TotalSeconds >= 100.0)
				{
					this.dcv.Remove(unturnedPlayer.CSteamID);
					UnturnedChat.Say(unturnedPlayer, "The block finised.");
				}
			}
		}
		public void Commandss(IRocketPlayer player, IRocketCommand command, ref bool cancel)
		{
			UnturnedPlayer unturnedPlayer = (UnturnedPlayer)player;
			foreach (RaidBlockConfiguration.Command command2 in base.Configuration.Instance.Commands2)
			{
				if (command.Name == command2.name && this.dcv.ContainsKey(unturnedPlayer.CSteamID))
				{
					cancel = true;
				}
			}
		}

		public Dictionary<CSteamID, DateTime> dcv = new Dictionary<CSteamID, DateTime>();

		public static RaidBlock Instance;
	}
}
