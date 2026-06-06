using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Magic
{
    public class VenusianBolt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bolt");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(45);
            Lighting.AddLight(Projectile.Center, 0.25f, 0.2f, 0f);
            if (Projectile.wet && !Projectile.lavaWet)
            {
                Projectile.Kill();
            }
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item73, Projectile.position);
                Projectile.localAI[0] += 1f;
            }
            for (int num457 = 0; num457 < 3; num457++)
            {
                int num458 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 55, 0f, 0f, 100, default, 1.2f);
                Main.dust[num458].noGravity = true;
                Main.dust[num458].velocity *= 0.5f;
                Main.dust[num458].velocity += Projectile.velocity * 0.1f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 600);
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int explosionDamage = Projectile.damage;
                float explosionKB = 6f;
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<VenusianExplosion>(), explosionDamage, explosionKB, Projectile.owner, 0f, 0f);

                int cinderDamage = (int)(Projectile.damage * 0.75f);
                float cinderKB = 0f;
                Vector2 cinderPos = Projectile.oldPosition + 0.5f * Projectile.Size;
                int numCinders = Main.rand.Next(7, 10);
                for (int i = 0; i < numCinders; i++)
                {
                    Vector2 cinderVel = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    while (cinderVel.X == 0f && cinderVel.Y == 0f)
                    {
                        cinderVel = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    }
                    cinderVel.Normalize();
                    cinderVel *= (float)Main.rand.Next(70, 101) * 0.1f;
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), cinderPos, cinderVel, ModContent.ProjectileType<VenusianFlame>(), cinderDamage, cinderKB, Projectile.owner, 0f, 0f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }
    }
}
