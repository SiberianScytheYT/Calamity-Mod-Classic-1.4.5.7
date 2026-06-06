using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Melee
{
    public class BalefulHarvesterProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pumpkin Skull");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 0f)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 50;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }

            int num123 = (int)Player.FindClosest(Projectile.Center, 1, 1);
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] < 110f && Projectile.ai[1] > 30f)
            {
                float scaleFactor2 = Projectile.velocity.Length();
                Vector2 vector17 = Main.player[num123].Center - Projectile.Center;
                vector17.Normalize();
                vector17 *= scaleFactor2;
                Projectile.velocity = (Projectile.velocity * 24f + vector17) / 25f;
                Projectile.velocity.Normalize();
                Projectile.velocity *= scaleFactor2;
            }

            if (Projectile.velocity.Length() < 18f)
            {
                Projectile.velocity *= 1.02f;
            }

            if (Projectile.velocity.X < 0f)
            {
                Projectile.spriteDirection = -1;
                Projectile.rotation = (float)Math.Atan2((double)-(double)Projectile.velocity.Y, (double)-(double)Projectile.velocity.X);
            }
            else
            {
                Projectile.spriteDirection = 1;
                Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
            }

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                for (int num124 = 0; num124 < 10; num124++)
                {
                    int num125 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Main.rand.NextBool(2) ? 5 : 6, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 2f);
                    Main.dust[num125].noGravity = true;
                    Main.dust[num125].velocity = Projectile.Center - Main.dust[num125].position;
                    Main.dust[num125].velocity.Normalize();
                    Main.dust[num125].velocity *= -5f;
                    Main.dust[num125].velocity += Projectile.velocity / 2f;
                }
            }
            else
            {
                for (int num157 = 0; num157 < 2; num157++)
                {
                    int num158 = Dust.NewDust(new Vector2(Projectile.position.X + 4f, Projectile.position.Y + 4f), Projectile.width - 8, Projectile.height - 8, Main.rand.NextBool(2) ? 5 : 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f);
                    Main.dust[num158].position -= Projectile.velocity * 2f;
                    Main.dust[num158].noGravity = true;
                    Dust expr_7A4A_cp_0_cp_0 = Main.dust[num158];
                    expr_7A4A_cp_0_cp_0.velocity.X *= 0.3f;
                    Dust expr_7A65_cp_0_cp_0 = Main.dust[num158];
                    expr_7A65_cp_0_cp_0.velocity.Y *= 0.3f;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, Main.rand.Next(0, 128));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int num621 = 0; num621 < 5; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 5, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 10; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 600);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int k = 0; k < 2; k++)
                {
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 174, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)Main.rand.Next(-35, 36) * 0.2f, (float)Main.rand.Next(-35, 36) * 0.2f, ModContent.ProjectileType<TinyFlare>(),
                     (int)((double)Projectile.damage * 0.35), Projectile.knockBack * 0.35f, Main.myPlayer, 0f, 0f);
                }
            }
        }
    }
}
