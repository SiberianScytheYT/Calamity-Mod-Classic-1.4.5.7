using CalRD.Items.Materials;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Typeless
{
	public class YanmeisKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Yanmei's Knife");
/*
			Tooltip.SetDefault("When hitting a boss, miniboss, or their minions, you gain various boosts and cripple the enemy hit\n" +
							   "A knife from an unknown world\n" +
							   "An owner whose heart is pure and free of taint\n" +
							   "A heart of iron and valor");
*/
		}

		public override void SetDefaults()
		{
			Item.height = 44;
			Item.width = 48;
			Item.damage = 8;
			Item.crit += 6;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAnimation = Item.useTime = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4.5f;
			Item.autoReuse = false;
			Item.value = CalamityGlobalItem.Rarity8BuyPrice;
			Item.UseSound = SoundID.Item71;
			Item.rare = 8;
			Item.shoot = ModContent.ProjectileType<YanmeisKnifeSlash>();
			Item.shootSpeed = 24f;
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.Calamity().KameiBladeUseDelay > 0)
				return false;
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.Calamity().KameiBladeUseDelay = 180;
			Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.PsychoKnife);
			recipe.AddIngredient(ItemID.Obsidian, 10);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 20);
			recipe.AddIngredient(ModContent.ItemType<PlagueCellCluster>(), 50);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
	}
}
