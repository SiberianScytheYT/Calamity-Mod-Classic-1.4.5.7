using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class DuststormInABottle : RogueWeapon
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Duststorm in a Bottle");
/*
			Tooltip.SetDefault("Explodes into a dust cloud\n" +
			"Stealth strikes form a more intense and longer lasting dust cloud");
*/
		}

		public override void SafeSetDefaults()
		{
			Item.width = 20;
			Item.damage = 47;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 25;
			Item.knockBack = 5f;
			Item.UseSound = SoundID.Item106;
			Item.autoReuse = true;
			Item.height = 24;
			Item.value = Item.buyPrice(0, 60, 0, 0);
			Item.rare = 7;
			Item.shoot = ModContent.ProjectileType<DuststormInABottleProj>();
			Item.shootSpeed = 12f;
			Item.Calamity().rogue = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.Calamity().StealthStrikeAvailable())
			{
				int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[stealth].Calamity().stealthStrike = true;
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HolyWater, 20);
			recipe.AddIngredient(ModContent.ItemType<GrandScale>());
			recipe.AddIngredient(ItemID.SandstorminaBottle);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
