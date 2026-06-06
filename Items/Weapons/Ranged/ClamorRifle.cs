using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Ranged
{
	public class ClamorRifle : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Clamor Rifle");
/*
			Tooltip.SetDefault("Shoots homing energy bolts");
*/
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 64;
			Item.height = 30;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2.5f;
			Item.value = Item.buyPrice(0, 36, 0, 0);
			Item.UseSound = new SoundStyle("CalRD/Sounds/Item/PlasmaBolt");
			Item.autoReuse = true;
			Item.rare = 5;
			Item.shoot = ModContent.ProjectileType<ClamorRifleProj>();
			Item.shootSpeed = 15f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<ClamorRifleProj>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
