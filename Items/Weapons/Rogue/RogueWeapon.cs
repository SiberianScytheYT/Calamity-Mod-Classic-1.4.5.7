using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CalRD.Items.Weapons.Rogue
{
	public abstract class RogueWeapon : ModItem
	{
		public virtual void SafeSetDefaults()
		{
		}

		public float StealthStrikeDamage;

		public RogueWeapon()
		{
			StealthStrikeDamage = 1f;
		}

		public override ModItem Clone(Item itemClone)
		{
			RogueWeapon myClone = (RogueWeapon)base.Clone(itemClone);
			myClone.StealthStrikeDamage = StealthStrikeDamage;
			return myClone;
		}

		public override int ChoosePrefix(UnifiedRandom rand)
		{
			WeightedRandom<string> newPrefix = new WeightedRandom<string>();
			newPrefix.Add("Pointy", 1);
			newPrefix.Add("Sharp", 1);
			newPrefix.Add("Feathered", 1);
			newPrefix.Add("Sleek", 1);
			newPrefix.Add("Hefty", 1);
			newPrefix.Add("Mighty", 1);
			newPrefix.Add("Glorious", 1);
			newPrefix.Add("Serrated", 1);
			newPrefix.Add("Vicious", 1);
			newPrefix.Add("Lethal", 1);
			newPrefix.Add("Flawless", 1);
			newPrefix.Add("Radical", 1);
			newPrefix.Add("Blunt", 1);
			newPrefix.Add("Flimsy", 1);
			newPrefix.Add("Unbalanced", 1);
			newPrefix.Add("Atrocious", 1);
			//return Mod.GetPrefix(newPrefix).Type;
			return newPrefix.random.Next();
		}
		
		//public override void PreReforge()/* tModPorter Note: Use CanReforge instead for logic determining if a reforge can happen. */
		/*
		{
			StealthStrikeDamage = 1f;
			return true;
		}
		*/

		public override bool? PrefixChance(int pre, UnifiedRandom rand)
		{
			if (Item.maxStack > 1)
			{
				return false;
			}
			return null;
		}

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			// Item.melee = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			// Item.ranged = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			// Item.magic = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
			Item.DamageType = DamageClass.Throwing;
			// Item.summon = false/* tModPorter Suggestion: Remove. See Item.DamageType */;
		}

		// Simply add the player's dedicated rogue damage.
		// Rogue weapons are internally throwing so they already benefit from throwing damage boosts.
		// 5E-06 to prevent downrounding is not needed anymore, added by TML itself
		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			damage.Base *= player.Calamity().throwingDamage - 1f;
			//Boost (or lower) the weapon's damage if it has a stealth strike available and an associated prefix
			if (player.Calamity().StealthStrikeAvailable() && Item.prefix > 0)
			{
				damage *= StealthStrikeDamage - 1f;
			}
		}

		// Simply add the player's dedicated rogue crit chance.
		// Rogue crit isn't boosted by Calamity universal crit boosts, so this won't double-add universal crit.
		public override void ModifyWeaponCrit(Player player, ref float crit)
		{
			crit += player.Calamity().throwingCrit;
		}

		public override float UseTimeMultiplier(Player player)
		{
			float rogueAS = 1f;
			if (player.Calamity().gloveOfPrecision)
				rogueAS -= 0.2f;
			if (player.Calamity().gloveOfRecklessness)
				rogueAS += 0.2f;
			if (player.Calamity().titanHeartMantle)
				rogueAS -= 0.15f;
			return rogueAS;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " rogue " + damageWord;
			}
			if (Item.prefix > 0)
			{
				float ssDmgBoost = StealthStrikeDamage - 1f;
				if (ssDmgBoost > 0)
				{
					TooltipLine StealthBonus = new TooltipLine(Mod, "PrefixSSDmg", "+" + Math.Round(ssDmgBoost * 100f) + "% stealth strike damage")
					{
						IsModifier = true
					};
					tooltips.Add(StealthBonus);
				}
				else if (ssDmgBoost < 0)
				{
					TooltipLine StealthBonus = new TooltipLine(Mod, "PrefixSSDmg", "-" + Math.Round(Math.Abs(ssDmgBoost) * 100f) + "% stealth strike damage")
					{
						IsModifier = true,
						IsModifierBad = true
					};
					tooltips.Add(StealthBonus);
				}
			}
		}

		public override bool ConsumeItem(Player player) => Main.rand.NextFloat() <= player.Calamity().throwingAmmoCost;
	}
}
