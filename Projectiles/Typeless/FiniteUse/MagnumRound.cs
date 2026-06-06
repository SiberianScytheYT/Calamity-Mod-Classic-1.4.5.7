using CalRD.CalPlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Projectiles.Typeless.FiniteUse
{
    public class MagnumRound : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Magnum Round");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.BulletHighVelocity;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // Crits are extra powerful, dealing 2.5x damage instead of 2x.
            if (modifiers.ToHitInfo(target.damage, true, modifiers.Knockback.Base, false, 0f).Crit)
            {
                Projectile.damage = (int)(Projectile.damage * 1.25);
                Projectile.knockBack *= 1.25f;
            }

            if (target.Organic())
                Projectile.damage += target.lifeMax / 25; //400 + 80 = 480 + (100000 / 25 = 4000) = 4480, if crit = 5600 = 5.6% of boss HP

            // Shots are hard capped at 6.6% of the entity's max health, meaning if you shoot a non-boss, you're an idiot.
            if (Projectile.damage > target.lifeMax / 15 && CalamityPlayer.areThereAnyDamnBosses)
                Projectile.damage = target.lifeMax / 15;
        }
    }
}
