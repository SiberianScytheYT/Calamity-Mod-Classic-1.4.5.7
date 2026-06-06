using CalRD.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Projectiles.Ranged
{
    public class SicknessRound : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Round");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.25f;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override bool PreAI()
        {
            for (int num136 = 0; num136 < 3; num136++)
            {
                Vector2 dspeed = -Projectile.velocity * 0.5f;
                float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)num136;
                float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)num136;
                int num137 = Dust.NewDust(new Vector2(x2, y2), 1, 1, 107, dspeed.X, dspeed.Y, 0, default, 1f);
                Main.dust[num137].alpha = Projectile.alpha;
                Main.dust[num137].position.X = x2;
                Main.dust[num137].position.Y = y2;
                Main.dust[num137].velocity = dspeed;
                Main.dust[num137].noGravity = true;
            }
            float num138 = (float)Math.Sqrt((double)(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y));
            float num139 = Projectile.localAI[0];
            if (num139 == 0f)
            {
                Projectile.localAI[0] = num138;
            }
            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = (Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi)) + MathHelper.ToRadians(90) * Projectile.direction;

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 360);
            target.AddBuff(BuffID.Venom, 360);
            target.AddBuff(ModContent.BuffType<Plague>(), 360);
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Sickness>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                for (int k = 0; k < 3; k++)
                {
                    Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 107, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                    Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)Main.rand.Next(-35, 36) * 0.2f, (float)Main.rand.Next(-35, 36) * 0.2f, ModContent.ProjectileType<SicknessRound2>(),
                    (int)((double)Projectile.damage * 0.5), (float)(int)((double)Projectile.knockBack * 0.5), Main.myPlayer, 0f, 0f);
                }
            }
        }
    }
}
