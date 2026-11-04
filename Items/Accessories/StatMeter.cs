using CalRD.CalPlayer;
using CalRD.World;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class StatMeter : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Stat Meter");
/*
			Tooltip.SetDefault("Displays almost all player stats");
*/
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.buyPrice(0, 6, 0, 0);
			Item.rare = 1;
		}

		public override void ModifyTooltips(List<TooltipLine> list)
		{
			Player player = Main.player[Main.myPlayer];
			if (player is null)
				return;
			CalamityPlayer modPlayer = player.Calamity();

			Item heldItem = null;
			if (player.selectedItem >= 0 && player.selectedItem < Main.InventorySlotsTotal)
				heldItem = player.ActiveItem();

			// Replace the vanilla tooltip with a full stat readout
			foreach (TooltipLine l in list)
			{
				if (l.Mod == "Terraria" && l.Name == "Tooltip0")
					l.Text = CreateStatMeterTooltip(player, modPlayer, heldItem);
			}

			// To save screen space, favorited tooltips do not exist for the Stat Meter
			list.RemoveAll(l => l.Mod == "Terraria" && (l.Name == "Favorite" || l.Name == "FavoriteDesc"));
		}

		private string CreateStatMeterTooltip(Player player, CalamityPlayer modPlayer, Item heldItem)
		{
			int defense = modPlayer.defenseStat;
			int DR = modPlayer.DRStat;
			int meleeSpeed = modPlayer.meleeSpeedStat;
			int manaCost = modPlayer.manaCostStat;
			int rogueVelocity = modPlayer.rogueVelocityStat;
			int minionSlots = modPlayer.minionSlotStat;
			int ammoConsumption = modPlayer.ammoReductionRanged;
			int rogueConsumption = modPlayer.ammoReductionRogue;
			int rogueStealth = modPlayer.stealthStat;
			// Format the stealth regen statistics to be accurate to 2 decimal places.
			string standingRegen = modPlayer.standingRegenStat.ToString("n2");
			string movingRegen = modPlayer.movingRegenStat.ToString("n2");
			int lifeRegen = modPlayer.lifeRegenStat;
			int manaRegen = modPlayer.manaRegenStat;
			int armorPenetration = modPlayer.armorPenetrationStat;
			string wingFlightTime = modPlayer.wingFlightTimeStat.ToString("n2");
			string jumpSpeed = modPlayer.jumpSpeedStat.ToString("n2");
			int moveSpeed = modPlayer.moveSpeedStat;
			int lightLevel = modPlayer.abyssLightLevelStat;
			int breathLoss = modPlayer.abyssBreathLossStat;
			int breathLossRate = modPlayer.abyssBreathLossRateStat;
			int lifeLostAtZeroBreath = modPlayer.abyssLifeLostAtZeroBreathStat;
			int defenseLoss = modPlayer.abyssDefenseLossStat;

			// The notice about held item mattering is always displayed first.
			StringBuilder sb = new StringBuilder(this.GetLocalizedValue("RealTooltip") + "\n\n", 1024);

			// Only append rippers stats in Rev+, if rippers are enabled.
			if (CalamityWorld.revenge && CalamityConfig.Instance.Rippers)
			{
				sb.Append(this.GetLocalization("Rippers").Format(modPlayer.adrenalineDamageStat, modPlayer.adrenalineDRStat, modPlayer.rageDamageStat))
					.Append("\n\n");
			}

			// Append item stats only if the held item isn't null, and base it off of the item's damage type.
			if (heldItem != null && !heldItem.IsAir)
			{
				if (heldItem.CountsAsClass(DamageClass.Melee))
				{
					sb.Append(this.GetLocalization("Melee").Format(modPlayer.damageStats[0], modPlayer.damageStats[5], modPlayer.critStats[0], meleeSpeed))
						.Append("\n\n");
				}
				else if (heldItem.CountsAsClass(DamageClass.Ranged))
				{
					sb.Append(this.GetLocalization("Ranged").Format(modPlayer.damageStats[1], modPlayer.critStats[1], ammoConsumption))
						.Append("\n\n");
				}
				else if (heldItem.CountsAsClass(DamageClass.Magic))
				{
					sb.Append(this.GetLocalization("Magic").Format(modPlayer.damageStats[2], modPlayer.critStats[2], manaCost, manaRegen))
						.Append("\n\n");
				}
				else if (heldItem.CountsAsClass(DamageClass.Summon))
				{
					sb.Append(this.GetLocalization("Summon").Format(modPlayer.damageStats[3], minionSlots))
						.Append("\n\n");
				}
				else if (heldItem.Calamity()?.rogue ?? false)
				{
					sb.Append(this.GetLocalization("Rogue").Format(modPlayer.damageStats[4], modPlayer.critStats[3], rogueVelocity, rogueConsumption, rogueStealth, standingRegen, movingRegen))
						.Append("\n\n");
				}
			}

			// Generic stats always render.
			sb.Append(this.GetLocalization("Generic").Format(defense, DR, lifeRegen, armorPenetration, wingFlightTime, jumpSpeed, moveSpeed))
				.Append("\n\n");
			sb.Append(CalamityWorld.death ? this.GetLocalizedValue("Death") : this.GetLocalizedValue("NonDeath")).Append(lightLevel).Append("\n\n");

			// Abyss stats only render if the player is in the Abyss.
			if (modPlayer.ZoneAbyss)
			{
				sb.Append(this.GetLocalization("Abyss").Format(breathLoss, breathLossRate, lifeLostAtZeroBreath, defenseLoss));
			}
			else
				sb.Append(this.GetLocalizedValue("NotInAbyss"));

			return sb.ToString();
		}
	}
}
