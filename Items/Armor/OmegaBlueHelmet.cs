using CalRD.Items.Materials;
using CalRD.Items.Placeables;
using CalRD.Buffs.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class OmegaBlueHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Omega Blue Helmet");
/*
			Tooltip.SetDefault("You can move freely through liquids\n12% increased damage and 8% increased critical strike chance\n+2 max minions");
*/
			if (Main.dedServ)
				return;
			var equipSlotHead = EquipLoader.GetEquipSlot(Mod, "OmegaBlueTransformation", EquipType.Head);
			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
		}

		public override void Load()
		{
			if (!Main.dedServ)
				EquipLoader.AddEquipTexture(Mod, "CalRD/Items/Armor/OmegaBlueHelmet_HeadMadness", EquipType.Head, name: "OmegaBlueTransformation");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 35, 0, 0);
			Item.rare = 10;
			Item.defense = 19;
			Item.Calamity().customRarity = CalamityRarity.PureGreen;
		}

		public override void UpdateEquip(Player player)
		{
			player.ignoreWater = true;

			player.GetDamage(DamageClass.Generic) += 0.12f;
			player.Calamity().AllCritBoost(8);

			player.maxMinions += 2;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<OmegaBlueChestplate>() &&
			       legs.type == ModContent.ItemType<OmegaBlueLeggings>();
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
			player.Calamity().omegaBlueTransformation = true;
			player.Calamity().omegaBlueTransformationForce = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			string hotkey = CalRD.TarraHotKey.TooltipHotkeyString();
			player.setBonus = "Increases armor penetration by 50\n" +
			                  "10% increased damage and critical strike chance\n" +
			                  "Short-ranged tentacles heal you by sucking enemy life\n" +
			                  "Press " + hotkey + " to activate abyssal madness for 5 seconds\n" +
			                  "Abyssal madness increases damage, critical strike chance, and tentacle aggression/range\n" +
			                  "This effect has a 25 second cooldown";

			player.GetArmorPenetration(DamageClass.Generic) += 50;
			player.Calamity().wearingRogueArmor = true;

			//raise rev caps
			player.Calamity().omegaBlueSet = true;

			if (player.Calamity().omegaBlueCooldown == 1) //dust when ready to use again
			{
				for (int i = 0; i < 66; i++)
				{
					int d = Dust.NewDust(player.position, player.width, player.height, 20, 0, 0, 100, Color.Transparent,
						2.6f);
					Main.dust[d].noGravity = true;
					Main.dust[d].noLight = true;
					Main.dust[d].fadeIn = 1f;
					Main.dust[d].velocity *= 6.6f;
				}

				SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/OmegaBlueRecharge"), player.Center);
			}

			if (player.Calamity().omegaBlueCooldown == 1500)
			{
				player.AddBuff(ModContent.BuffType<AbyssalMadnessCooldown>(), 1500, false);
			}

			if (player.Calamity().omegaBlueCooldown > 1500)
			{
				int d = Dust.NewDust(player.position, player.width, player.height, 20, 0, 0, 100, Color.Transparent,
					1.6f);
				Main.dust[d].noGravity = true;
				Main.dust[d].noLight = true;
				Main.dust[d].fadeIn = 1f;
				Main.dust[d].velocity *= 3f;
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ReaperTooth>(), 11);
			recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 5);
			recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
			recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
