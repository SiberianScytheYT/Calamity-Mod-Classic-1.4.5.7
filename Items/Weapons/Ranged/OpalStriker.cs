using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
	public class OpalStriker : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Opal Striker");
/*
			Tooltip.SetDefault("Fires a string of opal strikes");
*/
		}

		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 64;
			Item.height = 16;
			Item.useTime = 5;
			Item.reuseDelay = 25;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0f;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = 2;
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/OpalStrike");
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<OpalStrike>();
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<OpalStrike>(), damage, Item.knockBack, player.whoAmI);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Marble, 20);
			recipe.AddIngredient(ItemID.Amber, 5);
			recipe.AddIngredient(ItemID.Diamond, 3);
			recipe.AddIngredient(ItemID.FlintlockPistol);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
