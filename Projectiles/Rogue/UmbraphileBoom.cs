using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalRD.Buffs.DamageOverTime;
namespace CalRD.Projectiles.Rogue
{
	public class UmbraphileBoom : ModProjectile
	{
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Explosion");
		}

		public override void SetDefaults()
		{
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.Calamity().rogue = true;
		}

		public override void OnKill(int timeLeft)
		{
			int dustType = Main.rand.NextBool(2) ? 246 : 176;
			bool flag15 = false;
			bool flag16 = false;
			if (Projectile.velocity.X < 0f && Projectile.position.X < Projectile.ai[0])
			{
				flag15 = true;
			}
			if (Projectile.velocity.X > 0f && Projectile.position.X > Projectile.ai[0])
			{
				flag15 = true;
			}
			if (Projectile.velocity.Y < 0f && Projectile.position.Y < Projectile.ai[1])
			{
				flag16 = true;
			}
			if (Projectile.velocity.Y > 0f && Projectile.position.Y > Projectile.ai[1])
			{
				flag16 = true;
			}
			if (flag15 && flag16)
			{
				Projectile.Kill();
			}
			float num461 = 25f;
			if (Projectile.ai[0] > 180f)
			{
				num461 -= (Projectile.ai[0] - 180f) / 2f;
			}
			if (num461 <= 0f)
			{
				num461 = 0f;
				Projectile.Kill();
			}
			num461 *= 0.7f;
			Projectile.ai[0] += 4f;
			int num462 = 0;
			while ((float)num462 < num461)
			{
				float num463 = (float)Main.rand.Next(-10, 11);
				float num464 = (float)Main.rand.Next(-10, 11);
				float num465 = (float)Main.rand.Next(3, 9);
				float num466 = (float)Math.Sqrt((double)(num463 * num463 + num464 * num464));
				num466 = num465 / num466;
				num463 *= num466;
				num464 *= num466;
				int num467 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 1f);
				Dust dust = Main.dust[num467];
				dust.noGravity = true;
				dust.position.X = Projectile.Center.X;
				dust.position.Y = Projectile.Center.Y;
				dust.position.X += (float)Main.rand.Next(-10, 11);
				dust.position.Y += (float)Main.rand.Next(-10, 11);
				dust.velocity.X = num463;
				dust.velocity.Y = num464;
				num462++;
			}
			return;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.dayTime || Main.rand.NextBool(3)) //100% during day, 33.33% chance at night
				target.AddBuff(BuffID.Daybreak, 60);

			if (!Main.dayTime || Main.rand.NextBool(3)) //100% at night, 33.33% chance during day
				target.AddBuff(ModContent.BuffType<Nightwither>(), 60);
		}

		//public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
		/*
		{
			if (!Main.dayTime || Main.rand.NextBool(3)) //100% at night, 33.33% chance during day
				target.AddBuff(ModContent.BuffType<Nightwither>(), 60);
		}
		*/
	}
}
