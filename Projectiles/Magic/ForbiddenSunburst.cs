using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Magic
{
    public class ForbiddenSunburst : ModProjectile
    {
        public override string Texture => "CalRD/Projectiles/InvisibleProj";

        private static float ExplosionRadius = 190.0f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Sunburst");
        }

        public override void SetDefaults()
        {
            Projectile.width = 220;
            Projectile.height = 220;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 150;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.75f, 0.5f, 0f);
            if (Projectile.wet && !Projectile.lavaWet)
            {
                Projectile.Kill();
            }
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
                Projectile.localAI[0] += 1f;
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
                float num463 = (float)Main.rand.Next(-25, 26);
                float num464 = (float)Main.rand.Next(-25, 26);
                float num465 = (float)Main.rand.Next(9, 24);
                float num466 = (float)Math.Sqrt((double)(num463 * num463 + num464 * num464));
                num466 = num465 / num466;
                num463 *= num466;
                num464 *= num466;
                int num467 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, 0f, 0f, 100, default, 2.5f);
                Main.dust[num467].noGravity = true;
                Main.dust[num467].position.X = Projectile.Center.X;
                Main.dust[num467].position.Y = Projectile.Center.Y;
                Dust expr_149DF_cp_0 = Main.dust[num467];
                expr_149DF_cp_0.position.X += (float)Main.rand.Next(-10, 11);
                Dust expr_14A09_cp_0 = Main.dust[num467];
                expr_14A09_cp_0.position.Y += (float)Main.rand.Next(-10, 11);
                Main.dust[num467].velocity.X = num463;
                Main.dust[num467].velocity.Y = num464;
                num462++;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
            target.AddBuff(BuffID.OnFire, 600);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float dist1 = Vector2.Distance(Projectile.Center, targetHitbox.TopLeft());
            float dist2 = Vector2.Distance(Projectile.Center, targetHitbox.TopRight());
            float dist3 = Vector2.Distance(Projectile.Center, targetHitbox.BottomLeft());
            float dist4 = Vector2.Distance(Projectile.Center, targetHitbox.BottomRight());

            float minDist = dist1;
            if (dist2 < minDist)
                minDist = dist2;
            if (dist3 < minDist)
                minDist = dist3;
            if (dist4 < minDist)
                minDist = dist4;

            return minDist <= ExplosionRadius;
        }
    }
}
