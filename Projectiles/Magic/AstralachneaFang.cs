using CalRD.Buffs.DamageOverTime;
using CalRD.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Magic
{
	public class AstralachneaFang : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Fang");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 240;
		}

		public override void AI()
		{
			int astral = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), 0f, 0f, 100, default, 1f);
			Main.dust[astral].noGravity = true;

			Projectile.rotation = Projectile.velocity.ToRotation();

			CalamityGlobalProjectile.HomeInOnNPC(Projectile, false, 250f, 13f, 25f);
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			for (int k = 0; k < 3; k++)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), 0f, 0f);
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, ModContent.DustType<AstralBlue>(), 0f, 0f);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 180);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
			return false;
		}
	}
}
