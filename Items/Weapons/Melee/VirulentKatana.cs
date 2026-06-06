using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
	public class VirulentKatana : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Virulence");
/*
			Tooltip.SetDefault("Fires a plague cloud");
*/
		}

		public override void SetDefaults()
		{
			Item.damage = 96;
			Item.knockBack = 5.5f;
			Item.useAnimation = Item.useTime = 15;
			Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<PlagueDust>();

			Item.width = 48;
			Item.height = 62;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.buyPrice(0, 80, 0, 0);
			Item.rare = 8;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, (int)(damage * 0.85), Item.knockBack, player.whoAmI, 0f, 0f);
			return false;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Plague>(), 240);
		}

		public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(ModContent.BuffType<Plague>(), 240);
		}
	}
}
