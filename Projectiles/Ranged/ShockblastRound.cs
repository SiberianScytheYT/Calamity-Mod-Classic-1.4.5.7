using CalRD.Projectiles.Healing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
    public class ShockblastRound : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Round");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
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
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Shockblast>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 120f)
            {
                Projectile.damage = (int)(Projectile.damage * 0.995);
                Projectile.knockBack = Projectile.knockBack * 0.995f;
            }
        }

        public override bool PreAI()
        {
            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi) + MathHelper.ToRadians(90) * Projectile.direction;

            for (int num136 = 0; num136 < 2; num136++)
            {
                Vector2 dspeed = -Projectile.velocity * Main.rand.NextFloat(0.5f, 0.7f);
                float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * (float)num136;
                float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * (float)num136;
                int num137 = Dust.NewDust(new Vector2(x2, y2), 1, 1, 185, 0f, 0f, 0, default, 1f);
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
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Shockblast>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
            }
            if (target.type == NPCID.TargetDummy || Main.player[Projectile.owner].moonLeech)
            {
                return;
            }
            float healAmt = (float)Projectile.damage * 0.05f;
            if ((int)healAmt == 0)
            {
                return;
            }
            if (Main.player[Main.myPlayer].lifeSteal <= 0f)
            {
                return;
            }
			CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], healAmt, ModContent.ProjectileType<TransfusionTrail>(), 1200f, 1.5f);
        }
    }
}
