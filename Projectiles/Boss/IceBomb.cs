using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class IceBomb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ice Bomb");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.99f;

			if (Projectile.localAI[0] == 0f)
			{
				Projectile.scale += 0.01f;
				Projectile.alpha -= 50;
				if (Projectile.alpha <= 0)
				{
					Projectile.localAI[0] = 1f;
					Projectile.alpha = 0;
				}
			}
			else
			{
				Projectile.scale -= 0.01f;
				Projectile.alpha += 50;
				if (Projectile.alpha >= 255)
				{
					Projectile.localAI[0] = 0f;
					Projectile.alpha = 255;
				}
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.dayTime ? new Color(50, 50, 255, Projectile.alpha) : new Color(255, 255, 255, Projectile.alpha);
		}

		public override void OnKill(int timeLeft)
        {
            Lighting.AddLight((int)((Projectile.position.X + (Projectile.width / 2)) / 16f), (int)((Projectile.position.Y + (Projectile.height / 2)) / 16f), 0.01f, 0.25f, 0.25f);
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            float spread = 60f * 0.0174f;
            double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle;
            int i;
            if (Projectile.owner == Main.myPlayer)
            {
                for (i = 0; i < 3; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<IceRain>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1f, 0f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<IceRain>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1f, 0f);
                }
            }
            for (int k = 0; k < 3; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 67, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frostburn, 120, true);
            target.AddBuff(BuffID.Chilled, 90, true);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 60);
        }
    }
}
