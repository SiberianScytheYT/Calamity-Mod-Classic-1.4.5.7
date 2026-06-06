using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
	public class TyrannysEnd : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Tyranny's End");
/*
			Tooltip.SetDefault("Pierce the heart of even the most heavily-armored foe\n" +
				"Fires a .70 caliber sniper round that bypasses enemy defense and DR\n" +
				"Rounds mark enemies for death and summon a swarm of additional bullets on crits");
*/
		}

		public override void SetDefaults()
		{
			Item.damage = 4200;
			Item.crit += 35;
			Item.knockBack = 9.5f;
			Item.DamageType = DamageClass.Ranged;
			Item.useTime = 55;
			Item.useAnimation = 55;
			Item.shoot = ProjectileID.BulletHighVelocity;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Bullet;
			Item.autoReuse = true;

			Item.width = 94;
			Item.height = 32;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/LargeWeaponFire");
			Item.value = CalamityGlobalItem.Rarity15BuyPrice;
			Item.rare = 10;
			Item.Calamity().customRarity = CalamityRarity.Dedicated;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Vortexpopper>());
			recipe.AddIngredient(ModContent.ItemType<GoldenEagle>());
			recipe.AddIngredient(ModContent.ItemType<AMR>());
			recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 4);
			recipe.AddTile(ModContent.TileType<DraedonsForge>());
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<PiercingBullet>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
