using CalRD.Buffs.DamageOverTime;
using CalRD.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using CalRD.Projectiles.BaseProjectiles;
namespace CalRD.Projectiles.Melee.Spears
{
    public class SpatialLanceProjectile : BaseSpearProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Lance");
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;  //The width of the .png file in pixels divided by 2.
            // Projectile.aiStyle = 19;
            Projectile.DamageType = DamageClass.Melee;  //Dictates whether this is a melee-class weapon.
            Projectile.timeLeft = 90;
            Projectile.height = 40;  //The height of the .png file in pixels divided by 2.
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
            //projectile.Calamity().trueMelee = true;
        }

        public override float InitialSpeed => 3f;
        public override float ReelbackSpeed => 1.1f;
        public override float ForwardSpeed => 0.6f;
        public override Action<Projectile> EffectBeforeReelback => (proj) =>
        {
            Projectile.NewProjectile(Entity.GetSource_FromThis(), Projectile.Center.X + Projectile.velocity.X, Projectile.Center.Y + Projectile.velocity.Y,
                           Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<SpatialSpear>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
        };
        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(5))
            {
                int idx = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 66, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                Main.dust[idx].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
            target.AddBuff(ModContent.BuffType<GlacialState>(), 120);
            target.AddBuff(ModContent.BuffType<Plague>(), 120);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
        }
    }
}
