using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Magic
{
    public class RougeSlashLarge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Slash");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 5;
            Projectile.alpha = 255;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.75f, 0f, 0f);
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale -= 0.01f;
                Projectile.alpha += 15;
                if (Projectile.alpha >= 250)
                {
                    Projectile.alpha = 255;
                    Projectile.localAI[0] = 1f;
                }
            }
            else if (Projectile.localAI[0] == 1f)
            {
                Projectile.scale += 0.01f;
                Projectile.alpha -= 15;
                if (Projectile.alpha <= 0)
                {
                    Projectile.alpha = 0;
                    Projectile.localAI[0] = 0f;
                }
            }
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] == 12f)
            {
                Projectile.localAI[1] = 0f;
                for (int l = 0; l < 12; l++)
                {
                    Vector2 vector3 = Vector2.UnitX * (float)-(float)Projectile.width / 2f;
                    vector3 += -Vector2.UnitY.RotatedBy((double)((float)l * 3.14159274f / 6f), default) * new Vector2(8f, 16f);
                    vector3 = vector3.RotatedBy((double)(Projectile.rotation - 1.57079637f), default);
                    int num9 = Dust.NewDust(Projectile.Center, 0, 0, 60, 0f, 0f, 160, default, 1f);
                    Main.dust[num9].scale = 1.1f;
                    Main.dust[num9].noGravity = true;
                    Main.dust[num9].position = Projectile.Center + vector3;
                    Main.dust[num9].velocity = Projectile.velocity * 0.1f;
                    Main.dust[num9].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[num9].position) * 1.25f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity *= 0.25f;
            target.immune[Projectile.owner] = 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
