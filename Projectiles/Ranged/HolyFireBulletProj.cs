using CalRD.Buffs.DamageOverTime;
using CalRD.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Ranged
{
    public class HolyFireBulletProj : ModProjectile
    {
        private const int Lifetime = 600;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Holy Fire Bullet");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = Lifetime;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Projectile.spriteDirection = Projectile.direction;

            // Flaking dust
            if (Main.rand.NextBool())
            {
                float scale = Main.rand.NextFloat(0.6f, 1.6f);
                int dustID = Dust.NewDust(Projectile.Center, 1, 1, 244);
                Main.dust[dustID].position = Projectile.Center;
                Main.dust[dustID].noGravity = true;
                Main.dust[dustID].scale = scale;
                float angleDeviation = 0.17f;
                float angle = Main.rand.NextFloat(-angleDeviation, angleDeviation);
                Vector2 sprayVelocity = Projectile.velocity.RotatedBy(angle) * 0.6f;
                Main.dust[dustID].velocity = sprayVelocity;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(1f, 1f, 1f, 0f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(Projectile, lightColor, ProjectileID.Sets.TrailingMode[Projectile.type], 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int blastDamage = (int)(Projectile.damage * 0.85f);
                float scale = 0.85f + Main.rand.NextFloat() * 1.15f;
                int boom = Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FuckYou>(), blastDamage, Projectile.knockBack, Projectile.owner, 0f, scale);
                Main.projectile[boom].Calamity().forceRanged = true;
            }
            for (int k = 0; k < 4; k++)
            {
                float scale = Main.rand.NextFloat(1.4f, 1.8f);
                int dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 244);
                Main.dust[dustID].noGravity = false;
                Main.dust[dustID].scale = scale;
                float angleDeviation = 0.25f;
                float angle = Main.rand.NextFloat(-angleDeviation, angleDeviation);
                float velMult = Main.rand.NextFloat(0.08f, 0.14f);
                Vector2 shrapnelVelocity = Projectile.oldVelocity.RotatedBy(angle) * velMult;
                Main.dust[dustID].velocity = shrapnelVelocity;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
        }
    }
}
