using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Typeless
{
	public class TheDeadlyMicrobeProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Microbe");
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 3600;
		}

		public override void AI()
		{
			Projectile.SporeSacAI();
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.CursedInferno, 90);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.CursedInferno, 90);
		}

		public override void OnKill(int timeLeft)
		{
			CalamityGlobalProjectile.ExpandHitboxBy(Projectile, 56);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			int dustAmt = 36;
			for (int d = 0; d < dustAmt; d++)
			{
				Vector2 source = Vector2.Normalize(Projectile.velocity) * new Vector2((float)Projectile.width / 2f, (float)Projectile.height) * 0.75f;
				source = source.RotatedBy((double)((float)(d - (dustAmt / 2 - 1)) * MathHelper.TwoPi / (float)dustAmt), default) + Projectile.Center;
				Vector2 dustVel = source - Projectile.Center;
				int index = Dust.NewDust(source + dustVel, 0, 0, 44, dustVel.X, dustVel.Y, 100, default, 0.5f);
				Main.dust[index].noGravity = true;
				Main.dust[index].noLight = true;
				Main.dust[index].velocity = dustVel;
			}
			Projectile.Damage();
		}
	}
}
