using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Boss
{
    public class BrimstoneLaserSplit : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/Boss/BrimstoneLaser";

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Brimstone Homing Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.hostile = true;
            Projectile.scale = 2f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 480;
            Projectile.alpha = 120;
        }

        public override void AI()
        {
            int num103 = (int)Player.FindClosest(Projectile.Center, 1, 1);
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] < 150f && Projectile.ai[1] > 90f)
            {
                float scaleFactor2 = Projectile.velocity.Length();
                Vector2 vector11 = Main.player[num103].Center - Projectile.Center;
                vector11.Normalize();
                vector11 *= scaleFactor2;
                Projectile.velocity = (Projectile.velocity * 24f + vector11) / 25f;
                Projectile.velocity.Normalize();
                Projectile.velocity *= scaleFactor2;
            }
            else if (Projectile.ai[1] >= 150f)
            {
                if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 16f)
                {
                    Projectile.velocity.X *= 1.01f;
                    Projectile.velocity.Y *= 1.01f;
                }
            }
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            Lighting.AddLight(Projectile.Center, 0.3f, 0f, 0f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 50, 50, Projectile.alpha);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
    }
}
