using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class BensUmbrella : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Temporal Umbrella");
/*
			Tooltip.SetDefault("Surprisingly sturdy, I reckon this could defeat the Mafia in a single blow\n" +
							   "Summons a magic hat to hover above your head\n" +
							   "The hat will release a variety of objects to assault your foes\n" +
							   "Requires 5 minion slots to use\n" +
							   "There can only be one");
*/
		}

		public override void SetDefaults()
		{
			Item.damage = 963;
			Item.knockBack = 1f;
			Item.mana = 99;
			Item.useTime = Item.useAnimation = 10;
			Item.DamageType = DamageClass.Summon;
			Item.shootSpeed = 0f;
			Item.shoot = ModContent.ProjectileType<MagicHat>();

			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 74;
			Item.height = 72;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item68;
			Item.value = Item.buyPrice(5, 0, 0, 0);
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0 && player.maxMinions >= 5;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			CalamityUtils.KillShootProjectiles(true, type, player);
			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpikecragStaff>());
			recipe.AddIngredient(ModContent.ItemType<SarosPossession>());
			recipe.AddIngredient(ItemID.Umbrella);
			recipe.AddIngredient(ItemID.TopHat);
			recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
			recipe.Register();
		}
	}
}
