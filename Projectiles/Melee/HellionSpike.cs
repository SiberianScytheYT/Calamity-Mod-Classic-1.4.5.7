using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
	public class HellionSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Spike");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = 27;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 600;
			AIType = ProjectileID.SporeCloud;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 0f, 0.25f, 0f);
			if (Main.rand.NextBool(3))
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 44, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 44, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft > 595)
				return false;

			CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 2);
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 8;
			OnHitEffects(target.Center, hit.Crit);
			target.AddBuff(BuffID.Venom, 300);
		}

		//public override void OnHitPvp(Player target, int damage, bool crit)/* tModPorter Note: Removed. Use OnHitPlayer and check info.PvP */
		/*
		{
			OnHitEffects(target.Center, crit);
			target.AddBuff(BuffID.Venom, 300);
		}
		*/

		private void OnHitEffects(Vector2 targetPos, bool crit)
		{
            if (crit)
            {
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile petal = CalamityUtils.ProjectileBarrage(Projectile.GetSource_FromThis(), Projectile.Center, targetPos, Main.rand.NextBool(), 800f, 800f, 0f, 800f, 10f, ProjectileID.FlowerPetal, (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.5f, Projectile.owner, true);
					petal.Calamity().forceMelee = true;
					petal.localNPCHitCooldown = -1;
				}
            }
        }
	}
}
