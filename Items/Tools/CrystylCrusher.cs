using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
	public class CrystylCrusher : ModItem
	{
		private static int PickPower = 5000;
		private static float LaserSpeed = 14f;

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Crystyl Crusher");
/*
			Tooltip.SetDefault("Gotta dig faster, gotta go deeper\n" +
				"Right click to swing normally");
*/
			Item.staff[Item.type] = true;
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 2000;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.noMelee = true;
			Item.channel = true;
			Item.crit += 25;
			Item.width = 70;
			Item.height = 70;
			Item.useTime = Item.useAnimation = 2;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 9f;
			Item.shootSpeed = 14f;
			Item.value = Item.buyPrice(5, 0, 0, 0);
			Item.rare = 10;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/CrystylCharge");
			Item.shoot = ModContent.ProjectileType<CrystylCrusherRay>();
			Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
			Item.pick = PickPower;
		}

		public override Vector2? HoldoutOrigin()
		{
			if (Item.useStyle == ItemUseStyleID.Swing)
				return null;
			return new Vector2(10, 10);
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.shoot = ProjectileID.None;
				Item.shootSpeed = 0f;
				Item.tileBoost = 50;
				Item.UseSound = SoundID.Item1;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.useTurn = true;
				Item.autoReuse = true;
				Item.noMelee = false;
				Item.channel = false;
			}
			else
			{
				Item.shoot = ModContent.ProjectileType<CrystylCrusherRay>();
				Item.shootSpeed = LaserSpeed;
				Item.tileBoost = -6;
				Item.UseSound = new SoundStyle("CalRD/Sounds/Item/CrystylCharge");
				Item.useStyle = ItemUseStyleID.Shoot;
				Item.useTurn = false;
				Item.autoReuse = false;
				Item.noMelee = true;
				Item.channel = true;
			}
			return base.CanUseItem(player);
		}

		public override void ModifyTooltips(List<TooltipLine> list)
		{
			if (Item.useStyle == ItemUseStyleID.Shoot)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "TileBoost")
					{
						line2.Text = "";
					}
				}
			}
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (player.altFunctionUse == 2)
				return;

			if (Main.rand.NextBool(3))
			{
				int dustType = Utils.SelectRandom(Main.rand, new int[]
				{
					173,
					57,
					58
				});
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, dustType);
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("LunarPickaxe");
			recipe.AddIngredient(ModContent.ItemType<BlossomPickaxe>());
			recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
			recipe.Register();
		}
	}
}
